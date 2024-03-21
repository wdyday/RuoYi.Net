using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using RuoYi.Common.Enums;
using RuoYi.Common.Utils;
using RuoYi.Data.Models;
using RuoYi.Framework.JsonSerialization;
using RuoYi.Framework.UnifyResult;
using RuoYi.System.Services;
using System.Diagnostics;
using System.Reflection;

namespace RuoYi.System
{
    public class LogAttribute : Attribute, IAsyncActionFilter
    {
        #region 参数
        /// <summary>
        /// 模块
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// 功能
        /// </summary>
        public BusinessType BusinessType { get; set; } = BusinessType.OTHER;

        /// <summary>
        /// 操作人类别
        /// </summary>
        public OperatorType OperatorType { get; set; } = OperatorType.MANAGE;

        /// <summary>
        /// 是否保存请求的参数
        /// </summary>
        public bool IsSaveRequestData { get; set; } = true;

        /// <summary>
        /// 是否保存响应的参数
        /// </summary>
        public bool IsSaveResponseData { get; set; } = true;

        /// <summary>
        /// 排除指定的请求参数
        /// </summary>
        public string[] ExcludeParamNames { get; set; } = new string[0];
        #endregion

        /** 排除敏感属性字段 */
        private static string[] EXCLUDE_PROPERTIES = { "password", "oldPassword", "newPassword", "confirmPassword" };


        /// <summary>
        /// 监视 Action 执行
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 获取动作方法描述器
            var actionMethod = (context.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo;

            // 处理 Blazor Server
            if (actionMethod == null)
            {
                _ = await next.Invoke();
                return;
            }

            await HandleLogAsync(actionMethod, context.ActionArguments, context, next);
        }

        private async Task HandleLogAsync(MethodInfo actionMethod, IDictionary<string, object> parameterValues, FilterContext context, dynamic next)
        {
            // 排除 WebSocket 请求处理
            if (context.HttpContext.IsWebSocketRequest())
            {
                _ = await next();
                return;
            }

            // 获取当前的用户
            LoginUser loginUser = SecurityUtils.GetLoginUser();

            // 获取方法完整名称
            var methodFullName = actionMethod.DeclaringType!.FullName + "." + actionMethod.Name;

            // 获取 HttpContext 和 HttpRequest 对象
            var httpContext = context.HttpContext;
            var httpRequest = httpContext.Request;

            // 获取服务端 IPv4 地址
            //var localIPv4 = httpContext.GetLocalIpAddressToIPv4();

            // 获取客户端 Ipv4 地址
            var remoteIPv4 = httpContext.GetRemoteIpAddressToIPv4();
            // 远程查询操作地点
            var operLocation = await AddressUtils.GetRealAddressByIPAsync(remoteIPv4);

            // 获取请求方式
            var httpMethod = httpContext.Request.Method;

            // 获取请求的 Url 地址
            var requestUrl = httpRequest.GetRequestUrlAddress();

            // ------------------- 计算接口执行时间
            var timeOperation = Stopwatch.StartNew();
            var resultContext = await next();
            timeOperation.Stop();

            // 获取异常对象情况
            Exception e = resultContext.Exception;

            // *========数据库日志=========*//
            var operLog = new SysOperLog
            {
                Status = e != null ? BusinessStatus.FAIL.ToInt() : BusinessStatus.SUCCESS.ToInt(),
                ErrorMsg = e != null ? StringUtils.Substring(e.Message, 0, 2000) : null,

                // 请求的地址
                OperIp = remoteIPv4,
                OperUrl = requestUrl,
                OperName = loginUser != null ? loginUser.UserName : null,
                OperTime = DateTime.Now,

                // 远程查询操作地点
                OperLocation = operLocation,

                // 设置方法名称
                Method = methodFullName,
                // 设置请求方式
                RequestMethod = httpMethod,

                // 处理设置注解上的参数
                // 设置action动作
                BusinessType = BusinessType.ToInt(),
                // 设置标题
                Title = Title,

                // 设置操作人类别
                OperatorType = OperatorType.ToInt(),

                // 是否需要保存request，参数和值
                OperParam = IsSaveRequestData ? GetOperParam(parameterValues, ExcludeParamNames) : null,

                // 是否需要保存response，参数和值
                JsonResult = IsSaveRequestData ? GetResponseData(resultContext) : null,

                // 设置消耗时间
                CostTime = timeOperation.ElapsedMilliseconds
            };

            // 保存数据库
            _ = Task.Factory.StartNew(async () =>
            {
                var sysOperLogService = App.GetService<SysOperLogService>();
                await sysOperLogService.InsertAsync(operLog);
            });
        }

        // 获取请求的参数，放到log中
        private string? GetOperParam(IDictionary<string, object> parameterValues, string[] excludeParamNames)
        {
            var values = new Dictionary<string, object>();
            foreach (var parameterValue in parameterValues)
            {
                if (excludeParamNames.Contains(parameterValue.Key))
                {
                    continue;
                }
                //values.Add(parameterValue.Key, parameterValue.Value);

                var value = parameterValue.Value;

                object rawValue = default;

                // 文件类型参数
                if (value is IFormFile || value is List<IFormFile>)
                {
                    // 单文件
                    if (value is IFormFile formFile)
                    {
                        values.Add(parameterValue.Key, formFile.FileName);
                    }
                    // 多文件
                    else if (value is List<IFormFile> formFiles)
                    {
                        var fileNames = formFiles.Select(f => f.FileName).ToArray();
                        values.Add(parameterValue.Key, fileNames);
                    }
                }
                // 处理 byte[] 参数类型
                else if (value is byte[] byteArray)
                {
                    values.Add(parameterValue.Key, "byte[]");
                }
                // 处理基元类型，字符串类型和空值
                else if (value is string || value == null)
                {
                    values.Add(parameterValue.Key, parameterValue.Value);
                }
                // 其他类型统一进行序列化
                else
                {
                    values.Add(parameterValue.Key, parameterValue.Value);
                }
            }
            return JSON.Serialize(values);
        }

        // response，参数和值
        private string? GetResponseData(dynamic resultContext)
        {
            object returnValue = null;
            var result = resultContext.Result as IActionResult;

            // 解析返回值
            if (UnifyContext.CheckVaildResult(result, out var data))
            {
                returnValue = data;
            }
            // 处理文件类型
            else if (result is FileResult fresult)
            {
                returnValue = new
                {
                    FileName = fresult.FileDownloadName,
                    fresult.ContentType,
                    Length = fresult is FileContentResult cresult ? (object)cresult.FileContents.Length : null
                };
            }

            return returnValue != null ? JSON.Serialize(returnValue) : null;
        }
    }
}
