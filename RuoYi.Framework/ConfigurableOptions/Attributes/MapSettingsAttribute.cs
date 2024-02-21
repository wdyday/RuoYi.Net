namespace RuoYi.Framework.ConfigurableOptions;

/// <summary>
/// 重新映射属性配置
/// </summary>
[SuppressSniffer, AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class MapSettingsAttribute : Attribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="path">appsetting.json 对应键</param>
    public MapSettingsAttribute(string path)
    {
        Path = path;
    }

    /// <summary>
    /// 对应配置文件中的路径
    /// </summary>
    public string Path { get; set; }
}