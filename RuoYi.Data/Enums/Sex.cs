using System.ComponentModel;

namespace RuoYi.Data.Enums
{
    /// <summary>
    /// 用户性别（0男 1女 2未知）
    /// </summary>
    public enum Sex
    {
        [Description("男")]
        Male = 0,
        [Description("女")]
        Female = 1,
        [Description("未知")]
        Unknown = 2
    }
}
