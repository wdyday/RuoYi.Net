using System.ComponentModel;

namespace RuoYi.Data.Enums
{
    // 状态（0正常 1停用）
    // 是否为外链（0是 1否）
    // 是否缓存（0缓存 1不缓存）
    // 显示状态（0显示 1隐藏）
    public enum Status
    {
        [Description("正常")]
        Enabeled = 0,
        [Description("停用")]
        Disabled = 1
    }
}
