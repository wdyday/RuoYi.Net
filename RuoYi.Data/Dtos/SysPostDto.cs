using RuoYi.Data.Attributes;
using System.ComponentModel.DataAnnotations;

namespace RuoYi.Data.Dtos
{
    /// <summary>
    ///  岗位信息表 对象 sys_post
    ///  author ruoyi
    ///  date   2023-08-30 13:21:36
    /// </summary>
    public class SysPostDto : BaseDto
    {
        /// <summary>
        /// 岗位ID
        /// </summary>
        [Excel(Name = "岗位序号")]
        public long? PostId { get; set; }

        /// <summary>
        /// 岗位编码
        /// </summary>
        [Excel(Name = "岗位编码")]
        [Required(ErrorMessage = "岗位编码不能为空"), MaxLength(64, ErrorMessage = "岗位编码长度不能超过64个字符")]
        public string? PostCode { get; set; }
        public string? PostCodeLike { get; set; }

        /// <summary>
        /// 岗位名称
        /// </summary>
        [Excel(Name = "岗位名称")]
        [Required(ErrorMessage = "岗位名称不能为空"), MaxLength(50, ErrorMessage = "岗位名称长度不能超过50个字符")]
        public string? PostName { get; set; }
        public string? PostNameLike { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        [Excel(Name = "岗位排序")]
        [Required(ErrorMessage = "显示顺序不能为空")]
        public int? PostSort { get; set; }

        /// <summary>
        /// 状态（0正常 1停用）
        /// </summary>
        public string? Status { get; set; }

        [Excel(Name = "状态")]
        public string? StatusDesc { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long? UserId { get; set; }
    }
}
