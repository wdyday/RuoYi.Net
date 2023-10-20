using SqlSugar;

namespace RuoYi.Data.Entities
{
    /// <summary>
    ///  字典数据表 对象 sys_dict_data
    ///  author ruoyi
    ///  date   2023-09-04 17:49:59
    /// </summary>
    [SugarTable("sys_dict_data", "字典数据表")]
    public class SysDictData : UserBaseEntity
    {
        /// <summary>
        /// 字典编码 (dict_code)
        /// </summary>
        [SugarColumn(ColumnName = "dict_code", ColumnDescription = "字典编码", IsPrimaryKey = true, IsIdentity = true)]
        public long DictCode { get; set; }
        /// <summary>
        /// 字典排序 (dict_sort)
        /// </summary>
        [SugarColumn(ColumnName = "dict_sort", ColumnDescription = "字典排序")]
        public int? DictSort { get; set; }
        /// <summary>
        /// 字典标签 (dict_label)
        /// </summary>
        [SugarColumn(ColumnName = "dict_label", ColumnDescription = "字典标签")]
        public string? DictLabel { get; set; }
        /// <summary>
        /// 字典键值 (dict_value)
        /// </summary>
        [SugarColumn(ColumnName = "dict_value", ColumnDescription = "字典键值")]
        public string? DictValue { get; set; }
        /// <summary>
        /// 字典类型 (dict_type)
        /// </summary>
        [SugarColumn(ColumnName = "dict_type", ColumnDescription = "字典类型")]
        public string? DictType { get; set; }
        /// <summary>
        /// 样式属性（其他样式扩展） (css_class)
        /// </summary>
        [SugarColumn(ColumnName = "css_class", ColumnDescription = "样式属性（其他样式扩展）")]
        public string? CssClass { get; set; }
        /// <summary>
        /// 表格回显样式 (list_class)
        /// </summary>
        [SugarColumn(ColumnName = "list_class", ColumnDescription = "表格回显样式")]
        public string? ListClass { get; set; }
        /// <summary>
        /// 是否默认（Y是 N否） (is_default)
        /// </summary>
        [SugarColumn(ColumnName = "is_default", ColumnDescription = "是否默认（Y是 N否）")]
        public string? IsDefault { get; set; }
        /// <summary>
        /// 状态（0正常 1停用） (status)
        /// </summary>
        [SugarColumn(ColumnName = "status", ColumnDescription = "状态（0正常 1停用）")]
        public string? Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注")]
        public string? Remark { get; set; }
    }
}
