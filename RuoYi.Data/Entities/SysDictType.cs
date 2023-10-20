using SqlSugar;

namespace RuoYi.Data.Entities
{
    /// <summary>
    ///  字典类型表 对象 sys_dict_type
    ///  author ruoyi
    ///  date   2023-09-04 17:49:59
    /// </summary>
    [SugarTable("sys_dict_type", "字典类型表")]
    public class SysDictType : UserBaseEntity
    {
        /// <summary>
        /// 字典主键 (dict_id)
        /// </summary>
        [SugarColumn(ColumnName = "dict_id", ColumnDescription = "字典主键", IsPrimaryKey = true, IsIdentity = true)]
        public long DictId { get; set; }
        /// <summary>
        /// 字典名称 (dict_name)
        /// </summary>
        [SugarColumn(ColumnName = "dict_name", ColumnDescription = "字典名称")]
        public string? DictName { get; set; }
        /// <summary>
        /// 字典类型 (dict_type)
        /// </summary>
        [SugarColumn(ColumnName = "dict_type", ColumnDescription = "字典类型")]
        public string? DictType { get; set; }
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
