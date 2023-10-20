using RuoYi.Framework.Extensions;

namespace RuoYi.Framework.Utils
{
    public static class StringUtils
    {
        public static bool IsHttp(string? link)
        {
            if (link == null) return false;
            return link.StartsWith("http") || link.StartsWith("https");
        }

        public static bool IsEmpty(string? str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNotEmpty(string? str)
        {
            return !string.IsNullOrEmpty(str);
        }

        public static bool IsNotBlank(string? str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }

        public static bool Equals(string? str1, string? str2)
        {
            return str1.Equals(str2);
        }

        public static bool ContainsAny(string? str1, string? str2)
        {
            if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2)) return false;
            return str1.Contains(str2);
        }

        public static bool ContainsAny(IEnumerable<string> collection, string str)
        {
            if (collection.IsEmpty() || string.IsNullOrEmpty(str))
            {
                return false;
            }

            return collection.Contains(str);
        }

        public static bool ContainsAny(IEnumerable<string> collection1, IEnumerable<string> collection2)
        {
            if (collection1.IsEmpty() || collection2.IsEmpty())
            {
                return false;
            }
            else
            {
                foreach (string str in collection2)
                {
                    if (collection1.Contains(str))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public static int Length(string cs)
        {
            return cs == null ? 0 : cs.Length;
        }

        public static string StripEnd(string str, string stripChars)
        {
            int end = Length(str);
            if (end == 0)
            {
                return str;
            }
            else
            {
                if (stripChars == null)
                {
                    while (end != 0 && str[end - 1].Equals(""))
                    {
                        --end;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(stripChars))
                    {
                        return str;
                    }

                    while (end != 0 && stripChars.IndexOf(str[end - 1]) != -1)
                    {
                        --end;
                    }
                }

                return str.Substring(0, end);
            }
        }

        public static string StripStart(string str, string stripChars)
        {
            int strLen = Length(str);
            if (strLen == 0)
            {
                return str;
            }
            else
            {
                int start = 0;
                if (stripChars == null)
                {
                    while (start != strLen && str[start].Equals(""))
                    {
                        ++start;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(stripChars))
                    {
                        return str;
                    }

                    while (start != strLen && stripChars.IndexOf(str[start]) != -1)
                    {
                        ++start;
                    }
                }

                return str.Substring(start);
            }
        }

        public static T Nvl<T>(T value, T defaultValue)
        {
            return value != null ? value : defaultValue;
        }

        public static string DefaultIfEmpty(string str, string defaultStr)
        {
            return IsEmpty(str) ? defaultStr : str;
        }

        public static string TrimToEmpty(String str)
        {
            return str == null ? "" : str.Trim();
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="start">开始</param>
        /// <param name="end">结束</param>
        /// <returns></returns>
        public static string? Substring(string str, int start, int end)
        {
            if (str == null)
            {
                return null;
            }

            if (end < 0)
            {
                end = str.Length + end;
            }
            if (start < 0)
            {
                start = str.Length + start;
            }

            if (end > str.Length)
            {
                end = str.Length;
            }

            if (start > end)
            {
                return null;
            }

            if (start < 0)
            {
                start = 0;
            }
            if (end < 0)
            {
                end = 0;
            }

            return str.Substring(start, end);
        }

        public static string? SubstringBetween(string? str, string open, string close)
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

        public static string? SubstringBefore(string? str, string separator)
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

        public static string SubstringBeforeLast(string? str, string? separator)
        {
            if (!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(separator))
            {
                int pos = str.LastIndexOf(separator);
                return pos == -1 ? str : str.Substring(0, pos);
            }
            else
            {
                return str!;
            }
        }

        public static string SubstringAfterLast(string? str, string? separator)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            else if (string.IsNullOrEmpty(separator))
            {
                return "";
            }
            else
            {
                int pos = str.LastIndexOf(separator);
                return pos != -1 && pos != str.Length - separator.Length ? str.Substring(pos + separator.Length) : "";
            }
        }

        public static bool ContainsIgnoreCase(string? str, string searchStr)
        {
            if(str == null) return false;

            return str.ToLower().Contains(searchStr.ToLower());
        }

        public static bool ContainsAnyIgnoreCase(string? str, IEnumerable<string> searchStrs)
        {            
            if (str == null) return false;

            foreach(var searchStr in searchStrs)
            {
                if(ContainsIgnoreCase(str, searchStr))
                {
                    return true;
                }
            }

            return false;
        }

        public static int CountMatches(string str, string sub)
        {
            if (!IsEmpty(str) && !IsEmpty(sub))
            {
                int count = 0;

                for (int idx = 0; (idx = str.IndexOf(sub, idx)) != -1; idx += sub.Length)
                {
                    ++count;
                }

                return count;
            }
            else
            {
                return 0;
            }
        }

        //public static int IndexOf(string str, string sub, int startIndex)
        //{
        //    return str.IndexOf(sub, startIndex);
        //}

        public static bool StartsWithAny(string sequence, IEnumerable<string> searchStrings)
        {
            if (!string.IsNullOrEmpty(sequence) && searchStrings != null && searchStrings.Count() > 0)
            {
                for (int i = 0; i < searchStrings.Count(); i++)
                {
                    var searchString = searchStrings.ElementAt(i);
                    if (sequence.StartsWith(searchString))
                    {
                        return true;
                    }
                }

                return false;
            }
            else
            {
                return false;
            }
        }
    }
}
