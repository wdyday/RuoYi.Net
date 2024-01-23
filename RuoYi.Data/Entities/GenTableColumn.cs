using SqlSugar;

namespace RuoYi.Data.Entities
{
    [SugarTable("gen_table_column", "代码生成业务表字段")]
    public class GenTableColumn : UserBaseEntity
    {
        /// <summary>
        /// 编号 
        ///</summary>
        [SugarColumn(ColumnName = "column_id", IsPrimaryKey = true, IsIdentity = true)]
        public long ColumnId { get; set; }
        /// <summary>
        /// 归属表编号 
        ///</summary>
        [SugarColumn(ColumnName = "table_id")]
        public long? TableId { get; set; }
        /// <summary>
        /// 列名称 
        ///</summary>
        [SugarColumn(ColumnName = "column_name")]
        public string ColumnName { get; set; }
        /// <summary>
        /// 列描述 
        ///</summary>
        [SugarColumn(ColumnName = "column_comment")]
        public string ColumnComment { get; set; }
        /// <summary>
        /// 列类型 
        ///</summary>
        [SugarColumn(ColumnName = "column_type")]
        public string ColumnType { get; set; }
        /// <summary>
        /// Net类型 
        ///</summary>
        [SugarColumn(ColumnName = "net_type")]
        public string NetType { get; set; }
        /// <summary>
        /// Net字段名 
        ///</summary>
        [SugarColumn(ColumnName = "net_field")]
        public string NetField { get; set; }
        /// <summary>
        /// 是否主键（1是） 
        ///</summary>
        [SugarColumn(ColumnName = "is_pk")]
        public string? IsPk { get; set; }
        /// <summary>
        /// 是否自增（1是） 
        ///</summary>
        [SugarColumn(ColumnName = "is_increment")]
        public string? IsIncrement { get; set; }
        /// <summary>
        /// 是否必填（1是） 
        ///</summary>
        [SugarColumn(ColumnName = "is_required")]
        public string? IsRequired { get; set; }
        /// <summary>
        /// 是否为插入字段（1是） 
        ///</summary>
        [SugarColumn(ColumnName = "is_insert")]
        public string? IsInsert { get; set; }
        /// <summary>
        /// 是否编辑字段（1是） 
        ///</summary>
        [SugarColumn(ColumnName = "is_edit")]
        public string? IsEdit { get; set; }
        /// <summary>
        /// 是否列表字段（1是） 
        ///</summary>
        [SugarColumn(ColumnName = "is_list")]
        public string? IsList { get; set; }
        /// <summary>
        /// 是否查询字段（1是） 
        ///</summary>
        [SugarColumn(ColumnName = "is_query")]
        public string? IsQuery { get; set; }
        /// <summary>
        /// 查询方式（等于、不等于、大于、小于、范围） 
        /// 默认值: EQ
        ///</summary>
        [SugarColumn(ColumnName = "query_type")]
        public string QueryType { get; set; }
        /// <summary>
        /// 显示类型（文本框、文本域、下拉框、复选框、单选框、日期控件） 
        ///</summary>
        [SugarColumn(ColumnName = "html_type")]
        public string HtmlType { get; set; }
        /// <summary>
        /// 字典类型 
        /// 默认值: 
        ///</summary>
        [SugarColumn(ColumnName = "dict_type")]
        public string DictType { get; set; }
        /// <summary>
        /// 排序 
        ///</summary>
        [SugarColumn(ColumnName = "sort")]
        public int? Sort { get; set; }


        #region methods

        public bool Is_List()
        {
            return IsYes(IsList);
        }
        public bool Is_Pk()
        {
            return IsYes(IsPk);
        }
        public bool Is_Increment()
        {
            return IsYes(IsIncrement);
        }
        public bool Is_Required()
        {
            return IsYes(IsRequired);
        }
        public bool Is_Query()
        {
            return IsYes(IsQuery);
        }
        public bool Is_Insert()
        {
            return IsYes(IsInsert);
        }
        public bool Is_Edit()
        {
            return IsYes(IsEdit);
        }
        public static bool IsYes(string? yesNo)
        {
            return "1".Equals(yesNo);
        }

        private static List<string> _UsableColumns = new List<string> { "ParentId", "OrderNum", "Remark" };
        public bool IsUsableColumn(string netField)
        {
            // isSuperColumn()中的名单用于避免生成多余Domain属性，若某些属性在生成页面时需要用到不能忽略，则放在此处白名单
            return !string.IsNullOrEmpty(netField) && _UsableColumns.Contains(netField);
        }
        public bool IsUsableColumn()
        {
            return IsUsableColumn(NetField ?? "");
        }

        // BaseEntity: "createBy", "createTime", "updateBy", "updateTime", "remark",
        // TreeEntity: "parentName", "parentId", "orderNum", "ancestors"
        private static List<string> _SuperColumns = new List<string> { "CreateBy", "CreateTime", "UpdateBy", "UpdateTime", "Remark", "ParentName", "ParentId", "OrderNum", "Ancestors" };
        public bool IsSuperColumn(string netField)
        {
            return !string.IsNullOrEmpty(netField) && _SuperColumns.Contains(netField);
        }
        public bool IsSuperColumn()
        {
            return IsSuperColumn(NetField ?? "");
        }

        // 首字母小写的 NetField
        public string NetFieldLower()
        {
            if (!string.IsNullOrEmpty(NetField))
            {
                return string.Concat(NetField.First().ToString().ToLower(), NetField.AsSpan(1));
            }

            return NetField;
        }
        #endregion
    }
}
