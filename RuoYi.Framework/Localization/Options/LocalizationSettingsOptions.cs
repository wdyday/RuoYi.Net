﻿using Microsoft.Extensions.Configuration;
using RuoYi.Framework.ConfigurableOptions;
using RuoYi.Framework.Reflection;

namespace RuoYi.Framework.Localization;

/// <summary>
/// 多语言配置选项
/// </summary>
public sealed class LocalizationSettingsOptions : IConfigurableOptions<LocalizationSettingsOptions>
{
    /// <summary>
    /// 资源路径
    /// </summary>
    public string ResourcesPath { get; set; }

    /// <summary>
    /// 支持的语言列表
    /// </summary>
    public string[] SupportedCultures { get; set; }

    /// <summary>
    /// 默认的语言
    /// </summary>
    public string DefaultCulture { get; set; }

    /// <summary>
    /// 资源文件名前缀
    /// </summary>
    public string LanguageFilePrefix { get; set; }

    /// <summary>
    /// 资源所在程序集名称
    /// </summary>
    public string AssemblyName { get; set; }

    /// <summary>
    /// 设置 DateTime 格式化标准语言
    /// </summary>
    /// <remarks>不设置则自动根据当前语言</remarks>
    public string DateTimeFormatCulture { get; set; }

    /// <summary>
    /// 选项后期配置
    /// </summary>
    /// <param name="options"></param>
    /// <param name="configuration"></param>
    public void PostConfigure(LocalizationSettingsOptions options, IConfiguration configuration)
    {
        ResourcesPath ??= "Resources";
        SupportedCultures ??= Array.Empty<string>();
        DefaultCulture ??= string.Empty;
        LanguageFilePrefix ??= "Lang";
        AssemblyName ??= Reflect.GetAssemblyName(Reflect.GetEntryAssembly());
        DateTimeFormatCulture ??= string.Empty;
    }
}