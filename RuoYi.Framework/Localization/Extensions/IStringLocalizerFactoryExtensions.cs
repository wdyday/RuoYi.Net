using RuoYi.Framework;
using RuoYi.Framework.Localization;

namespace Microsoft.Extensions.Localization;

/// <summary>
/// IStringLocalizerFactory 拓展类
/// </summary>
[SuppressSniffer]
public static class IStringLocalizerFactoryExtensions
{
    /// <summary>
    /// 创建默认多语言工厂
    /// </summary>
    /// <param name="stringLocalizerFactory"></param>
    /// <returns></returns>
    public static IStringLocalizer Create(this IStringLocalizerFactory stringLocalizerFactory)
    {
        var localizationSettings = App.GetOptions<LocalizationSettingsOptions>();

        // 处理空配置问题
        if (!string.IsNullOrWhiteSpace(localizationSettings.LanguageFilePrefix)
            && !string.IsNullOrWhiteSpace(localizationSettings.AssemblyName))
        {
            return stringLocalizerFactory.Create(localizationSettings.LanguageFilePrefix, localizationSettings.AssemblyName);
        }

        return null;
    }
}