using Newtonsoft.Json.Linq;
using SqlSugar;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Dynamic;
using System.Reflection;
using System.Text;
using System.Text.Json;

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

        /// <summary>
        /// 将对象转成字典
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ToDictionary(this object input)
        {
            if (input == null) return default;

            // 处理 JObject 类型
            if (input is JObject jobj)
            {
                var dic = new Dictionary<string, object>();
                foreach (var (key, value) in jobj)
                {
                    dic.Add(key, value);
                }

                return dic;
            }

            // 处理本就是字典类型
            if (input.GetType().HasImplementedRawGeneric(typeof(IDictionary<,>)))
            {
                if (input is ExpandoObject expandObject)
                {
                    return expandObject;
                }

                var dic = new Dictionary<string, object>();
                var dicInput = ((IDictionary)input);
                foreach (var key in dicInput.Keys)
                {
                    dic.Add(key.ToString(), dicInput[key]);
                }

                return dic;
            }

            // 处理 JSON 类型
            if (input is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Object)
            {
                return jsonElement.ToObject() as IDictionary<string, object>;
            }

            var valueType = input.GetType();

            // 判断是否是 struct 结构类型
            var isStruct = !valueType.IsPrimitive && !valueType.IsEnum && valueType.IsValueType;

            // 处理基元类型，集合类型
            if (!isStruct && (valueType.IsRichPrimitive()
                || valueType.IsArray
                || (typeof(IEnumerable).IsAssignableFrom(valueType)
                    && valueType.IsGenericType)))
            {
                return new Dictionary<string, object>()
            {
                { "data",input }
            };
            }

            // 剩下的当对象处理
            var properties = input.GetType().GetProperties();
            var fields = input.GetType().GetFields();
            var members = properties.Cast<MemberInfo>().Concat(fields.Cast<MemberInfo>());

            return members.ToDictionary(m => m.Name, m => GetValue(input, m));
        }

        /// <summary>
        /// 获取成员值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        private static object GetValue(object obj, MemberInfo member)
        {
            if (member is PropertyInfo info)
                return info.GetValue(obj, null);

            if (member is FieldInfo info1)
                return info1.GetValue(obj);

            throw new ArgumentException("Passed member is neither a PropertyInfo nor a FieldInfo.");
        }

        /// <summary>
        /// 将流保存到本地磁盘
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static void CopyToSave(this Stream stream, string path)
        {
            // 空检查
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(nameof(path));

            using var fileStream = File.Create(path);
            stream.CopyTo(fileStream);
        }

        /// <summary>
        /// 将字节数组保存到本地磁盘
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static void CopyToSave(this byte[] bytes, string path)
        {
            using var stream = new MemoryStream(bytes);
            stream.CopyToSave(path);
        }

        /// <summary>
        /// 将流保存到本地磁盘
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task CopyToSaveAsync(this Stream stream, string path)
        {
            // 空检查
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(nameof(path));

            using var fileStream = File.Create(path);
            await stream.CopyToAsync(fileStream);
        }

        /// <summary>
        /// 将字节数组保存到本地磁盘
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task CopyToSaveAsync(this byte[] bytes, string path)
        {
            using var stream = new MemoryStream(bytes);
            await stream.CopyToSaveAsync(path);
        }

        #region internal

        /// <summary>
        /// 判断是否是富基元类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        internal static bool IsRichPrimitive(this Type type)
        {
            // 处理元组类型
            if (type.IsValueTuple()) return false;

            // 处理数组类型，基元数组类型也可以是基元类型
            if (type.IsArray) return type.GetElementType().IsRichPrimitive();

            // 基元类型或值类型或字符串类型
            if (type.IsPrimitive || type.IsValueType || type == typeof(string)) return true;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) return type.GenericTypeArguments[0].IsRichPrimitive();

            return false;
        }

        /// <summary>
        /// 判断类型是否实现某个泛型
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="generic">泛型类型</param>
        /// <returns>bool</returns>
        internal static bool HasImplementedRawGeneric(this Type type, Type generic)
        {
            // 检查接口类型
            var isTheRawGenericType = type.GetInterfaces().Any(IsTheRawGenericType);
            if (isTheRawGenericType) return true;

            // 检查类型
            while (type != null && type != typeof(object))
            {
                isTheRawGenericType = IsTheRawGenericType(type);
                if (isTheRawGenericType) return true;
                type = type.BaseType;
            }

            return false;

            // 判断逻辑
            bool IsTheRawGenericType(Type type) => generic == (type.IsGenericType ? type.GetGenericTypeDefinition() : type);
        }

        /// <summary>
        /// 判断是否是元组类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        internal static bool IsValueTuple(this Type type)
        {
            return type.Namespace == "System" && type.Name.Contains("ValueTuple`");
        }
        /// <summary>
        /// JsonElement 转 Object
        /// </summary>
        /// <param name="jsonElement"></param>
        /// <returns></returns>
        internal static object ToObject(this JsonElement jsonElement)
        {
            switch (jsonElement.ValueKind)
            {
                case JsonValueKind.String:
                    return jsonElement.GetString();

                case JsonValueKind.Undefined:
                case JsonValueKind.Null:
                    return default;

                case JsonValueKind.Number:
                    return jsonElement.GetDecimal();

                case JsonValueKind.True:
                case JsonValueKind.False:
                    return jsonElement.GetBoolean();

                case JsonValueKind.Object:
                    var enumerateObject = jsonElement.EnumerateObject();
                    var dic = new Dictionary<string, object>();
                    foreach (var item in enumerateObject)
                    {
                        dic.Add(item.Name, item.Value.ToObject());
                    }
                    return dic;

                case JsonValueKind.Array:
                    var enumerateArray = jsonElement.EnumerateArray();
                    var list = new List<object>();
                    foreach (var item in enumerateArray)
                    {
                        list.Add(item.ToObject());
                    }
                    return list;

                default:
                    return default;
            }
        }

        /// <summary>
        /// 获取类型自定义特性
        /// </summary>
        /// <typeparam name="TAttribute">特性类型</typeparam>
        /// <param name="type">类类型</param>
        /// <param name="inherit">是否继承查找</param>
        /// <returns>特性对象</returns>
        internal static TAttribute GetTypeAttribute<TAttribute>(this Type type, bool inherit = false)
            where TAttribute : Attribute
        {
            // 空检查
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            // 检查特性并获取特性对象
            return type.IsDefined(typeof(TAttribute), inherit)
                ? type.GetCustomAttribute<TAttribute>(inherit)
                : default;
        }

        /// <summary>
        /// 清除字符串前后缀
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="pos">0：前后缀，1：后缀，-1：前缀</param>
        /// <param name="affixes">前后缀集合</param>
        /// <returns></returns>
        internal static string ClearStringAffixes(this string str, int pos = 0, params string[] affixes)
        {
            // 空字符串直接返回
            if (string.IsNullOrWhiteSpace(str)) return str;

            // 空前后缀集合直接返回
            if (affixes == null || affixes.Length == 0) return str;

            var startCleared = false;
            var endCleared = false;

            string tempStr = null;
            foreach (var affix in affixes)
            {
                if (string.IsNullOrWhiteSpace(affix)) continue;

                if (pos != 1 && !startCleared && str.StartsWith(affix, StringComparison.OrdinalIgnoreCase))
                {
                    tempStr = str[affix.Length..];
                    startCleared = true;
                }
                if (pos != -1 && !endCleared && str.EndsWith(affix, StringComparison.OrdinalIgnoreCase))
                {
                    var _tempStr = !string.IsNullOrWhiteSpace(tempStr) ? tempStr : str;
                    tempStr = _tempStr[..^affix.Length];
                    endCleared = true;

                    if (string.IsNullOrWhiteSpace(tempStr))
                    {
                        tempStr = null;
                        endCleared = false;
                    }
                }
                if (startCleared && endCleared) break;
            }

            return !string.IsNullOrWhiteSpace(tempStr) ? tempStr : str;
        }

        /// <summary>
        /// 将一个对象转换为指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static T ChangeType<T>(this object obj)
        {
            return (T)ChangeType(obj, typeof(T));
        }

        /// <summary>
        /// 将一个对象转换为指定类型
        /// </summary>
        /// <param name="obj">待转换的对象</param>
        /// <param name="type">目标类型</param>
        /// <returns>转换后的对象</returns>
        internal static object ChangeType(this object obj, Type type)
        {
            if (type == null) return obj;
            if (type == typeof(string)) return obj?.ToString();
            if (type == typeof(Guid) && obj != null) return Guid.Parse(obj.ToString());
            if (type == typeof(bool) && obj != null && obj is not bool)
            {
                var objStr = obj.ToString().ToLower();
                if (objStr == "1" || objStr == "true" || objStr == "yes" || objStr == "on") return true;
                return false;
            }
            if (obj == null) return type.IsValueType ? Activator.CreateInstance(type) : null;

            var underlyingType = Nullable.GetUnderlyingType(type);
            if (type.IsAssignableFrom(obj.GetType())) return obj;
            else if ((underlyingType ?? type).IsEnum)
            {
                if (underlyingType != null && string.IsNullOrWhiteSpace(obj.ToString())) return null;
                else return Enum.Parse(underlyingType ?? type, obj.ToString());
            }
            // 处理DateTime -> DateTimeOffset 类型
            else if (obj.GetType().Equals(typeof(DateTime)) && (underlyingType ?? type).Equals(typeof(DateTimeOffset)))
            {
                return ((DateTime)obj).ConvertToDateTimeOffset();
            }
            // 处理 DateTimeOffset -> DateTime 类型
            else if (obj.GetType().Equals(typeof(DateTimeOffset)) && (underlyingType ?? type).Equals(typeof(DateTime)))
            {
                return ((DateTimeOffset)obj).ConvertToDateTime();
            }
            else if (typeof(IConvertible).IsAssignableFrom(underlyingType ?? type))
            {
                try
                {
                    return Convert.ChangeType(obj, underlyingType ?? type, null);
                }
                catch
                {
                    return underlyingType == null ? Activator.CreateInstance(type) : null;
                }
            }
            else
            {
                var converter = TypeDescriptor.GetConverter(type);
                if (converter.CanConvertFrom(obj.GetType())) return converter.ConvertFrom(obj);

                var constructor = type.GetConstructor(Type.EmptyTypes);
                if (constructor != null)
                {
                    var o = constructor.Invoke(null);
                    var propertys = type.GetProperties();
                    var oldType = obj.GetType();

                    foreach (var property in propertys)
                    {
                        var p = oldType.GetProperty(property.Name);
                        if (property.CanWrite && p != null && p.CanRead)
                        {
                            property.SetValue(o, ChangeType(p.GetValue(obj, null), property.PropertyType), null);
                        }
                    }
                    return o;
                }
            }
            return obj;
        }

        /// <summary>
        /// 合并两个字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dic">字典</param>
        /// <param name="newDic">新字典</param>
        /// <returns></returns>
        internal static Dictionary<string, T> AddOrUpdate<T>(this Dictionary<string, T> dic, IDictionary<string, T> newDic)
        {
            foreach (var key in newDic.Keys)
            {
                if (dic.TryGetValue(key, out var value))
                {
                    dic[key] = value;
                }
                else
                {
                    dic.Add(key, newDic[key]);
                }
            }

            return dic;
        }

        /// <summary>
        /// 合并两个字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dic">字典</param>
        /// <param name="newDic">新字典</param>
        internal static void AddOrUpdate<T>(this ConcurrentDictionary<string, T> dic, Dictionary<string, T> newDic)
        {
            foreach (var (key, value) in newDic)
            {
                dic.AddOrUpdate(key, value, (key, old) => value);
            }
        }

        #endregion
    }
}
