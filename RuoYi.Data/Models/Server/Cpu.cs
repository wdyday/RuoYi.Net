namespace RuoYi.Data.Models
{
    /// <summary>
    /// CPU相关信息
    /// </summary>
    public class Cpu
    {
        /**
         * 核心数
         */
        public int CpuNum { get; set; }

        /**
         * CPU总的使用率
         */
        public double Total { get; set; }

        /**
         * CPU系统使用率
         */
        public string? Sys { get; set; }

        /**
         * CPU用户使用率
         */
        public string? Used { get; set; }

        /**
         * CPU当前等待率
         */
        public double Wait { get; set; }

        /**
         * CPU当前空闲率
         */
        public double Free { get; set; }
    }
}
