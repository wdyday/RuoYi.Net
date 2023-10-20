using Microsoft.AspNetCore.Mvc;

namespace RuoYi.Data.Dtos
{
    public class BaseDto
    {
        /// <summary>
        /// 创建人
        /// </summary>
        public string? CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string? UpdateBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        [FromQuery(Name = "")]
        public QueryParam Params { get; set; } = new QueryParam();

    }

    public class QueryParam
    {
        [FromQuery(Name = "params[beginTime]")]
        public DateTime? BeginTime { get; set; }
        [FromQuery(Name = "params[endTime]")]
        public DateTime? EndTime { get; set; }

        public string? DataScopeSql { get; set; }
    }
}
