﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RuoYi.Framework.Localization;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// 多语言中间件拓展
/// </summary>
[SuppressSniffer]
public static class LocalizationApplicationBuilderExtensions
{
    /// <summary>
    /// 配置多语言中间件拓展
    /// </summary>
    /// <param name="app"></param>
    /// <param name="customizeConfigure"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseAppLocalization(this IApplicationBuilder app, Action<RequestLocalizationOptions> customizeConfigure = default)
    {
        // 获取多语言配置选项
        var localizationSettings = app.ApplicationServices.GetRequiredService<IOptions<LocalizationSettingsOptions>>().Value;

        var requestLocalization = new RequestLocalizationOptions();
        Penetrates.SetRequestLocalization(requestLocalization, localizationSettings);

        // 使用自定义配置
        customizeConfigure?.Invoke(requestLocalization);

        // 设置多语言请求中间件
        app.UseRequestLocalization(requestLocalization);

        return app;
    }
}