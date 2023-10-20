using SkiaSharp;
using System.ComponentModel;

namespace RuoYi.Framework.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDesc(this Enum thisEnum)
        {
            try
            {
                var type = thisEnum.GetType();

                var memberInfo = type.GetMember(thisEnum.ToString());

                var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes == null || attributes.Length != 1)
                {
                    //如果没有定义描述，就把当前枚举值的对应名称返回
                    return thisEnum.ToString();
                }

                return (attributes.Single() as DescriptionAttribute)!.Description;
            }
            catch
            {
                return "";
            }
        }

        public static int ToInt(this Enum thisEnum)
        {
            var val = Enum.Parse(thisEnum.GetType(), thisEnum.ToString());
            return val != null ? Convert.ToInt32(val) : default;
        }

        public static short ToShort(this Enum thisEnum)
        {
            var val = Enum.Parse(thisEnum.GetType(), thisEnum.ToString());
            return val != null ? Convert.ToInt16(val) : default;
        }

        public static byte ToByte(this Enum thisEnum)
        {
            var val = Enum.Parse(thisEnum.GetType(), thisEnum.ToString());
            return val != null ? Convert.ToByte(val) : default;
        }

        public static string GetValue(this Enum thisEnum)
        {
            var val = Enum.Parse(thisEnum.GetType(), thisEnum.ToString());
            return Convert.ToInt32(val).ToString();
        }
    }
}
