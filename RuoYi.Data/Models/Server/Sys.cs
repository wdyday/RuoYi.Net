namespace RuoYi.Data.Models
{
    /// <summary>
    /// 系统相关信息
    /// </summary>
    public class Sys
    {
        /**
         * 服务器名称
         */
        public string ComputerName { get; set; }

        /**
         * 服务器Ip
         */
        public string ComputerIp { get; set; }

        /**
         * 项目路径
         */
        public string UserDir { get; set; }

        /**
         * 操作系统
         */
        public string OsName { get; set; }

        /**
         * 系统架构
         */
        public string OsArch { get; set; }
    }
}
