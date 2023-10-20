using SqlSugar;

namespace RuoYi.Data.Entities
{
    /// <summary>
    ///  通知公告表 对象 sys_notice
    ///  author ruoyi
    ///  date   2023-09-04 17:50:00
    /// </summary>
    [SugarTable("sys_notice", "通知公告表")]
    public class SysNotice : UserBaseEntity
    {
        /// <summary>
        /// 公告ID (notice_id)
        /// </summary>
        [SugarColumn(ColumnName = "notice_id", ColumnDescription = "公告ID", IsPrimaryKey = true, IsIdentity = true)]
        public int NoticeId { get; set; }
        /// <summary>
        /// 公告标题 (notice_title)
        /// </summary>
        [SugarColumn(ColumnName = "notice_title", ColumnDescription = "公告标题")]
        public string NoticeTitle { get; set; }
        /// <summary>
        /// 公告类型（1通知 2公告） (notice_type)
        /// </summary>
        [SugarColumn(ColumnName = "notice_type", ColumnDescription = "公告类型（1通知 2公告）")]
        public string NoticeType { get; set; }
        /// <summary>
        /// 公告内容 (notice_content)
        /// </summary>
        [SugarColumn(ColumnName = "notice_content", ColumnDescription = "公告内容")]
        public string? NoticeContent { get; set; }
        /// <summary>
        /// 公告状态（0正常 1关闭） (status)
        /// </summary>
        [SugarColumn(ColumnName = "status", ColumnDescription = "公告状态（0正常 1关闭）")]
        public string? Status { get; set; }
    }
}
