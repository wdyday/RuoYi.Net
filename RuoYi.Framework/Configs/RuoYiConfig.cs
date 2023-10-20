namespace RuoYi.Framework.Configs
{
    public class RuoYiConfig
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// 版权年份
        /// </summary>
        public string? CopyrightYear { get; set; }

        /// <summary>
        /// 实例演示开关
        /// </summary>
        public bool DemoEnabled { get; set; }

        /// <summary>
        /// 上传路径
        /// </summary>
        public string? Profile { get; set; }

        /// <summary>
        /// 获取地址开关
        /// </summary>
        public bool AddressEnabled { get; set; }

        /// <summary>
        /// 验证码类型
        /// </summary>
        public string? CaptchaType { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        public string AvatarPath
        {
            get { return Path.Combine(Profile, "avatar"); }
        }

        /// <summary>
        /// 获取下载路径
        /// </summary>
        public string DownloadPath
        {
            get { return Path.Combine(Profile, "download"); }
        }

        /// <summary>
        /// 获取上传路径
        /// </summary>
        public string UploadPath
        {
            get { return Path.Combine(Profile, "upload"); }
        }
    }
}
