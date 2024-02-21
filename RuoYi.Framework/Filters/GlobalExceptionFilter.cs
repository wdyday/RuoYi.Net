using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using RuoYi.Framework.Exceptions;
using RuoYi.Framework.Extensions;

namespace RuoYi.Framework.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(IWebHostEnvironment env, ILogger<GlobalExceptionFilter> logger)
        {
            _env = env;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "GlobalException");

            context.ExceptionHandled = true;

            if (context.Exception is ServiceException)
            {
                var result = GetServiceExcepitionResult(context);
                context.Result = new JsonResult(result);
            }
            else
            {
                string msg;
                if (_env.EnvironmentName.EqualsIgnoreCase("Development"))
                {
                    msg = context.Exception.Message + Environment.NewLine + context.Exception.InnerException?.Message + Environment.NewLine + context.Exception.StackTrace;
                }
                else
                {
                    msg = "系统错误";
                }
                var result = AjaxResult.Error(msg);
                context.Result = new JsonResult(result);
            }
        }

        private AjaxResult GetServiceExcepitionResult(ExceptionContext context)
        {
            var serviceExcepion = context.Exception as ServiceException;
            return serviceExcepion!.Code.HasValue 
                ? AjaxResult.Error(serviceExcepion!.Code.Value, serviceExcepion!.ErrorMessage?.ToString() ?? "")
                : AjaxResult.Error(serviceExcepion!.ErrorMessage?.ToString() ?? "");
        }
    }
}
