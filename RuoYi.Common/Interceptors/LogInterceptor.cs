//using AspectCore.DynamicProxy;
//using Furion.Logging;
//using RuoYi.Common.Enums;

//namespace RuoYi.Common.Interceptors
//{
//    public class LogAttribute : AbstractInterceptorAttribute
//    {
//        /// <summary>
//        /// 模块
//        /// </summary>
//        public string Title { get; set; } = "";

//        /// <summary>
//        /// 功能
//        /// </summary>
//        public BusinessType BusinessType { get; set; } = BusinessType.OTHER;

//        /// <summary>
//        /// 操作人类别
//        /// </summary>
//        public OperatorType OperatorType { get; set; } = OperatorType.MANAGE;

//        /// <summary>
//        /// 是否保存请求的参数
//        /// </summary>
//        public bool IsSaveRequestData { get; set; } = true;

//        /// <summary>
//        /// 是否保存响应的参数
//        /// </summary>
//        public bool IsSaveResponseData { get; set; } = true;

//        /// <summary>
//        /// 排除指定的请求参数
//        /// </summary>
//        public string[] ExcludeParamNames { get; set; } = new string[0];


//        /** 排除敏感属性字段 */
//        public static string[] EXCLUDE_PROPERTIES = { "password", "oldPassword", "newPassword", "confirmPassword" };

//        public override async Task Invoke(AspectContext context, AspectDelegate next)
//        {
//            try
//            {
//                HandleLog();
//            }
//            catch (Exception ex)
//            {
//                Log.Error("LogInterceptor Error", ex);
//            }

//            await next(context);
//        }

//        private void HandleLog()
//        {

//        }
//    }
//}
