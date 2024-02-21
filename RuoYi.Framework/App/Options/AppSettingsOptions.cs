﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RuoYi.Framework.ConfigurableOptions;

namespace RuoYi.Framework;

/// <summary>
/// 应用全局配置
/// </summary>
public sealed class AppSettingsOptions : IConfigurableOptions<AppSettingsOptions>
{
    /// <summary>
    /// 集成 MiniProfiler 组件
    /// </summary>
    public bool? InjectMiniProfiler { get; set; }

    /// <summary>
    /// 是否启用规范化文档
    /// </summary>
    public bool? InjectSpecificationDocument { get; set; }

    /// <summary>
    /// 是否启用引用程序集扫描
    /// </summary>
    public bool? EnabledReferenceAssemblyScan { get; set; }

    /// <summary>
    /// 外部程序集
    /// </summary>
    /// <remarks>扫描 dll 文件，如果是单文件发布，需拷贝放在根目录下</remarks>
    public string[] ExternalAssemblies { get; set; }

    /// <summary>
    /// 排除扫描的程序集
    /// </summary>
    public string[] ExcludeAssemblies { get; set; }

    /// <summary>
    /// 是否打印数据库连接信息到 MiniProfiler 中
    /// </summary>
    public bool? PrintDbConnectionInfo { get; set; }

    /// <summary>
    /// 是否输出原始 Sql 执行日志（ADO.NET）
    /// </summary>
    public bool? OutputOriginalSqlExecuteLog { get; set; }

    /// <summary>
    /// 配置支持的包前缀名
    /// </summary>
    public string[] SupportPackageNamePrefixs { get; set; }

    /// <summary>
    /// 【部署】二级虚拟目录
    /// </summary>
    public string VirtualPath { get; set; }

    /// <summary>
    /// 后期配置
    /// </summary>
    /// <param name="options"></param>
    /// <param name="configuration"></param>
    public void PostConfigure(AppSettingsOptions options, IConfiguration configuration)
    {
        // 非 Web 环境总是 false，如果是生产环境且不配置 InjectMiniProfiler，默认总是false，MiniProfiler 生产环境耗内存
        if (App.WebHostEnvironment == default
            || (App.HostEnvironment.IsProduction() && options.InjectMiniProfiler == null)) options.InjectMiniProfiler = false;
        else options.InjectMiniProfiler ??= true;

        options.InjectSpecificationDocument ??= true;
        options.EnabledReferenceAssemblyScan ??= false;
        options.ExternalAssemblies ??= Array.Empty<string>();
        options.ExcludeAssemblies ??= Array.Empty<string>();
        options.PrintDbConnectionInfo ??= true;
        options.OutputOriginalSqlExecuteLog ??= true;
        options.SupportPackageNamePrefixs ??= Array.Empty<string>();
        options.VirtualPath ??= string.Empty;
    }
}
