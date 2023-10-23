namespace RuoYi.Data
{
    public class CacheConstants
    {
        /// <summary>
        /// 登录用户 redis key
        /// </summary>
        public const string LOGIN_TOKEN_KEY = "login_tokens:";

        /// <summary>
        /// 参数管理 cache key
        /// </summary>
        public const string SYS_CONFIG_KEY = "sys_config:";

        /// <summary>
        /// 验证码 redis key
        /// </summary>
        public const string CAPTCHA_CODE_KEY = "captcha_codes:";

        /// <summary>
        /// 登录账户密码错误次数 redis key
        /// </summary>
        public const string PWD_ERR_CNT_KEY = "pwd_err_cnt:";

        /// <summary>
        /// 字典管理 cache key
        /// </summary>
        public const string SYS_DICT_KEY = "sys_dict:";
    }
}
