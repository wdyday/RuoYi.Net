using ICSharpCode.SharpZipLib.Zip;
using RazorEngineCore;
using RuoYi.Common.Utils;
using RuoYi.Framework.Exceptions;
using RuoYi.Framework.Interceptors;
using RuoYi.Framework.JsonSerialization;
using RuoYi.Generator.Dtos;

namespace RuoYi.Generator.Services;

public class GenTableService : BaseService<GenTable, GenTableDto>, ITransient
{
    private readonly ILogger<GenTableService> _logger;
    private readonly IRazorEngine _razorEngine;

    private readonly GenTableRepository _genTableRepository;
    private readonly GenTableColumnRepository _genTableColumnRepository;
    //private readonly RepoSqlService _repoSqlService;

    public GenTableService(ILogger<GenTableService> logger,
        IRazorEngine razorEngine,
        GenTableRepository genTableRepository,
        GenTableColumnRepository genTableColumnRepository)
    {
        _logger = logger;
        _razorEngine = razorEngine;
        _genTableRepository = genTableRepository;
        _genTableColumnRepository = genTableColumnRepository;
        //_repoSqlService = repoSqlService;

        BaseRepo = genTableRepository;
    }

    /// <summary>
    /// 查询业务信息
    /// </summary>
    /// <param name="id">业务ID</param>
    /// <returns>业务信息</returns>
    public GenTableDto SelectGenTableById(long id)
    {
        var genTable = _genTableRepository.SelectGenTableById(id);
        return _genTableRepository.ToDto(genTable);
    }

    /// <summary>
    /// 查询业务列表
    /// </summary>
    /// <param name="dto">业务信息</param>
    /// <returns>业务集合</returns>
    public List<GenTable> SelectGenTableList(GenTableDto dto)
    {
        return _genTableRepository.GetList(dto);
    }

    /// <summary>
    /// 查询据库列表
    /// </summary>
    /// <param name="dto">业务信息</param>
    /// <returns>数据库表集合</returns>
    public SqlSugarPagedList<GenTable> SelectDbTableList(GenTableDto dto)
    {
        //var pageDomain = PageUtils.GetPageDomain();
        var queryable = _genTableRepository.DbQueryable(dto);
        return _genTableRepository.GetPagedList(queryable);
    }

    /// <summary>
    /// 查询据库列表
    /// </summary>
    /// <param name="tableNames">表名称组</param>
    /// <returns>数据库表集合</returns>
    public List<GenTable> SelectDbTableListByNames(string[] tableNames)
    {
        return _genTableRepository.SelectDbTableListByNames(tableNames);
    }

    /// <summary>
    /// 查询所有表信息
    /// </summary>
    /// <returns>表信息集合</returns>
    public List<GenTableDto> SelectGenTableAll()
    {
        var list = _genTableRepository.SelectGenTableAll();
        return _genTableRepository.ToDtos(list);
    }

    /// <summary>
    /// 修改业务
    /// </summary>
    /// <param name="dto">业务信息</param>
    public void UpdateGenTable(GenTableDto dto)
    {
        var options = new
        {
            treeCode = dto.TreeCode,
            treeName = dto.TreeName,
            treeParentCode = dto.TreeParentCode,
            parentMenuId = dto.ParentMenuId
        };
        dto.Options = JSON.Serialize(options);
        int row = _genTableRepository.Update(dto);

        if (row > 0 && !dto.Columns.IsEmpty())
        {
            _genTableColumnRepository.Update(dto.Columns);
        }
    }

    /// <summary>
    /// 删除业务对象
    /// </summary>
    /// <param name="tableIds">需要删除的数据ID</param>
    public void DeleteGenTableByIds(long[] tableIds)
    {
        if (tableIds.IsEmpty()) return;

        _genTableRepository.DeleteByKeys(tableIds);
        _genTableColumnRepository.Delete(d => tableIds.Contains(d.TableId ?? 0));
    }

