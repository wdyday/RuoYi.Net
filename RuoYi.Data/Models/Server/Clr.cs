namespace RuoYi.Data.Models
{
    /// <summary>
    /// CLR相关信息
    /// </summary>
    public class Clr
    {
        /// <summary>
        /// SKD名称
        /// </summary>
        public string? Name { get; set; }

        /**
         * 当前CLR占用的内存总数(M)
         */
        public string? Total{ get; set; }

        /**
         * CLR最大可用内存总数(M)
         */
        public double Max{ get; set; }

        /**
         * CLR空闲内存(M)
         */
        public string? Free{ get; set; }

        /**
         * .NET版本
         */
        public string Version{ get; set; }

        /**
         * .NET路径
         */
        public string Home{ get; set; }

        /// <summary>
        /// 启动时间
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 运行时间
        /// </summary>
        public string RunTime { get; set; }

        /// <summary>
        /// 运行参数
        /// </summary>
        public string InputArgs { get; set; }

        /// <summary>
        /// 已使用内存(M)
        /// </summary>
        public double Used { get; set; }

        /// <summary>
        /// 使用率
        /// </summary>
        public string? Usage { get; set; }
    }
}
