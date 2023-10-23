namespace RuoYi.Data.Models
{
    public class Server
    {
        /**
         * CPU相关信息
         */
        public Cpu Cpu { get; set; } = new Cpu();

        /**
         * 內存相关信息
         */
        public Mem Mem { get; set; } = new Mem();

        /**
         * CLR相关信息
         */
        public Clr Clr { get; set; } = new Clr();

        /**
         * 服务器相关信息
         */
        public Sys Sys { get; set; } = new Sys();

        /**
         * 磁盘相关信息
         */
        public List<SysFile> SysFiles { get; set; } = new List<SysFile>();
    }
}
