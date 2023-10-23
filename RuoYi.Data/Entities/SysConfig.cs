using SqlSugar;

namespace RuoYi.Data.Entities
{
    /// <summary>
    ///  参数配置表 
    /// </summary>
    [SugarTable("sys_config", "参数配置表")]
    public class SysConfig : UserBaseEntity
    {
        /// <summary>
        /// 参数主键 (config_id)
        /// </summary>
        [SugarColumn(ColumnName = "config_id", ColumnDescription = "参数主键", IsPrimaryKey = true, IsIdentity = true)]
        public int ConfigId { get; set; }
        /// <summary>
        /// 参数名称 (config_name)
        /// </summary>
        [SugarColumn(ColumnName = "config_name", ColumnDescription = "参数名称")]
        public string? ConfigName { get; set; }
        /// <summary>
        /// 参数键名 (config_key)
        /// </summary>
        [SugarColumn(ColumnName = "config_key", ColumnDescription = "参数键名")]
        public string? ConfigKey { get; set; }
        /// <summary>
        /// 参数键值 (config_value)
        /// </summary>
        [SugarColumn(ColumnName = "config_value", ColumnDescription = "参数键值")]
        public string? ConfigValue { get; set; }
        /// <summary>
        /// 系统内置（Y是 N否） (config_type)
        /// </summary>
        [SugarColumn(ColumnName = "config_type", ColumnDescription = "系统内置（Y是 N否）")]
        public string? ConfigType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注")]
        public string? Remark { get; set; }
    }
}
