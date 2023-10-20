using Microsoft.AspNetCore.Http;

namespace RuoYi.Framework.Exceptions
{
    /// <summary>
    /// 业务异常
    /// </summary>
    public class ServiceException : Exception
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public int? Code { get; set; }

        /// <summary>
        /// 错误提示
        /// </summary>
        public string? ErrorMessage { get; set; }

        public ServiceException() : base()
        {
        }

        public ServiceException(string? message) : base(message)
        {
            Code = StatusCodes.Status500InternalServerError;
            ErrorMessage = message;
        }

        public ServiceException(string? message, int? code) : base(message)
        {
            Code = code;
            ErrorMessage = message;
        }
    }
}
