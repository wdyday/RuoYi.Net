using System.ComponentModel;

namespace RuoYi.Common.Enums
{
    public enum UserStatus
    {
        [Description("正常")]
        OK,

        [Description("停用")]
        DISABLE,

        [Description("删除")]
        DELETED
    }
}
