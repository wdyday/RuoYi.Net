﻿#if !NET5_0
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RuoYi.Framework.JsonSerialization;

/// <summary>
/// DateOnly 类型序列化
/// </summary>
[SuppressSniffer]
public class SystemTextJsonDateOnlyJsonConverter : JsonConverter<DateOnly>
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public SystemTextJsonDateOnlyJsonConverter()
        : this(default)
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="format"></param>
    public SystemTextJsonDateOnlyJsonConverter(string format = "yyyy-MM-dd")
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
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateOnly.Parse(reader.GetString());
    }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format));
    }
}

/// <summary>
/// DateOnly? 类型序列化
/// </summary>
[SuppressSniffer]
public class SystemTextJsonNullableDateOnlyJsonConverter : JsonConverter<DateOnly?>
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public SystemTextJsonNullableDateOnlyJsonConverter()
        : this(default)
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="format"></param>
    public SystemTextJsonNullableDateOnlyJsonConverter(string format = "yyyy-MM-dd")
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
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateOnly.TryParse(reader.GetString(), out var date) ? date : null;
    }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
    {
        if (value == null) writer.WriteNullValue();
        else writer.WriteStringValue(value.Value.ToString(Format));
    }
}
#endif