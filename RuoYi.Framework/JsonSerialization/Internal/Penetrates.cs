using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RuoYi.Framework.Extensions;
using System.Text.Json;

namespace RuoYi.Framework.JsonSerialization;

/// <summary>
/// 常量、公共方法配置类
/// </summary>
internal static class Penetrates
{
    /// <summary>
    /// 转换
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    internal static DateTime ConvertToDateTime(ref Utf8JsonReader reader)
    {
        // 处理时间戳自动转换
        if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt64(out var longValue))
        {
            return longValue.ConvertToDateTime();
        };

        var stringValue = reader.GetString();

        // 处理时间戳自动转换
        if (long.TryParse(stringValue, out var longValue2))
        {
            return longValue2.ConvertToDateTime();
        }

        return Convert.ToDateTime(stringValue);
    }

    /// <summary>
    /// 转换
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    internal static DateTime ConvertToDateTime(ref JsonReader reader)
    {
        if (reader.TokenType == JsonToken.Integer)
        {
            return JValue.ReadFrom(reader).Value<long>().ConvertToDateTime();
        }

        var stringValue = JValue.ReadFrom(reader).Value<string>();

        // 处理时间戳自动转换
        if (long.TryParse(stringValue, out var longValue2))
        {
            return longValue2.ConvertToDateTime();
        }

        return Convert.ToDateTime(stringValue);
    }
}