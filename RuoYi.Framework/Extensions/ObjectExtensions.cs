using SqlSugar;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace RuoYi.Framework.Extensions
{
    public static class ObjectExtensions
    {
        /** 空字符串 */
        //private static string NULLSTR = "";

        /** 下划线 */
        private static char SEPARATOR = '_';

        /// <summary>
        /// 首字母小写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string? ToLowerCamelCase(this string? str)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;

            return string.Concat(str.First().ToString().ToLower(), str.AsSpan(1));
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string? ToUpperCamelCase(this string? str)
        {
            if (string.IsNullOrWhiteSpace(str)) return str;

            return string.Concat(str.First().ToString().ToUpper(), str.AsSpan(1));
        }

        /// <summary>
        /// 判断集合是否为空
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="collection">集合对象</param>
        /// <returns>实例，true 表示空集合，false 表示非空集合</returns>
        public static bool IsEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }

        /// <summary>
        /// 判断集合是否不为空
        /// </summary>
        public static bool IsNotEmpty<T>(this IEnumerable<T> collection)
        {
            return !IsEmpty(collection);
        }

        /// <summary>
        /// 替换第一个符合条件的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="oldValue">所要替换掉的值</param>
        /// <param name="newValue">所要替换的值</param>
        /// <returns>返回替换后的值 所要替换掉的值为空或Null，返回原值</returns>
        public static string ReplaceFirst(this string value, string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(oldValue))
                return value;

            int idx = value.IndexOf(oldValue);
            if (idx == -1)
                return value;
            value = value.Remove(idx, oldValue.Length);
            return value.Insert(idx, newValue);
        }

        /// <summary>
        /// 驼峰式命名法, 例如：user_name->UserName
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string? ToUpperCamelCase2(this string str)
        {
            if (str == null)
            {
                return str;
            }
            str = str.ToLower();

            var strs = str.Split(SEPARATOR);

            var sb = new StringBuilder(str.Length);

            foreach (string s in strs)
            {
                sb.Append(s.ToUpperCamelCase());
            }
            return sb.ToString();
        }

        /// <summary>
        /// 驼峰转下划线命名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToUnderScoreCase(this string str)
        {
            if (str == null)
            {
                return null;
            }
            StringBuilder sb = new StringBuilder();
            // 前置字符是否大写
            bool preCharIsUpperCase = true;
            // 当前字符是否大写
            bool curreCharIsUpperCase = true;
            // 下一字符是否大写
            bool nexteCharIsUpperCase = true;

            var chars = str.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                char c = chars[i];
                if (i > 0)
                {
                    preCharIsUpperCase = char.IsUpper(chars[i - 1]);
                }
                else
                {
                    preCharIsUpperCase = false;
                }

                curreCharIsUpperCase = char.IsUpper(c);

                if (i < (str.Length - 1))
                {
                    nexteCharIsUpperCase = char.IsUpper(chars[i + 1]);
                }

                if (preCharIsUpperCase && curreCharIsUpperCase && !nexteCharIsUpperCase)
                {
                    sb.Append(SEPARATOR);
                }
                else if ((i != 0 && !preCharIsUpperCase) && curreCharIsUpperCase)
                {
                    sb.Append(SEPARATOR);
                }

                sb.Append(char.ToLower(c));
            }

            return sb.ToString();
        }

        public static string? SubstringBetween(this string? str, string open, string close)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(open) || string.IsNullOrEmpty(close))
            {
                return null;
            }

            int start = str.IndexOf(open);
            if (start != -1)
            {
                int end = str.IndexOf(close, start + open.Length);
                if (end != -1)
                {
                    return str.Substring(start + open.Length, end - (start + open.Length));
                }
            }
            return null;
        }

        public static string? SubstringBefore(this string? str, string separator)
        {
            if (string.IsNullOrEmpty(str) || separator == null)
            {
                return str;
            }

            if (separator == "")
            {
                return "";
            }

            int pos = str.IndexOf(separator);
            if (pos == -1)
            {
                return str;
            }
            return str.Substring(0, pos);
        }

        /// <summary>
        /// 忽略大小写 判断是否相同
        /// </summary>
        public static bool EqualsIgnoreCase(this string str, string compareStr)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(compareStr)) return false;
            return str.Equals(compareStr, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsHttp(this string str)
        {
            if (str == null) return false;

            return str.StartsWith("http", StringComparison.OrdinalIgnoreCase);
        }

        // 带分隔符的字符串转缓存 List
        public static IEnumerable<T> SplitTo<T>(this string str, string separator = ",")
        {
            if (str == null) return new List<T>();
            return str.Split(separator, StringSplitOptions.RemoveEmptyEntries).Select(s =>
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                return (T)converter.ConvertFromString(s)!;
            });
        }
        public static List<T> SplitToList<T>(this string str, string separator = ",")
        {
            return SplitTo<T>(str, separator).ToList();
        }
        public static T[] SplitToArray<T>(this string str, string separator = ",")
        {
            return SplitTo<T>(str, separator).ToArray();
        }

        /// <summary>
        /// 查找方法指定特性，如果没找到则继续查找声明类
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="method"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static TAttribute GetFoundAttribute<TAttribute>(this MethodInfo method, bool inherit)
            where TAttribute : Attribute
        {
            // 获取方法所在类型
            var declaringType = method.DeclaringType;

            var attributeType = typeof(TAttribute);

            // 判断方法是否定义指定特性，如果没有再查找声明类
            var foundAttribute = method.IsDefined(attributeType, inherit)
                ? method.GetCustomAttribute<TAttribute>(inherit)
                : (
                    declaringType.IsDefined(attributeType, inherit)
                    ? declaringType.GetCustomAttribute<TAttribute>(inherit)
                    : default
                );

            return foundAttribute;
        }

        /// <summary>
        /// 字典转字符串
        /// </summary>
        /// <param name="dic">字典</param>
        /// <returns>key2:value2;key2:value2</returns>
        public static string ToDictString(this System.Collections.IDictionary dic)
        {
            var results = new List<string>();
            foreach (var key in dic.Keys)
            {
                results.Add($"{key}:{dic[key]}");
            }
            return string.Join(", ", results);
        }
    }
}
