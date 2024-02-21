namespace RuoYi.Framework.Configuration;

/// <summary>
/// Configuration 模块常量
/// </summary>
internal static class Constants
{
    /// <summary>
    /// 正则表达式常量
    /// </summary>
    internal static class Patterns
    {
        /// <summary>
        /// 配置文件名
        /// </summary>
        internal const string ConfigurationFileName = @"(?<fileName>(?<realName>.+?)(\.(?<environmentName>\w+))?(?<extension>\.(json|xml|ini)))";

        /// <summary>
        /// 配置文件参数
        /// </summary>
        internal const string ConfigurationFileParameter = @"\s+(?<parameter>\b\w+\b)\s*=\s*(?<value>\btrue\b|\bfalse\b)";
    }
}