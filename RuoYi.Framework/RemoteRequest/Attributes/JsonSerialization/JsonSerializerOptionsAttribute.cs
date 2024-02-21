namespace RuoYi.Framework.RemoteRequest;

/// <summary>
/// 配置序列化选项
/// </summary>
[SuppressSniffer, AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
public class JsonSerializerOptionsAttribute : Attribute
{
}