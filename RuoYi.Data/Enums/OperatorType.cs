using System.ComponentModel;

namespace RuoYi.Data.Enums
{
    /// <summary>
    /// 操作类别（0其它 1后台用户 2手机端用户）
    /// </summary>
    public enum OperatorType
    {
        [Description("其它")]
        Ohter = 0,
        [Description("后台用户")]
        Web = 1,
        [Description("手机端用户")]
        Mobile = 2
    }
}