    /// <summary>
    /// 导入表结构
    /// </summary>
    /// <param name="tableList">导入表列表</param>
    [Transactional]
    public virtual void ImportGenTable(List<GenTable> tableList)
    {
        string operName = SecurityUtils.GetUsername();
        try
        {
            foreach (GenTable table in tableList)
            {
                string tableName = table.TableName;
                GenUtils.InitTable(table, operName);
                _genTableRepository.Insert(table);
                if (table.TableId > 0)
                {
                    // 保存列信息
                    List<GenTableColumn> genTableColumns = _genTableColumnRepository.SelectDbTableColumnsByName(tableName);
                    foreach (GenTableColumn column in genTableColumns)
                    {
                        GenUtils.InitColumnField(column, table);
                    }
                    _genTableColumnRepository.InsertEntities(genTableColumns);
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "ImportGenTable error");
            throw new ServiceException($"导入失败：{e.Message}");
        }
    }

    public Dictionary<string, string> PreviewCode(long tableId)
    {
        Dictionary<string, string> dataMap = new Dictionary<string, string>();
        // 查询表信息
        GenTable table = _genTableRepository.SelectGenTableById(tableId);
        if (table == null) return dataMap;

        // 设置主子表信息
        SetSubTable(table);
        // 设置主键列信息
        SetPkColumn(table);

        var context = TemplateUtils.PrepareContext(table);

        // 获取模板列表
        List<string> templatePaths = TemplateUtils.GetTemplateList(_genTableRepository.GetDbType(), table.TplCategory);
        foreach (string templatePath in templatePaths)
        {
            //// 渲染模板
            //string tpl = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "StaticFiles", templatePath));
            //// https://github.com/adoconnection/RazorEngineCore
            //var template = _razorEngine.Compile<RazorEngineTemplateBase<TemplateContext>>(tpl, builder =>
            //{
            //    //builder.AddUsing("RuoYi.Data.Entities"); // using

            //    builder.AddAssemblyReferenceByName("System.Collections");  // by name
            //    builder.AddAssemblyReferenceByName("RuoYi.Data");  // by name

            //    builder.AddAssemblyReference(typeof(RuoYi.Generator.Dtos.TemplateContext)); // by type

            //    //builder.AddAssemblyReference(Assembly.Load("source")); // by reference
            //});
            //string text = template.Run(instance =>
            //{
            //    instance.Model = context;
            //});

            string text = TemplateUtils.Compile(context, templatePath);
            dataMap.Add(templatePath, text);
        }
        return dataMap;
    }

    /// <summary>
    /// 生成代码（下载方式）
    /// </summary>
    /// <param name="tableName">表名称</param>
    /// <returns></returns>
    public byte[] DownloadCode(string tableName)
    {
        using var outputStream = new MemoryStream();
        ZipOutputStream zip = new ZipOutputStream(outputStream);
        GeneratorCode(tableName, zip);
        ZipUtils.Close(zip);
        return outputStream.ToArray();
    }

    /// <summary>
    /// 批量生成代码（下载方式）
    /// </summary>
    /// <param name="tableNames">表数组</param>
    /// <returns></returns>
    public byte[] DownloadCode(string[] tableNames)
    {
        using var outputStream = new MemoryStream();
        ZipOutputStream zip = new ZipOutputStream(outputStream);
        foreach (string tableName in tableNames)
        {
            GeneratorCode(tableName, zip);
        }
        ZipUtils.Close(zip);
        return outputStream.ToArray();
    }

    public void GeneratorCode(string tableName)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 同步数据库
    /// </summary>
    /// <param name="tableName">表名称</param>
    public async Task SynchDbAsync(string tableName)
    {
        GenTable table = _genTableRepository.SelectGenTableByName(tableName);
        List<GenTableColumn> tableColumns = table.Columns;
        Dictionary<string, GenTableColumn> tableColumnMap = tableColumns.ToDictionary(c => c.ColumnName);

        List<GenTableColumn> dbTableColumns = _genTableColumnRepository.SelectDbTableColumnsByName(tableName);
        if (dbTableColumns.IsNullOrEmpty())
        {
            throw new ServiceException("同步数据失败，原表结构不存在");
        }
        List<string> dbTableColumnNames = dbTableColumns.Select(c => c.ColumnName).ToList();

        foreach (var column in dbTableColumns)
        {
            GenUtils.InitColumnField(column, table);
            if (tableColumnMap.ContainsKey(column.ColumnName))
            {
                GenTableColumn prevColumn = tableColumnMap[column.ColumnName];
                column.ColumnId = prevColumn.ColumnId;
                if (column.Is_List())
                {
                    // 如果是列表，继续保留查询方式/字典类型选项
                    column.DictType = prevColumn.DictType;
                    column.QueryType = prevColumn.QueryType;
                }
                if (!string.IsNullOrEmpty(prevColumn.IsRequired) && !column.Is_Pk()
                        && (column.Is_Insert() || column.Is_Edit())
                        && (column.IsUsableColumn() || !column.IsSuperColumn()))
                {
                    // 如果是(新增/修改&非主键/非忽略及父属性)，继续保留必填/显示类型选项
                    column.IsRequired = prevColumn.IsRequired;
                    column.HtmlType = prevColumn.HtmlType;
                }
                await _genTableColumnRepository.UpdateAsync(column);
            }
            else
            {
                await _genTableColumnRepository.InsertAsync(column);
            }
        }

        var delColumnNames = tableColumns
            .Where(column => !dbTableColumnNames.Contains(column.ColumnName))
            .Select(column => column.ColumnName).ToList();
        if (!delColumnNames.IsNullOrEmpty())
        {
            await _genTableColumnRepository.DeleteAsync(column => delColumnNames.Contains(column.ColumnName));
        }
    }

    public void ValidateEdit(GenTableDto genTable)
    {
        if (GenConstants.TPL_TREE.Equals(genTable.TplCategory))
        {
            if (string.IsNullOrEmpty(genTable.TreeCode?.ToString()))
            {
                throw new ServiceException("树编码字段不能为空");
            }
            else if (string.IsNullOrEmpty(genTable.TreeParentCode?.ToString()))
            {
                throw new ServiceException("树父编码字段不能为空");
            }
            else if (string.IsNullOrEmpty(genTable.TreeName?.ToString()))
            {
                throw new ServiceException("树名称字段不能为空");
            }
            else if (GenConstants.TPL_SUB.Equals(genTable.TplCategory))
            {
                if (string.IsNullOrEmpty(genTable.SubTableName))
                {
                    throw new ServiceException("关联子表的表名不能为空");
                }
                else if (string.IsNullOrEmpty(genTable.SubTableFkName))
                {
                    throw new ServiceException("子表关联的外键名不能为空");
                }
            }
        }
    }

    /**
     * 设置主键列信息
     * 
     * @param table 业务表信息
     */
    public void SetPkColumn(GenTable table)
    {
        if (table == null) return;

        foreach (GenTableColumn column in table.Columns)
        {
            if (column.Is_Pk())
            {
                table.PkColumn = column;
                break;
            }
        }
        if (table.PkColumn == null)
        {
            table.PkColumn = table.Columns[0];
        }
        if (GenConstants.TPL_SUB.Equals(table.TplCategory))
        {
            foreach (GenTableColumn column in table.SubTable.Columns)
            {
                if (column.Is_Pk())
                {
                    table.SubTable.PkColumn = column;
                    break;
                }
            }
            if (table.SubTable.PkColumn == null)
            {
                table.SubTable.PkColumn = table.SubTable.Columns[0];
            }
        }
    }

    /**
     * 设置主子表信息
     * 
     * @param table 业务表信息
     */
    public void SetSubTable(GenTable table)
    {
        string subTableName = table?.SubTableName;
        if (!string.IsNullOrEmpty(subTableName))
        {
            table.SubTable = _genTableRepository.SelectGenTableByName(subTableName);
        }
    }

    /// <summary>
    /// 查询表信息并生成zip文件
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="zip">ZipOutputStream</param>
    private void GeneratorCode(string tableName, ZipOutputStream zip)
    {
        // 查询表信息
        GenTable table = _genTableRepository.SelectGenTableByName(tableName);
        if (table == null) return;

        // 设置主子表信息
        SetSubTable(table);
        // 设置主键列信息
        SetPkColumn(table);

        TemplateContext context = TemplateUtils.PrepareContext(table);

        // 获取模板列表
        List<string> templates = TemplateUtils.GetTemplateList(_genTableRepository.GetDbType(), table.TplCategory);
        foreach (string template in templates)
        {
            // 渲染模板
            string text = TemplateUtils.Compile(context, template);
            try
            {
                // 添加到zip
                var fileName = TemplateUtils.GetFileName(template, table);
                ZipUtils.PutTextEntry(zip, fileName, text);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"渲染模板失败，表名: {table.TableName}");
            }
        }
    }

}
