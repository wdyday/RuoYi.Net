using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using RuoYi.Common.Constants;

namespace RuoYi.Common.Files
{
    public static class FileExtensions
    {
        public static void UseRyStaticFiles(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            var staticPath = Path.Combine(env.ContentRootPath, AppConstants.StaticFileFolder);
            FileUploadUtils.CreateDirectory(staticPath);

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(staticPath),
                OnPrepareResponse = (stf) =>
                {
                    stf.Context.Response.Headers["Access-Control-Allow-Origin"] = "*";
                    stf.Context.Response.Headers["Access-Control-Allow-Headers"] = "*";
                }
            });
        }
    }
}
