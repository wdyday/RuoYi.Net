using RuoYi.Framework;
using System.Reflection;

namespace RuoYi.Web.Entry
{
    public class SingleFilePublish : ISingleFilePublish
    {
        public Assembly[] IncludeAssemblies()
        {
            return Array.Empty<Assembly>();
        }

        public string[] IncludeAssemblyNames()
        {
            return new[]
            {
                "RuoYi.Framework",
                "RuoYi.Common",
                "RuoYi.Data",
                "RuoYi.Web.Entry",
                "RuoYi.Admin",
                "RuoYi.Generator",
                "RuoYi.System"
            };
        }
    }
}