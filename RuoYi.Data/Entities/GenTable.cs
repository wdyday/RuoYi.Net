using SqlSugar;

namespace RuoYi.Data.Entities
{
    [SugarTable("gen_table", "代码生成业务表")]
    public class GenTable : UserBaseEntity
    {
        /** 编号 */
        [SugarColumn(ColumnName = "table_id", IsPrimaryKey = true, IsIdentity = true)]
        public long TableId { get; set; }

        /** 表名称 */
        //NotBlank(message = "表名称不能为空")
        [SugarColumn(ColumnName = "table_name", ColumnDescription = "表名称")]
        public string? TableName { get; set; }

        /** 表描述 */
        //NotBlank(message = "表描述不能为空")
        [SugarColumn(ColumnName = "table_comment", ColumnDescription = "表描述")]
        public string? TableComment { get; set; }

        /** 关联父表的表名 */
        [SugarColumn(ColumnName = "sub_table_name", ColumnDescription = "关联父表的表名")]
        public string? SubTableName { get; set; }

        /** 本表关联父表的外键名 */
        [SugarColumn(ColumnName = "sub_table_fk_name", ColumnDescription = "本表关联父表的外键名")]
        public string? SubTableFkName { get; set; }

        /** 实体类名称(首字母大写) */
        //NotBlank(message = "实体类名称不能为空")
        [SugarColumn(ColumnName = "class_name", ColumnDescription = "实体类名称(首字母大写)")]
        public string? ClassName { get; set; }

        /** 使用的模板（crud单表操作 tree树表操作 sub主子表操作） */
        [SugarColumn(ColumnName = "tpl_category", ColumnDescription = "使用的模板")]
        public string? TplCategory { get; set; }

        /** 生成包路径 */
        [SugarColumn(ColumnName = "package_name", ColumnDescription = "生成包路径")]
        public string? PackageName { get; set; }

        /** 生成模块名 */
        //NotBlank(message = "生成模块名不能为空")
        [SugarColumn(ColumnName = "module_name", ColumnDescription = "生成模块名")]
        public string? ModuleName { get; set; }

        /** 生成业务名 */
        [SugarColumn(ColumnName = "business_name", ColumnDescription = "生成业务名")]
        public string? BusinessName { get; set; }

        /** 生成功能名 */
        [SugarColumn(ColumnName = "function_name", ColumnDescription = "生成功能名")]
        public string? FunctionName { get; set; }

        /** 生成作者 */
        [SugarColumn(ColumnName = "function_author", ColumnDescription = "生成作者")]
        public string? FunctionAuthor { get; set; }

        /** 生成代码方式（0zip压缩包 1自定义路径） */
        [SugarColumn(ColumnName = "gen_type", ColumnDescription = "生成代码方式")]
        public string? GenType { get; set; }

        /** 生成路径（不填默认项目路径） */
        [SugarColumn(ColumnName = "gen_path", ColumnDescription = "生成路径")]
        public string? GenPath { get; set; }

        /** 其它生成选项 */
        [SugarColumn(ColumnName = "options", ColumnDescription = "其它生成选项")]
        public string? Options { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注")]
        public string? Remark { get; set; }

        /** 表列信息 */
        [Navigate(NavigateType.OneToMany, nameof(GenTableColumn.TableId))]
        public List<GenTableColumn>? Columns { get; set; }

        #region 非DB字段

        /** 主键信息 */
        [SugarColumn(IsIgnore = true)]
        public GenTableColumn? PkColumn { get; set; }

        /** 子表信息 */
        [SugarColumn(IsIgnore = true)]
        public GenTable? SubTable { get; set; }
        #endregion

        #region methods
        /** Entity基类字段 */
        public static string[] BASE_ENTITY = { "CreateBy", "CreateTime", "UpdateBy", "UpdateTime", "Remark" };

        /** Tree基类字段 */
        public static string[] TREE_ENTITY = { "ParentName", "ParentId", "OrderNum", "Ancestors", "Children" };

        public bool IsCrud()
        {
            return !string.IsNullOrEmpty(TplCategory) && "crud".Equals(TplCategory);
        }
        public bool IsSub()
        {
            return !string.IsNullOrEmpty(TplCategory) && "sub".Equals(TplCategory);
        }
        public bool IsTree()
        {
            return IsTree(TplCategory);
        }
        public bool IsTree(string tplCategory)
        {
            return !string.IsNullOrEmpty(tplCategory) && "tree".Equals(TplCategory);
        }

        public bool IsSuperColumn(string netField)
        {
            return IsSuperColumn(TplCategory, netField);
        }

        public bool IsSuperColumn(string tplCategory, string netField)
        {
            if (IsTree(tplCategory))
            {
                return TREE_ENTITY.Contains(netField) || BASE_ENTITY.Contains(netField);
            }
            return BASE_ENTITY.Contains(netField);
        }
        #endregion
    }
}
