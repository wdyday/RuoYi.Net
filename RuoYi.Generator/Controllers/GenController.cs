using RuoYi.Common.Enums;
using RuoYi.Framework;
using RuoYi.System;

namespace RuoYi.Generator.Controllers
{
    /// <summary>
    /// 代码生成 操作处理
    /// </summary>
    [ApiDescriptionSettings("Generator")]
    [Route("tool/[controller]")]
    public class GenController : ControllerBase
    {
        private readonly ILogger<GenController> _logger;
        private readonly GenTableService _genTableService;
        private readonly GenTableColumnService _genTableColumnService;

        public GenController(ILogger<GenController> logger,
            GenTableService genTableService,
            GenTableColumnService genTableColumnService)
        {
            _logger = logger;
            _genTableService = genTableService;
            _genTableColumnService = genTableColumnService;
        }

        /// <summary>
        /// 查询代码生成列表
        /// </summary>
        [HttpGet("list")]
        [AppAuthorize("tool:gen:list")]
        public async Task<SqlSugarPagedList<GenTable>> GenList([FromQuery] GenTableDto dto)
        {
            return await _genTableService.GetPagedListAsync(dto);
        }

        /// <summary>
        /// 修改代码生成业务
        /// </summary>
        [HttpGet("{tableId}")]
        [AppAuthorize("tool:gen:query")]
        public AjaxResult GetTableInfo(long tableId)
        {
            GenTableDto table = _genTableService.SelectGenTableById(tableId);
            List<GenTableDto> tables = _genTableService.SelectGenTableAll();
            List<GenTableColumn> list = _genTableColumnService.SelectGenTableColumnListByTableId(tableId);
            Dictionary<string, object> map = new Dictionary<string, object>();
            map.Add("info", table);
            map.Add("rows", list);
            map.Add("tables", tables);
            return AjaxResult.Success(map);
        }

        /// <summary>
        /// 查询数据库列表
        /// </summary>
        [HttpGet("db/list")]
        [AppAuthorize("tool:gen:list")]
        public SqlSugarPagedList<GenTable> TableList([FromQuery] GenTableDto dto)
        {
            return _genTableService.SelectDbTableList(dto);
        }

        /// <summary>
        /// 查询数据表字段列表
        /// </summary>
        [HttpGet("column/{tableId}")]
        [AppAuthorize("tool:gen:list")]
        public SqlSugarPagedList<GenTableColumn> ColumnList(long tableId)
        {
            var pagedList = new SqlSugarPagedList<GenTableColumn>();

            var list = _genTableColumnService.SelectGenTableColumnListByTableId(tableId);
            pagedList.Rows = list;
            pagedList.Total = list.Count;

            return pagedList;
        }

        /// <summary>
        /// 导入表结构（保存）
        /// </summary>    
        [HttpPost("importTable")]
        [AppAuthorize("tool:gen:import")]
        public AjaxResult ImportTable(string tables)
        {
            string[] tableNames = tables.Split(",");
            // 查询表信息
            List<GenTable> tableList = _genTableService.SelectDbTableListByNames(tableNames);
            _genTableService.ImportGenTable(tableList);

            return AjaxResult.Success();
        }

        /// <summary>
        /// 修改 代码生成业务
        /// </summary>
        [HttpPut("")]
        [AppAuthorize("tool:gen:edit")]
        [TypeFilter(typeof(RuoYi.Framework.DataValidation.DataValidationFilter))]
        [Log(Title = "代码生成", BusinessType = BusinessType.UPDATE)]
        public AjaxResult Edit([FromBody] GenTableDto genTable)
        {
            _genTableService.ValidateEdit(genTable);
            _genTableService.UpdateGenTable(genTable);

            return AjaxResult.Success();
        }

        /// <summary>
        /// 删除代码生成
        /// </summary>
        [HttpDelete("{tableIds}")]
        [AppAuthorize("tool:gen:remove")]
        [Log(Title = "代码生成", BusinessType = BusinessType.DELETE)]
        public AjaxResult Remove(long[] tableIds)
        {
            _genTableService.DeleteGenTableByIds(tableIds);

            return AjaxResult.Success();
        }

        /// <summary>
        /// 预览代码
        /// </summary>
        [HttpGet("preview/{tableId}")]
        [AppAuthorize("tool:gen:preview")]
        public AjaxResult Preview(long tableId)
        {
            Dictionary<string, string> dataMap = _genTableService.PreviewCode(tableId);
            return AjaxResult.Success(dataMap);
        }

        /// <summary>
        /// 生成代码（单表下载）
        /// </summary>
        [HttpGet("genCode/{tableName}")]
        [AppAuthorize("tool:gen:code")]
        [Log(Title = "代码生成", BusinessType = BusinessType.GENCODE)]
        public async Task<AjaxResult> GenCode(string tableName)
        {
            var data = _genTableService.DownloadCode(tableName);
            await GenUtils.ExportZipAsync(App.HttpContext.Response, data);

            return AjaxResult.Success();
        }

        /// <summary>
        /// 批量生成代码（多表下载）
        /// </summary>
        [HttpGet("batchGenCode")]
        [AppAuthorize("tool:gen:code")]
        [Log(Title = "代码生成", BusinessType = BusinessType.GENCODE)]
        public async Task DownloadBatch(string tables)
        {
            var tableNames = !string.IsNullOrEmpty(tables) ? tables.Split(',') : new string[0];
            var data = _genTableService.DownloadCode(tableNames);
            await GenUtils.ExportZipAsync(App.HttpContext.Response, data);
        }

        /// <summary>
        /// 同步数据库
        /// </summary>
        [HttpGet("synchDb/{tableName}")]
        [AppAuthorize("tool:gen:edit")]
        [Log(Title = "代码生成", BusinessType = BusinessType.UPDATE)]
        public async Task<AjaxResult> SyncDbAsync(string tableName)
        {
            await _genTableService.SynchDbAsync(tableName);
            return AjaxResult.Success();
        }
    }
}
