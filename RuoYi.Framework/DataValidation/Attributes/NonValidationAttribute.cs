namespace Microsoft.AspNetCore.Mvc;

/// <summary>
/// 跳过验证
/// </summary>
[SuppressSniffer, AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public sealed class NonValidationAttribute : Attribute
{
}