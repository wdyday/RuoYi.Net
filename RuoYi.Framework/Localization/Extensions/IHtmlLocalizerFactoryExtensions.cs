using RuoYi.Framework;
using RuoYi.Framework.Localization;

namespace Microsoft.AspNetCore.Mvc.Localization;

/// <summary>
/// IHtmlLocalizerFactory 拓展类
/// </summary>
[SuppressSniffer]
public static class IHtmlLocalizerFactoryExtensions
{
    /// <summary>
    /// 创建默认多语言工厂
    /// </summary>
    /// <param name="htmlLocalizerFactory"></param>
    /// <returns></returns>
    public static IHtmlLocalizer Create(this IHtmlLocalizerFactory htmlLocalizerFactory)
    {
        var localizationSettings = App.GetOptions<LocalizationSettingsOptions>();
        return htmlLocalizerFactory.Create(localizationSettings.LanguageFilePrefix, localizationSettings.AssemblyName);
    }
}