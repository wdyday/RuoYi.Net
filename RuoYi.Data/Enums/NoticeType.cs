using System.ComponentModel;

namespace RuoYi.Data.Enums
{
    /// <summary>
    /// 公告类型（1通知 2公告）
    /// </summary>
    public enum NoticeType
    {
        [Description("通知")]
        Notice = 1,
        [Description("公告")]
        Announcement = 2
    }
}
