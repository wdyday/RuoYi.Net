namespace RuoYi.Framework.DataValidation;

/// <summary>
/// 验证消息类型特性
/// </summary>
[SuppressSniffer, AttributeUsage(AttributeTargets.Enum)]
public sealed class ValidationMessageTypeAttribute : Attribute
{
}