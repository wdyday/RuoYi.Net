namespace RuoYi.Data.Models
{
    /// <summary>
    /// 系统文件相关信息
    /// </summary>
    public class SysFile
    {
        /**
         * 盘符路径
         */
        public string DirName{ get; set; }

        /**
         * 文件类型
         */
        public string SysTypeName{ get; set; }

        /**
         * 盘符类型
         */
        public string TypeName{ get; set; }

        /**
         * 总大小
         */
        public string Total{ get; set; }

        /**
         * 剩余大小
         */
        public string Free{ get; set; }

        /**
         * 已经使用量
         */
        public string Used{ get; set; }

        /**
         * 资源的使用率
         */
        public decimal Usage{ get; set; }
    }
}
