using Mapster;
using System.Reflection;
using System.Runtime.Loader;

namespace RuoYi.Framework.Utils
{
    public static class ReflectUtils
    {
        public static object GetPropertyValue(object target, string propertyName)
        {
            //return target.GetType().GetProperty(propertyName).GetValue(target, null);
            var type = target.GetType();
            return type.InvokeMember(propertyName, BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance, Type.DefaultBinder, target, null)!;
        }

        public static T GetPropertyValue<T>(object target, string propertyName)
        {
            //return target.GetType().GetProperty(propertyName).GetValue(target, null);
            var type = target.GetType();
            var val = type.InvokeMember(propertyName, BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance, Type.DefaultBinder, target, null);
            return val != null ? (T)val : default!;
        }

        public static void SetPropertyValue(object target, string propertyName, object? value)
        {
            var type = target.GetType();
            type.InvokeMember(propertyName, BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance, Type.DefaultBinder, target, new[] { value });
        }

        // 按类型创建实例
        public static object CreateInstance(Type type)
        {
            var instance = Activator.CreateInstance(type, true);
            return instance!;
        }
        public static T CreateInstance<T>()
        {
            var instance = Activator.CreateInstance(typeof(T), true);
            return (T)instance!;
        }

        public static bool ExistProperty(object target, string propertyName)
        {
            PropertyInfo[] properties = target.GetType().GetProperties();

            return properties.Length > 0 && properties.Where(f => f.Name == propertyName).Any();
        }

        public static void Map(object from, object to, IEnumerable<string> excludeNames = null, Dictionary<string, string> mapNames = null)
        {
            excludeNames = excludeNames != null ? excludeNames : new List<string>();
            mapNames = mapNames != null ? mapNames : new Dictionary<string, string>();

            var fromProperties = from.GetType().GetProperties();
            foreach (var fromProperty in fromProperties)
            {
                if (excludeNames.Contains(fromProperty.Name)) continue;

                // to 中包含同名属性
                if (ExistProperty(to, fromProperty.Name))
                {
                    SetPropertyValue(to, fromProperty.Name, fromProperty.GetValue(from)!);
                }
                else if (mapNames.ContainsKey(fromProperty.Name) && ExistProperty(to, mapNames[fromProperty.Name]))
                {
                    SetPropertyValue(to, mapNames[fromProperty.Name], fromProperty.GetValue(from)!);
                }
            }
        }

        public static List<string> GetPropertyNames<T>()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            return properties.Select(p => p.Name).ToList();
        }

        public static IEnumerable<PropertyInfo> GetPropertyInfos<TDestination>()
        {
            var type = typeof(TDestination);
            var infos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return infos;
        }

        public static TDestination CopyTo<TSource, TDestination>(TSource source) where TSource : class where TDestination : class
        {
            if (source == null) return default;
            
            var destination = source.Adapt<TDestination>();
            return destination;
        }

        public static List<TDestination> CopyToList<TSource, TDestination>(List<TSource> list) where TSource : class where TDestination : class
        {
            List<TDestination> dstList = new List<TDestination>();
            foreach (TSource source in list)
            {
                if (source != null)
                {
                    dstList.Add(CopyTo<TSource, TDestination>(source));
                }
            }
            return dstList;
        }

    }
}
