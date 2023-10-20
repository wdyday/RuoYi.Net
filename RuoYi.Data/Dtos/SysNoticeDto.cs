using System.ComponentModel.DataAnnotations;

namespace RuoYi.Data.Dtos
{
    /// <summary>
    ///  通知公告表 对象 sys_notice
    ///  author ruoyi
    ///  date   2023-09-04 17:50:00
    /// </summary>
    public class SysNoticeDto : BaseDto
    {
        /// <summary>
        /// 公告ID
        /// </summary>
        public int? NoticeId { get; set; }
        /// <summary>
        /// 公告标题
        /// </summary>
        [Required(ErrorMessage = "公告标题不能为空"), MaxLength(50, ErrorMessage = "公告标题不能超过50个字符")]
        public string? NoticeTitle { get; set; }
        /// <summary>
        /// 公告类型（1通知 2公告）
        /// </summary>
        public string? NoticeType { get; set; }
        /// <summary>
        /// 公告内容
        /// </summary>
        public string? NoticeContent { get; set; }
        /// <summary>
        /// 公告状态（0正常 1关闭）
        /// </summary>
        public string? Status { get; set; }
    }
}
