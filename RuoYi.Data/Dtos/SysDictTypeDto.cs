using RuoYi.Data.Attributes;
using System.ComponentModel.DataAnnotations;

namespace RuoYi.Data.Dtos
{
    /// <summary>
    ///  字典类型表 对象 sys_dict_type
    ///  author ruoyi
    ///  date   2023-09-04 17:49:59
    /// </summary>
    public class SysDictTypeDto : BaseDto
    {
        /// <summary>
        /// 字典主键
        /// </summary>
        [Excel(Name = "字典主键")]
        public long? DictId { get; set; }

        /// <summary>
        /// 字典名称
        /// </summary>
        [Excel(Name = "字典名称")]
        public string? DictName { get; set; }

        /// <summary>
        /// 字典类型
        /// </summary>
        [Excel(Name = "字典类型")]
        [RegularExpression(@"^[a-z][a-z0-9_]*$", ErrorMessage = "字典类型必须以字母开头，且只能为（小写字母，数字，下滑线）")]
        public string? DictType { get; set; }

        /// <summary>
        /// 状态（0正常 1停用）
        /// </summary>
        public string? Status { get; set; }

        [Excel(Name = "状态")]
        public string? StatusDesc { get; set; }
    }
}
