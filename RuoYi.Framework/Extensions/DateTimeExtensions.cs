namespace RuoYi.Framework.Extensions;

public static class DateTimeExtensions
{
    /// <summary>
    /// convert datetime to yyyyMM
    /// </summary>
    public static string ToYm(this DateTime datetime)
    {
        return datetime.To_Ym("");
    }

    /// <summary>
    /// convert datetime to yyyy-MM
    /// </summary>
    public static string To_Ym(this DateTime datetime, string separator = "-")
    {
        return datetime.ToString($"yyyy{separator}MM");
    }

    /// <summary>
    /// convert datetime to yyyyMMdd
    /// </summary>
    public static string ToYmd(this DateTime datetime)
    {
        return datetime.To_Ymd("");
    }

    /// <summary>
    /// convert datetime to yyyy-MM-dd
    /// </summary>
    public static string To_Ymd(this DateTime datetime, string separator = "-")
    {
        return datetime.ToString($"yyyy{separator}MM{separator}dd");
    }

    /// <summary>
    /// convert datetime to yyyyMMddHHmmss
    /// </summary>
    public static string ToYmdHms(this DateTime datetime)
    {
        return datetime.ToString($"yyyyMMddHHmmss");
    }

    /// <summary>
    /// convert datetime to yyyy-MM-dd HH:mm:ss
    /// </summary>
    public static string To_YmdHms(this DateTime datetime, string separator1 = "-", string separator2 = ":")
    {
        return datetime.ToString($"yyyy{separator1}MM{separator1}dd HH{separator2}mm{separator2}ss");
    }

    /// <summary>
    /// 转换成 13 位时间戳
    /// </summary>
    public static long ToUnixTimeMilliseconds(this DateTime datetime)
    {
        return new DateTimeOffset(datetime).ToUnixTimeMilliseconds();
    }

    /// <summary>
    /// 转换成 11 位时间戳
    /// </summary>
    public static long ToUnixTimeSeconds(this DateTime datetime)
    {
        return new DateTimeOffset(datetime).ToUnixTimeSeconds();
    }

    /// <summary>
    /// 13位时间戳 转换成 DateTime
    /// </summary>
    public static DateTime FromUnixTimeMilliseconds(this long timestamp)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;
    }

    /// <summary>
    /// 11位时间戳 转换成 DateTime
    /// </summary>
    public static DateTime FromUnixTimeSeconds(this long timestamp)
    {
        return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
    }

    /// <summary>
    /// 将时间戳转换为 DateTime
    /// </summary>
    /// <param name="timestamp"></param>
    /// <returns></returns>
    internal static DateTime ConvertToDateTime(this long timestamp)
    {
        var timeStampDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var digitCount = (int)Math.Floor(Math.Log10(timestamp) + 1);

        if (digitCount != 13 && digitCount != 10)
        {
            throw new ArgumentException("Data is not a valid timestamp format.");
        }

        return (digitCount == 13
            ? timeStampDateTime.AddMilliseconds(timestamp)  // 13 位时间戳
            : timeStampDateTime.AddSeconds(timestamp)).ToLocalTime();   // 10 位时间戳
    }

    #region DateTime?

    /// <summary>
    /// convert datetime to yyyyMM
    /// </summary>
    public static string ToYm(this DateTime? datetime)
    {
        return datetime.HasValue ? datetime.Value.ToYm() : string.Empty;
    }

    /// <summary>
    /// convert datetime to yyyyMMdd
    /// </summary>
    public static string To_Ym(this DateTime? datetime, string separator = "-")
    {
        return datetime.HasValue ? datetime.Value.To_Ym(separator) : string.Empty;
    }

    /// <summary>
    /// convert datetime to yyyyMMdd
    /// </summary>
    public static string ToYmd(this DateTime? datetime)
    {
        return datetime.To_Ymd("");
    }

    /// <summary>
    /// convert datetime to yyyy-MM-dd
    /// </summary>
    public static string To_Ymd(this DateTime? datetime, string separator = "-")
    {
        return datetime.HasValue ? datetime.Value.To_Ymd(separator) : string.Empty;
    }

    /// <summary>
    /// convert datetime to yyyyMMddHHmmss
    /// </summary>
    public static string ToYmdHms(this DateTime? datetime)
    {
        return datetime.HasValue ? datetime.Value.ToYmdHms() : string.Empty;
    }

    /// <summary>
    /// convert datetime to yyyy-MM-dd HH:mm:ss
    /// </summary>
    public static string To_YmdHms(this DateTime? datetime, string separator1 = "-", string separator2 = ":")
    {
        return datetime.HasValue ? datetime.Value.To_YmdHms(separator1, separator2) : string.Empty;
    }
    #endregion

    #region DateTimeOffset
    /// <summary>
    /// 将 DateTimeOffset 转换成本地 DateTime
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static DateTime ConvertToDateTime(this DateTimeOffset dateTime)
    {
        if (dateTime.Offset.Equals(TimeSpan.Zero))
            return dateTime.UtcDateTime;
        if (dateTime.Offset.Equals(TimeZoneInfo.Local.GetUtcOffset(dateTime.DateTime)))
            return dateTime.ToLocalTime().DateTime;
        else
            return dateTime.DateTime;
    }

    /// <summary>
    /// 将 DateTimeOffset? 转换成本地 DateTime?
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static DateTime? ConvertToDateTime(this DateTimeOffset? dateTime)
    {
        return dateTime.HasValue ? dateTime.Value.ConvertToDateTime() : null;
    }

    /// <summary>
    /// 将 DateTime 转换成 DateTimeOffset
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static DateTimeOffset ConvertToDateTimeOffset(this DateTime dateTime)
    {
        return DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
    }

    /// <summary>
    /// 将 DateTime? 转换成 DateTimeOffset?
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static DateTimeOffset? ConvertToDateTimeOffset(this DateTime? dateTime)
    {
        return dateTime.HasValue ? dateTime.Value.ConvertToDateTimeOffset() : null;
    }

    #endregion
}
