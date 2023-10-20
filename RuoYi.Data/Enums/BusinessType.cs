using System.ComponentModel;

namespace RuoYi.Data.Enums
{
    /// <summary>
    /// 业务类型（0其它 1新增 2修改 3删除）
    /// </summary>
    public enum BusinessType
    {
        [Description("通知")]
        Ohter = 0,
        [Description("新增")]
        Add = 1,
        [Description("修改")]
        Edit = 2,
        [Description("删除")]
        Del = 3
    }
}
