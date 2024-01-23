namespace RuoYi.Data.Models
{
    /// <summary>
    /// 路由显示信息
    /// </summary>
    public class MetaVo
    {
        /**
         * 设置该路由在侧边栏和面包屑中展示的名字
         */
        public string Title { get; set; }

        /**
         * 设置该路由的图标，对应路径src/assets/icons/svg
         */
        public string Icon { get; set; }

        /**
         * 设置为true，则不会被 <keep-alive>缓存
         */
        public bool NoCache { get; set; }

        /**
         * 内链地址（http(s)://开头）
         */
        public string Link { get; set; }

        public MetaVo()
        {
        }

        public MetaVo(string title, string icon)
        {
            this.Title = title;
            this.Icon = icon;
        }

        public MetaVo(string title, string icon, bool noCache)
        {
            this.Title = title;
            this.Icon = icon;
            this.NoCache = noCache;
        }

        public MetaVo(string title, string icon, string link)
        {
            this.Title = title;
            this.Icon = icon;
            this.Link = link;
        }

        public MetaVo(string title, string icon, bool noCache, string link)
        {
            this.Title = title;
            this.Icon = icon;
            this.NoCache = noCache;
            if (!string.IsNullOrEmpty(link) && link.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                this.Link = link;
            }
        }

    }
}
