using RuoYi.Framework.Authorization;

namespace RuoYi.Admin.Authorization
{
    /// <summary>
    /// JWT 授权自定义处理程序
    /// </summary>
    public class JwtHandler : AppAuthorizeHandler
    {
        /// <summary>
        /// 此处不做校验, 在 AuthorizationMiddlewareResultHandler 校验, 返回自定义code
        /// </summary>
        public override async Task HandleAsync(AuthorizationHandlerContext context)
        {
            await Task.FromResult(true);
        }
    }
}
