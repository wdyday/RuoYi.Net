using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace RuoYi.Admin.Authorization
{
    public static class AuthorizationExtensions
    {
        public static AuthenticationBuilder AddRyJwt(this IServiceCollection services, bool enableGlobalAuthorize = false)
        {
            var builder = services.AddJwt<JwtHandler>(enableGlobalAuthorize: enableGlobalAuthorize);
            services.AddScoped<IAuthorizationMiddlewareResultHandler, AuthorizationMiddlewareResultHandler>();

            return builder;
        }
    }
}
