using RuoYi.Framework.Attributes;
using RuoYi.Quartz.Services;

namespace RuoYi.Quartz.Tasks
{
    [Task("ryTask")]
    public class RyTask
    {
        public void RyMultipleParams(string s, bool b, long l, double d, int i)
        {
            RuoYi.Framework.Logging.Log.Information($"执行多参方法： 字符串类型{s}，布尔类型{b}，长整型{l}，浮点型{d}，整形{i}");
        }

        public void RyParams(string param)
        {
            RuoYi.Framework.Logging.Log.Information($"执行有参方法：{param}");
        }

        public void RyNoParams()
        {
            RuoYi.Framework.Logging.Log.Information("执行无参方法");
        }
    }
}
