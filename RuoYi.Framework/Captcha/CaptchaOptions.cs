using Furion.ConfigurableOptions;
using Lazy.Captcha.Core;
using Lazy.Captcha.Core.Generator;
using Microsoft.Extensions.Configuration;
using SkiaSharp;

namespace RuoYi.Framework.Captcha
{
    [OptionsSettings("CaptchaOptions")]
    public class LazyCaptchaOptions : CaptchaOptions, IConfigurableOptions<LazyCaptchaOptions>
    {
        // 默认值
        public void PostConfigure(LazyCaptchaOptions options, IConfiguration configuration)
        {
            options.CaptchaType = CaptchaType.ARITHMETIC; // 验证码类型, 默认 算术表达式
            options.CodeLength = 1; // 验证码长度, 要放在CaptchaType设置后.  当类型为算术表达式时，长度代表操作的个数
            options.ExpirySeconds = 120; // 验证码过期时间
            options.IgnoreCase = true; // 比较时是否忽略大小写
            options.StoreageKeyPrefix = "captcha_codes:"; // 存储键前缀

            options.ImageOption.Animation = false; // 是否启用动画
            options.ImageOption.FrameDelay = 30; // 每帧延迟,Animation=true时有效, 默认30

            options.ImageOption.Width = 150; // 验证码宽度
            options.ImageOption.Height = 50; // 验证码高度
            options.ImageOption.BackgroundColor = SKColors.White; // 验证码背景色

            options.ImageOption.BubbleCount = 2; // 气泡数量
            options.ImageOption.BubbleMinRadius = 5; // 气泡最小半径
            options.ImageOption.BubbleMaxRadius = 15; // 气泡最大半径
            options.ImageOption.BubbleThickness = 1; // 气泡边沿厚度

            options.ImageOption.InterferenceLineCount = 2; // 干扰线数量

            options.ImageOption.FontSize = 36; // 字体大小
            options.ImageOption.FontFamily = DefaultFontFamilys.Instance.Kaiti; // 字体

            /* 
             * 中文使用kaiti，其他字符可根据喜好设置（可能部分转字符会出现绘制不出的情况）。
             * 当验证码类型为“ARITHMETIC”时，不要使用“Ransom”字体。（运算符和等号绘制不出来）
             */
            options.ImageOption.TextBold = true;// 粗体，该配置2.0.3新增
        }


        // 热更新(实时的最新值)
        public void OnListener(LazyCaptchaOptions options, IConfiguration configuration)
        {
            this.CaptchaType = options.CaptchaType;
            this.CodeLength = options.CodeLength; // 验证码长度, 要放在CaptchaType设置后.  当类型为算术表达式时，长度代表操作的个数
            this.ExpirySeconds = options.ExpirySeconds; // 验证码过期时间
            this.ImageOption.Width = options.ImageOption.Width; // 验证码宽度
            this.ImageOption.Height = options.ImageOption.Height; // 验证码高度
            this.ImageOption.BackgroundColor = options.ImageOption.BackgroundColor; // 验证码背景色
            this.ImageOption.FontSize = options.ImageOption.FontSize = 36; // 字体大小
            this.ImageOption.FontFamily = options.ImageOption.FontFamily; // 字体
            this.ImageOption.TextBold = options.ImageOption.TextBold;// 粗体，该配置2.0.3新增
        }
    }
}
