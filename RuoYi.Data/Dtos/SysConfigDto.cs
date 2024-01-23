using RuoYi.Data.Attributes;
using System.ComponentModel.DataAnnotations;

namespace RuoYi.Data.Dtos
{
    /// <summary>
    ///  参数配置表 
    /// </summary>
    public class SysConfigDto : BaseDto
    {
        /// <summary>
        /// 参数主键 (config_id)
        /// </summary>
        [Excel(Name = "参数主键")]
        public int? ConfigId { get; set; }

        /// <summary>
        /// 参数名称 (config_name)
        /// </summary>
        [Excel(Name = "参数名称")]
        [Required(ErrorMessage = "参数名称不能为空"), MaxLength(100, ErrorMessage = "参数名称不能超过100个字符")]
        public string? ConfigName { get; set; }

        /// <summary>
        /// 参数键名 (config_key)
        /// </summary>
        [Excel(Name = "参数键名")]
        [Required(ErrorMessage = "参数键名不能为空"), MaxLength(100, ErrorMessage = "参数键名不能超过100个字符")]
        public string? ConfigKey { get; set; }

        /// <summary>
        /// 参数键值 (config_value)
        /// </summary>
        [Excel(Name = "参数键值")]
        [Required(ErrorMessage = "参数键值不能为空"), MaxLength(500, ErrorMessage = "参数键值不能超过500个字符")]
        public string? ConfigValue { get; set; }

        /// <summary>
        /// 系统内置（Y是 N否） (config_type)
        /// </summary>
        public string? ConfigType { get; set; }

        [Excel(Name = "系统内置")]
        public string? ConfigTypeDesc { get; set; }
    }
}
