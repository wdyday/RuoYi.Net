namespace RuoYi.Data.Models
{
    /// <summary>
    /// 內存相关信息
    /// </summary>
    public class Mem
    {
        /**
         * 内存总量
         */
        public double Total { get; set; }

        /**
         * 已用内存
         */
        public double Used { get; set; }

        /**
         * 剩余内存
         */
        public double Free { get; set; }

        /// <summary>
        /// 使用率
        /// </summary>
        public decimal Usage { get; set; }
    }
}
