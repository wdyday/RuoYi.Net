﻿#if !NET5_0
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RuoYi.Framework.JsonSerialization;

/// <summary>
/// TimeOnly 类型序列化
/// </summary>
[SuppressSniffer]
public class NewtonsoftJsonTimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public NewtonsoftJsonTimeOnlyJsonConverter()
        : this(default)
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="format"></param>
    public NewtonsoftJsonTimeOnlyJsonConverter(string format = "HH:mm:ss")
    {
        Format = format;
    }

    /// <summary>
    /// 时间格式化格式
    /// </summary>
    public string Format { get; private set; }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="objectType"></param>
    /// <param name="existingValue"></param>
    /// <param name="hasExistingValue"></param>
    /// <param name="serializer"></param>
    /// <returns></returns>
    public override TimeOnly ReadJson(JsonReader reader, Type objectType, TimeOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var value = JValue.ReadFrom(reader).Value<string>();
        return TimeOnly.Parse(value);
    }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="serializer"></param>
    public override void WriteJson(JsonWriter writer, TimeOnly value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value.ToString(Format));
    }
}

/// <summary>
/// TimeOnly? 类型序列化
/// </summary>
[SuppressSniffer]
public class NewtonsoftJsonNullableTimeOnlyJsonConverter : JsonConverter<TimeOnly?>
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public NewtonsoftJsonNullableTimeOnlyJsonConverter()
        : this(default)
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="format"></param>
    public NewtonsoftJsonNullableTimeOnlyJsonConverter(string format = "HH:mm:ss")
    {
        Format = format;
    }

    /// <summary>
    /// 时间格式化格式
    /// </summary>
    public string Format { get; private set; }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="objectType"></param>
    /// <param name="existingValue"></param>
    /// <param name="hasExistingValue"></param>
    /// <param name="serializer"></param>
    /// <returns></returns>
    public override TimeOnly? ReadJson(JsonReader reader, Type objectType, TimeOnly? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var value = JValue.ReadFrom(reader).Value<string>();
        return !string.IsNullOrWhiteSpace(value) ? TimeOnly.Parse(value) : null;
    }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="serializer"></param>
    public override void WriteJson(JsonWriter writer, TimeOnly? value, JsonSerializer serializer)
    {
        if (value == null) writer.WriteNull();
        else serializer.Serialize(writer, value.Value.ToString(Format));
    }
}
#endif