﻿using System.Reflection;

namespace RuoYi.Framework;

/// <summary>
/// 解决单文件发布程序集扫描问题
/// </summary>
public interface ISingleFilePublish
{
    /// <summary>
    /// 包含程序集数组
    /// </summary>
    /// <remarks>配置单文件发布扫描程序集</remarks>
    /// <returns></returns>
    Assembly[] IncludeAssemblies();

    /// <summary>
    /// 包含程序集名称数组
    /// </summary>
    /// <remarks>配置单文件发布扫描程序集名称</remarks>
    /// <returns></returns>
    string[] IncludeAssemblyNames();
}