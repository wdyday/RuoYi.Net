namespace RuoYi.Common.Constants
{
    /// <summary>
    /// 通用常量信息
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// UTF-8 字符集
        /// </summary>
        public const string UTF8 = "UTF-8";

        /**
         * GBK 字符集
         */
        public const string GBK = "GBK";

        /**
         * www主域
         */
        public const string WWW = "www.";

        /**
         * http请求
         */
        public const string HTTP = "http://";

        /**
         * https请求
         */
        public const string HTTPS = "https://";

        /**
         * 通用成功标识
         */
        public const string SUCCESS = "0";

        /**
         * 通用失败标识
         */
        public const string FAIL = "1";

        /**
         * 登录成功
         */
        public const string LOGIN_SUCCESS = "Success";

        /**
         * 注销
         */
        public const string LOGOUT = "Logout";

        /**
         * 注册
         */
        public const string REGISTER = "Register";

        /**
         * 登录失败
         */
        public const string LOGIN_FAIL = "Error";

        /**
         * 验证码有效期（分钟）
         */
        public const int CAPTCHA_EXPIRATION = 2;

        /**
         * 令牌
         */
        public const string TOKEN = "token";

        /**
         * 令牌前缀
         */
        public const string TOKEN_PREFIX = "Bearer ";

        /**
         * 令牌前缀
         */
        public const string LOGIN_USER_KEY = "login_user_key";

        /**
         * 用户ID
         */
        public const string JWT_USERID = "userid";

        /**
         * 用户名称
         */
        public const string JWT_USERNAME = "sub";

        /**
         * 用户头像
         */
        public const string JWT_AVATAR = "avatar";

        /**
         * 创建时间
         */
        public const string JWT_CREATED = "created";

        /**
         * 用户权限
         */
        public const string JWT_AUTHORITIES = "authorities";

        /**
         * 资源映射路径 前缀
         */
        public const string RESOURCE_PREFIX = "/profile";

        /**
         * RMI 远程方法调用
         */
        public const string LOOKUP_RMI = "rmi:";

        /**
         * LDAP 远程方法调用
         */
        public const string LOOKUP_LDAP = "ldap:";

        /**
         * LDAPS 远程方法调用
         */
        public const string LOOKUP_LDAPS = "ldaps:";

        /**
         * 自动识别json对象白名单配置（仅允许解析的包名，范围越小越安全）
         */
        public static string[] JSON_WHITELIST_STR = { "org.springframework", "com.ruoyi" };

        /**
         * 定时任务白名单配置（仅允许访问的包名，如其他需要可以自行添加）
         */
        public static string[] JOB_WHITELIST_STR = { "com.ruoyi" };

        /**
         * 定时任务违规的字符
         */
        public static string[] JOB_ERROR_STR = { "java.net.URL", "javax.naming.InitialContext", "org.yaml.snakeyaml",
            "org.springframework", "org.apache", "com.ruoyi.common.utils.file", "com.ruoyi.common.config" };
    }
}
