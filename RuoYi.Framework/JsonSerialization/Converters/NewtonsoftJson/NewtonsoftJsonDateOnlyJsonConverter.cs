﻿#if !NET5_0
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RuoYi.Framework.JsonSerialization;

/// <summary>
/// DateOnly 类型序列化
/// </summary>
[SuppressSniffer]
public class NewtonsoftJsonDateOnlyJsonConverter : JsonConverter<DateOnly>
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public NewtonsoftJsonDateOnlyJsonConverter()
        : this(default)
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="format"></param>
    public NewtonsoftJsonDateOnlyJsonConverter(string format = "yyyy-MM-dd")
    {
        Format = format;
    }

    /// <summary>
    /// 日期格式化格式
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
    public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var value = JValue.ReadFrom(reader).Value<string>();
        return DateOnly.Parse(value);
    }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="serializer"></param>
    public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value.ToString(Format));
    }
}

/// <summary>
/// DateOnly? 类型序列化
/// </summary>
[SuppressSniffer]
public class NewtonsoftJsonNullableDateOnlyJsonConverter : JsonConverter<DateOnly?>
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public NewtonsoftJsonNullableDateOnlyJsonConverter()
        : this(default)
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="format"></param>
    public NewtonsoftJsonNullableDateOnlyJsonConverter(string format = "yyyy-MM-dd")
    {
        Format = format;
    }

    /// <summary>
    /// 日期格式化格式
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
    public override DateOnly? ReadJson(JsonReader reader, Type objectType, DateOnly? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var value = JValue.ReadFrom(reader).Value<string>();
        return !string.IsNullOrWhiteSpace(value) ? DateOnly.Parse(value) : null;
    }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="serializer"></param>
    public override void WriteJson(JsonWriter writer, DateOnly? value, JsonSerializer serializer)
    {
        if (value == null) writer.WriteNull();
        else serializer.Serialize(writer, value.Value.ToString(Format));
    }
}
#endif