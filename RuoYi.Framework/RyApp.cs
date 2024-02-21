using RuoYi.Framework.Configs;

namespace RuoYi.Framework
{
    public static class RyApp
    {
        public static RuoYiConfig RuoYiConfig = App.GetConfig<RuoYiConfig>("RuoYiConfig");
        public static UserConfig UserConfig = App.GetConfig<UserConfig>("UserConfig");
    }
}
