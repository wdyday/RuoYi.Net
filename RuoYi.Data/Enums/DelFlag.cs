using System.ComponentModel;

namespace RuoYi.Data.Enums
{
    /// <summary>
    /// 删除标志（0代表存在 2代表删除）
    /// </summary>
    public enum DelFlag
    {
        [Description("未删除")]
        No = 0,
        [Description("已删除")]
        Yes = 2
    }
}
