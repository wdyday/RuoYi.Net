using Lazy.Captcha.Core;
using RuoYi.Framework;

namespace Microsoft.Extensions.DependencyInjection;

public static class CaptchaExtensions
{
    public static IServiceCollection AddLazyCaptcha(this IServiceCollection services)
    {
        // CaptchaOptions 的配置参考: https://github.com/pojianbing/LazyCaptcha
        var options = App.GetConfig<CaptchaOptions>(nameof(CaptchaOptions), true);

        return services.AddCaptcha(option =>
        {
            option.CaptchaType = options.CaptchaType; // 验证码类型
            option.CodeLength = options.CodeLength; // 验证码长度, 要放在CaptchaType设置后.  当类型为算术表达式时，长度代表操作的个数
            option.ExpirySeconds = options.ExpirySeconds; // 验证码过期时间
            option.IgnoreCase = options.IgnoreCase; // 比较时是否忽略大小写
            option.StoreageKeyPrefix = options.StoreageKeyPrefix; // 存储键前缀

            option.ImageOption.Animation = options.ImageOption.Animation; // 是否启用动画
            option.ImageOption.FrameDelay = options.ImageOption.FrameDelay; // 每帧延迟,Animation=true时有效, 默认30

            option.ImageOption.Width = options.ImageOption.Width; // 验证码宽度
            option.ImageOption.Height = options.ImageOption.Height; // 验证码高度
            option.ImageOption.BackgroundColor = options.ImageOption.BackgroundColor; // 验证码背景色

            option.ImageOption.BubbleCount = options.ImageOption.BubbleCount; // 气泡数量
            option.ImageOption.BubbleMinRadius = options.ImageOption.BubbleMinRadius; // 气泡最小半径
            option.ImageOption.BubbleMaxRadius = options.ImageOption.BubbleMaxRadius; // 气泡最大半径
            option.ImageOption.BubbleThickness = options.ImageOption.BubbleThickness; // 气泡边沿厚度

            option.ImageOption.InterferenceLineCount = options.ImageOption.InterferenceLineCount; // 干扰线数量

            option.ImageOption.FontSize = options.ImageOption.FontSize; // 字体大小
            option.ImageOption.FontFamily = options.ImageOption.FontFamily; // 字体

            /* 
             * 中文使用kaiti，其他字符可根据喜好设置（可能部分转字符会出现绘制不出的情况）。
             * 当验证码类型为“ARITHMETIC”时，不要使用“Ransom”字体。（运算符和等号绘制不出来）
             */

            option.ImageOption.TextBold = options.ImageOption.TextBold;// 粗体，该配置2.0.3新增
        });
    }
}