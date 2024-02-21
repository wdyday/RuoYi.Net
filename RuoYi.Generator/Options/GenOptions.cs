using RuoYi.Framework.ConfigurableOptions;
using Microsoft.Extensions.Configuration;

namespace RuoYi.Generator.Options
{
    [OptionsSettings("GenOptions")]
    public class GenOptions: IConfigurableOptions<GenOptions>
    {
        /** 作者 */
        public string Author { get; set; }

        /** 生成包路径 */
        public string PackageName { get; set; }

        /** 自动去除表前缀，默认是false */
        public bool AutoRemovePre { get; set; }

        /** 表前缀(类名不会包含表前缀) */
        public string TablePrefix { get; set; }

        // 默认值
        public void PostConfigure(GenOptions options, IConfiguration configuration)
        {
            options.Author ??= "ruoyi.net";
            options.PackageName ??= "RuoYi.System";
            options.AutoRemovePre = false;
            options.TablePrefix = "sys_";
        }

        // 热更新(实时的最新值)
        public void OnListener(GenOptions options, IConfiguration configuration)
        {
            this.Author = options.Author;
            this.PackageName = options.PackageName;
            this.AutoRemovePre = options.AutoRemovePre;
            this.TablePrefix = options.TablePrefix;
        }
    }
}
