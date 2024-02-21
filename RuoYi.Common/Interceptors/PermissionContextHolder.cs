using RuoYi.Framework;

namespace RuoYi.Common.Interceptors
{
    /// <summary>
    /// 权限信息
    /// </summary>
    public static class PermissionContextHolder
    {
        private const string PERMISSION_CONTEXT_ATTRIBUTES = "PERMISSION_CONTEXT";

        public static void SetContext(string permission)
        {
            if(App.HttpContext.Items.ContainsKey(PERMISSION_CONTEXT_ATTRIBUTES))
                App.HttpContext.Items.Remove(PERMISSION_CONTEXT_ATTRIBUTES);

            App.HttpContext.Items.Add(PERMISSION_CONTEXT_ATTRIBUTES, permission);
        }

        public static string GetContext()
        {
            object permission;
            App.HttpContext.Items.TryGetValue(PERMISSION_CONTEXT_ATTRIBUTES, out permission);
            return permission?.ToString()!;
        }
    }
}
