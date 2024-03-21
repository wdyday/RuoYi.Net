using RuoYi.Data.Attributes;

namespace RuoYi.Data.Dtos
{
    /// <summary>
    ///  操作日志记录 对象 sys_oper_log
    ///  author ruoyi
    ///  date   2023-09-28 12:38:39
    /// </summary>
    public class SysOperLogDto : BaseDto
    {
        /// <summary>
        /// 日志主键
        /// </summary>
        [Excel(Name = "操作序号")]
        public long OperId { get; set; }

        /// <summary>
        /// 模块标题
        /// </summary>
        [Excel(Name = "操作模块")]
        public string? Title { get; set; }

        /// <summary>
        /// 业务类型（0其它 1新增 2修改 3删除）
        /// </summary>
        public int? BusinessType { get; set; }

        [Excel(Name = "业务类型")]
        public string? BusinessTypeDesc { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        public string? Method { get; set; }
        /// <summary>
        /// 请求方式
        /// </summary>
        public string? RequestMethod { get; set; }
        /// <summary>
        /// 操作类别（0其它 1后台用户 2手机端用户）
        /// </summary>
        public int? OperatorType { get; set; }
        /// <summary>
        /// 操作人员
        /// </summary>
        public string? OperName { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string? DeptName { get; set; }
        /// <summary>
        /// 请求URL
        /// </summary>
        public string? OperUrl { get; set; }
        /// <summary>
        /// 主机地址
        /// </summary>
        public string? OperIp { get; set; }
        /// <summary>
        /// 操作地点
        /// </summary>
        public string? OperLocation { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
        public string? OperParam { get; set; }
        /// <summary>
        /// 返回参数
        /// </summary>
        public string? JsonResult { get; set; }
        /// <summary>
        /// 操作状态（0正常 1异常）
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string? ErrorMsg { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime? OperTime { get; set; }
        /// <summary>
        /// 消耗时间
        /// </summary>
        public long? CostTime { get; set; }
    }
}
