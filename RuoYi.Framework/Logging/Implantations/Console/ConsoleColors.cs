namespace RuoYi.Framework.Logging;

/// <summary>
/// 控制台颜色结构
/// </summary>
internal readonly struct ConsoleColors
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="foreground"></param>
    /// <param name="background"></param>
    public ConsoleColors(ConsoleColor? foreground, ConsoleColor? background)
    {
        Foreground = foreground;
        Background = background;
    }

    /// <summary>
    /// 前景色
    /// </summary>
    public ConsoleColor? Foreground { get; }

    /// <summary>
    /// 背景色
    /// </summary>
    public ConsoleColor? Background { get; }
}