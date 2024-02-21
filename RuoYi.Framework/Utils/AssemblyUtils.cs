using RuoYi.Framework.Attributes;
using System.Reflection;

namespace RuoYi.Framework.Utils
{
    public static class AssemblyUtils
    {
        /// <summary>
        /// 查找被当前属性标记的类
        /// </summary>
        /// <param name="attribute">当前属性</param>
        /// <returns></returns>
        public static List<Type> GetClassTypes(Type attributeType)
        {
            var list = new List<Type>();

            // 业务逻辑项目 
            var whiteListAssembly = App.GetConfig<string>("JobWhiteListAssembly");
            var whiteListAssemblies = !string.IsNullOrEmpty(whiteListAssembly) ? whiteListAssembly.Split(",") : new string[] { };
            var businessAssemblyPrefixes = whiteListAssemblies.Length > 0 ? whiteListAssemblies : new string[] { "RuoYi." };
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => StringUtils.ContainsAnyIgnoreCase(a.FullName, businessAssemblyPrefixes))
                .ToList();
            foreach (var assembly in assemblies)
            {
                Type[] types = assembly.GetExportedTypes();
                foreach (Type type in types)
                {
                    var attrs = type.GetCustomAttributes(attributeType);
                    foreach (var attr in attrs)
                    {
                        if (attr.GetType().Equals(attributeType))
                        {
                            list.Add(type);
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 取 带有 TaskAttribute 特性的类的类型
        /// </summary>
        /// <param name="name">TaskAttribute.Name, 如 ryTask</param>
        /// <returns></returns>
        public static Type? GetTaskAttributeClassType(string name)
        {
            var attrType = typeof(TaskAttribute);
            var types = GetClassTypes(attrType);
            foreach (var type in types)
            {
                var attr = type.GetCustomAttributes(attrType, false).First();
                var attrName = ReflectUtils.GetPropertyValue<string>(attr, "Name");
                if (name.Equals(attrName))
                {
                    return type;
                }
            }

            return null;
        }

        /// <summary>
        /// 取 带有 某特性的类的类型
        /// </summary>
        /// <param name="attributeType">某特性的类型</param>
        /// <param name="className">类名字符串: 如 RyTask</param>
        /// <returns></returns>
        public static Type? GetClassType(Type attributeType, string className)
        {
            var types = GetClassTypes(attributeType);
            return types.Where(t => t.Name.Equals(className)).FirstOrDefault();
        }

        /// <summary>
        /// 取 带有某特性的类的 命名空间
        /// </summary>
        /// <param name="attributeType">某特性</param>
        /// <param name="className">类名</param>
        /// <returns></returns>
        public static string GetAttributeClassNamespace(Type attributeType, string className)
        {
            var types = GetClassTypes(attributeType);
            return GetNamespace(types, className);
        }

        /// <summary>
        /// 取 带有TaskAttribute特性的类的 命名空间
        /// </summary>
        /// <param name="attributeType">某特性</param>
        /// <param name="name">TaskAttribute.Name, 如 ryTask</param>
        /// <returns></returns>
        public static string GetTaskAttributeClassNamespace(string name)
        {
            var type = GetTaskAttributeClassType( name);
            return type?.Namespace ?? string.Empty;
        }

        public static string GetNamespace(List<Type> types, string className)
        {
            var type = types.Where(t => t.Name.Equals(className)).FirstOrDefault();
            return type?.Namespace ?? string.Empty;
        }
    }
}
