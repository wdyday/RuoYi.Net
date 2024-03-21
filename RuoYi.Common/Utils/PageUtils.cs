using RuoYi.Common.Data;
using RuoYi.Framework;
using RuoYi.Framework.Extensions;

namespace RuoYi.Common.Utils
{
    public class PageUtils
    {
        public static PageDomain GetPageDomain()
        {
            var request = App.HttpContext.Request;
            var pageNum = !string.IsNullOrEmpty(request?.Query["pageNum"]) ? Convert.ToInt32(request?.Query["pageNum"]) : 1;
            var pageSize = !string.IsNullOrEmpty(request?.Query["pageSize"]) ? Convert.ToInt32(request?.Query["pageSize"]) : 10;
            var orderByColumn = request?.Query["orderByColumn"].ToString();
            var isAsc = request?.Query["isAsc"];

            var orderBy = !string.IsNullOrEmpty(orderByColumn) ? $"{orderByColumn.ToUnderScoreCase()} {isAsc}" : "";

            return new PageDomain
            {
                PageNum = pageNum > 0 ? pageNum : 1,
                PageSize = pageSize > 0 ? pageSize : 10,
                OrderByColumn = orderByColumn,
                PropertyName = orderByColumn.ToUpperCamelCase(),
                OrderBy = orderBy,
                IsAsc = isAsc
            };
        }
    }
}
