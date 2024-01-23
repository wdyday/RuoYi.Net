using RuoYi.Data.Attributes;
using System.ComponentModel.DataAnnotations;

namespace RuoYi.Data.Dtos
{
    /// <summary>
    ///  字典数据表 对象 sys_dict_data
    ///  author ruoyi
    ///  date   2023-09-04 17:49:59
    /// </summary>
    public class SysDictDataDto : BaseDto
    {
        /// <summary>
        /// 字典编码
        /// </summary>
        [Excel(Name = "字典编码")]
        public long DictCode { get; set; }

        /// <summary>
        /// 字典排序
        /// </summary>
        [Excel(Name = "字典排序")]
        public int? DictSort { get; set; }

        /// <summary>
        /// 字典标签
        /// </summary>
        [Excel(Name = "字典标签")]
        [Required(ErrorMessage = "字典标签不能为空"), StringLength(100, ErrorMessage = "字典标签长度不能超过100个字符")]
        public string? DictLabel { get; set; }

        /// <summary>
        /// 字典键值
        /// </summary>
        [Excel(Name = "字典键值")]
        [Required(ErrorMessage = "字典键值不能为空"), StringLength(100, ErrorMessage = "字典键值长度不能超过100个字符")]
        public string? DictValue { get; set; }

        /// <summary>
        /// 字典类型
        /// </summary>
        [Excel(Name = "字典类型")]
        [Required(ErrorMessage = "字典类型不能为空"), StringLength(100, ErrorMessage = "字典类型长度不能超过100个字符")]
        public string? DictType { get; set; }

        /// <summary>
        /// 样式属性（其他样式扩展）
        /// </summary>
        [StringLength(100, ErrorMessage = "样式属性长度不能超过100个字符")]
        public string? CssClass { get; set; }

        /// <summary>
        /// 表格回显样式
        /// </summary>
        public string? ListClass { get; set; }

        /// <summary>
        /// 是否默认（Y是 N否）
        /// </summary>
        public string? IsDefault { get; set; }

        [Excel(Name = "是否默认")]
        public string? IsDefaultDesc { get; set; }

        /// <summary>
        /// 状态（0正常 1停用）
        /// </summary>
        public string? Status { get; set; }

        [Excel(Name = "状态")]
        public string? StatusDesc { get; set; }
    }
}
