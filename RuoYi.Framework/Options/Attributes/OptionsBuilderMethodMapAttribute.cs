namespace RuoYi.Framework.Options;

/// <summary>
/// 选项构建器方法映射特性
/// </summary>
[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
internal sealed class OptionsBuilderMethodMapAttribute : Attribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="methodName">映射方法名</param>
    /// <param name="voidReturn">无返回值</param>
    internal OptionsBuilderMethodMapAttribute(string methodName, bool voidReturn)
    {
        MethodName = methodName;
        VoidReturn = voidReturn;
    }

    /// <summary>
    /// 方法名称
    /// </summary>
    internal string MethodName { get; set; }

    /// <summary>
    /// 有无返回值
    /// </summary>
    internal bool VoidReturn { get; set; }
}