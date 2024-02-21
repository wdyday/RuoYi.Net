﻿namespace RuoYi.Framework.SpecificationDocument;

/// <summary>
/// 配置规范化文档 OperationId 问题
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class OperationIdAttribute : Attribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="operationId">自定义 OperationId，可用户生成可读的前端代码</param>
    public OperationIdAttribute(string operationId)
    {
        OperationId = operationId;
    }

    /// <summary>
    /// 自定义 OperationId
    /// </summary>
    public string OperationId { get; set; }
}