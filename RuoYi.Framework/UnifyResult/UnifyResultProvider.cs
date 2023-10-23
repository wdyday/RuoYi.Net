using Furion;
using Furion.DataValidation;
using Furion.FriendlyException;
using Furion.UnifyResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RuoYi.Framework
{
    /// <summary>
    /// RESTful 风格返回值 TO REMOVE
    /// </summary>
    [UnifyModel(typeof(AjaxResult))]
    public class UnifyResultProvider : IUnifyResultProvider
    {
        /// <summary>
        /// 异常返回值
        /// </summary>
        /// <param name="context"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public IActionResult OnException(ExceptionContext context, ExceptionMetadata metadata)
        {
            return new JsonResult(Result(metadata.StatusCode, data: metadata.Data, msg: metadata.Errors?.ToString()!)
                , UnifyContext.GetSerializerSettings(context)); // 当前行仅限 Furion 4.6.6+ 使用
        }

        /// <summary>
        /// 成功返回值
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public IActionResult OnSucceeded(ActionExecutedContext context, object data)
        {
            return new JsonResult(Result(StatusCodes.Status200OK, data)
                , UnifyContext.GetSerializerSettings(context)); // 当前行仅限 Furion 4.6.6+ 使用
        }

        /// <summary>
        /// 验证失败返回值
        /// </summary>
        /// <param name="context"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public IActionResult OnValidateFailed(ActionExecutingContext context, ValidationMetadata metadata)
        {
            return new JsonResult(Result(metadata.StatusCode ?? StatusCodes.Status400BadRequest, data: metadata.Data, msg: metadata.ValidationResult?.ToString()!)
                , UnifyContext.GetSerializerSettings(context)); // 当前行仅限 Furion 4.6.6+ 使用
        }

        /// <summary>
        /// 特定状态码返回值
        /// </summary>
        /// <param name="context"></param>
        /// <param name="statusCode"></param>
        /// <param name="unifyResultSettings"></param>
        /// <returns></returns>
        public async Task OnResponseStatusCodes(HttpContext context, int statusCode, UnifyResultSettingsOptions unifyResultSettings)
        {
            // 设置响应状态码
            UnifyContext.SetResponseStatusCodes(context, statusCode, unifyResultSettings);

            switch (statusCode)
            {
                // 处理 401 状态码
                case StatusCodes.Status401Unauthorized:
                    await context.Response.WriteAsJsonAsync(Result(statusCode, msg: "401 Unauthorized")
                        , App.GetOptions<JsonOptions>()?.JsonSerializerOptions);
                    break;
                // 处理 403 状态码
                case StatusCodes.Status403Forbidden:
                    await context.Response.WriteAsJsonAsync(Result(statusCode, msg: "403 Forbidden")
                        , App.GetOptions<JsonOptions>()?.JsonSerializerOptions);
                    break;
                default: break;
            }
        }

        /// <summary>
        /// 返回 RESTful 风格结果集
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="succeed"></param>
        /// <param name="data"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        private static AjaxResult Result(int statusCode, object data = default, string msg = default)
        {
            return new AjaxResult(statusCode, msg, data);
        }
    }

}
