//using Lazy.Captcha.Core.Generator.Code;
//using RuoYi.Framework.Extensions;

//namespace RuoYi.Common.Utils
//{
//    public class StringUtils
//    {
//        public static bool IsHttp(string? link)
//        {
//            if (link == null) return false;
//            return link.StartsWith("http") || link.StartsWith("https");
//        }

//        public static bool IsEmpty(string? str)
//        {
//            return string.IsNullOrEmpty(str);
//        }

//        public static bool IsNotEmpty(string? str)
//        {
//            return !string.IsNullOrEmpty(str);
//        }

//        public static bool IsNotBlank(string? str)
//        {
//            return !string.IsNullOrWhiteSpace(str);
//        }
               

//        public static bool Equals(string? str1, string? str2)
//        {
//            return str1.Equals(str2);
//        }

//        public static bool ContainsAny(string? str1, string? str2)
//        {
//            if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2)) return false;
//            return str1.Contains(str2);
//        }

//        public static bool ContainsAny(IEnumerable<string> collection, string str)
//        {
//            if (collection.IsEmpty() || string.IsNullOrEmpty(str))
//            {
//                return false;
//            }

//            return collection.Contains(str);
//        }

//        public static bool ContainsAny(IEnumerable<string> collection1, IEnumerable<string> collection2)
//        {
//            if (collection1.IsEmpty() || collection2.IsEmpty())
//            {
//                return false;
//            }
//            else
//            {
//                foreach (string str in collection2)
//                {
//                    if (collection1.Contains(str))
//                    {
//                        return true;
//                    }
//                }
//                return false;
//            }
//        }

//        public static int Length(string cs)
//        {
//            return cs == null ? 0 : cs.Length;
//        }

//        public static string StripEnd(string str, string stripChars)
//        {
//            int end = Length(str);
//            if (end == 0)
//            {
//                return str;
//            }
//            else
//            {
//                if (stripChars == null)
//                {
//                    while (end != 0 && str[end - 1].Equals(""))
//                    {
//                        --end;
//                    }
//                }
//                else
//                {
//                    if (string.IsNullOrEmpty(stripChars))
//                    {
//                        return str;
//                    }

//                    while (end != 0 && stripChars.IndexOf(str[end - 1]) != -1)
//                    {
//                        --end;
//                    }
//                }

//                return str.Substring(0, end);
//            }
//        }


//        public static string StripStart(string str, string stripChars)
//        {
//            int strLen = Length(str);
//            if (strLen == 0)
//            {
//                return str;
//            }
//            else
//            {
//                int start = 0;
//                if (stripChars == null)
//                {
//                    while (start != strLen && str[start].Equals(""))
//                    {
//                        ++start;
//                    }
//                }
//                else
//                {
//                    if (string.IsNullOrEmpty(stripChars))
//                    {
//                        return str;
//                    }

//                    while (start != strLen && stripChars.IndexOf(str[start]) != -1)
//                    {
//                        ++start;
//                    }
//                }

//                return str.Substring(start);
//            }
//        }

//        public static T Nvl<T>(T value, T defaultValue)
//        {
//            return value != null ? value : defaultValue;
//        }

//        public static string DefaultIfEmpty(string str, string defaultStr)
//        {
//            return IsEmpty(str) ? defaultStr : str;
//        }

//        /// <summary>
//        /// 截取字符串
//        /// </summary>
//        /// <param name="str">字符串</param>
//        /// <param name="start">开始</param>
//        /// <param name="end">结束</param>
//        /// <returns></returns>
//        public static string? Substring(string str, int start, int end)
//        {
//            if (str == null)
//            {
//                return null;
//            }

//            if (end < 0)
//            {
//                end = str.Length + end;
//            }
//            if (start < 0)
//            {
//                start = str.Length + start;
//            }

//            if (end > str.Length)
//            {
//                end = str.Length;
//            }

//            if (start > end)
//            {
//                return null;
//            }

//            if (start < 0)
//            {
//                start = 0;
//            }
//            if (end < 0)
//            {
//                end = 0;
//            }

//            return str.Substring(start, end);
//        }
//    }
//}
