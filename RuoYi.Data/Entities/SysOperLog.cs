using SqlSugar;

namespace RuoYi.Data.Entities
{
    /// <summary>
    ///  操作日志记录 对象 sys_oper_log
    ///  author ruoyi
    ///  date   2023-09-28 12:38:39
    /// </summary>
    [SugarTable("sys_oper_log", "操作日志记录")]
    public class SysOperLog : BaseEntity
    {
        /// <summary>
        /// 日志主键 (oper_id)
        /// </summary>
        [SugarColumn(ColumnName = "oper_id", ColumnDescription = "日志主键", IsPrimaryKey = true, IsIdentity = true)]
        public long OperId { get; set; }
        /// <summary>
        /// 模块标题 (title)
        /// </summary>
        [SugarColumn(ColumnName = "title", ColumnDescription = "模块标题")]
        public string? Title { get; set; }
        /// <summary>
        /// 业务类型（0其它 1新增 2修改 3删除） (business_type)
        /// </summary>
        [SugarColumn(ColumnName = "business_type", ColumnDescription = "业务类型（0其它 1新增 2修改 3删除）")]
        public int? BusinessType { get; set; }
        /// <summary>
        /// 方法名称 (method)
        /// </summary>
        [SugarColumn(ColumnName = "method", ColumnDescription = "方法名称")]
        public string? Method { get; set; }
        /// <summary>
        /// 请求方式 (request_method)
        /// </summary>
        [SugarColumn(ColumnName = "request_method", ColumnDescription = "请求方式")]
        public string? RequestMethod { get; set; }
        /// <summary>
        /// 操作类别（0其它 1后台用户 2手机端用户） (operator_type)
        /// </summary>
        [SugarColumn(ColumnName = "operator_type", ColumnDescription = "操作类别（0其它 1后台用户 2手机端用户）")]
        public int? OperatorType { get; set; }
        /// <summary>
        /// 操作人员 (oper_name)
        /// </summary>
        [SugarColumn(ColumnName = "oper_name", ColumnDescription = "操作人员")]
        public string? OperName { get; set; }
        /// <summary>
        /// 部门名称 (dept_name)
        /// </summary>
        [SugarColumn(ColumnName = "dept_name", ColumnDescription = "部门名称")]
        public string? DeptName { get; set; }
        /// <summary>
        /// 请求URL (oper_url)
        /// </summary>
        [SugarColumn(ColumnName = "oper_url", ColumnDescription = "请求URL")]
        public string? OperUrl { get; set; }
        /// <summary>
        /// 主机地址 (oper_ip)
        /// </summary>
        [SugarColumn(ColumnName = "oper_ip", ColumnDescription = "主机地址")]
        public string? OperIp { get; set; }
        /// <summary>
        /// 操作地点 (oper_location)
        /// </summary>
        [SugarColumn(ColumnName = "oper_location", ColumnDescription = "操作地点")]
        public string? OperLocation { get; set; }
        /// <summary>
        /// 请求参数 (oper_param)
        /// </summary>
        [SugarColumn(ColumnName = "oper_param", ColumnDescription = "请求参数")]
        public string? OperParam { get; set; }
        /// <summary>
        /// 返回参数 (json_result)
        /// </summary>
        [SugarColumn(ColumnName = "json_result", ColumnDescription = "返回参数")]
        public string? JsonResult { get; set; }
        /// <summary>
        /// 操作状态（0正常 1异常） (status)
        /// </summary>
        [SugarColumn(ColumnName = "status", ColumnDescription = "操作状态（0正常 1异常）")]
        public int? Status { get; set; }
        /// <summary>
        /// 错误消息 (error_msg)
        /// </summary>
        [SugarColumn(ColumnName = "error_msg", ColumnDescription = "错误消息")]
        public string? ErrorMsg { get; set; }
        /// <summary>
        /// 操作时间 (oper_time)
        /// </summary>
        [SugarColumn(ColumnName = "oper_time", ColumnDescription = "操作时间")]
        public DateTime? OperTime { get; set; }
        /// <summary>
        /// 消耗时间 (cost_time)
        /// </summary>
        [SugarColumn(ColumnName = "cost_time", ColumnDescription = "消耗时间")]
        public long? CostTime { get; set; }
    }
}
