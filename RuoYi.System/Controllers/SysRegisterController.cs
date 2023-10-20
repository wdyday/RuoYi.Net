using RuoYi.Common.Utils;
using RuoYi.Data.Models;
using RuoYi.Framework;
using RuoYi.System.Services;

namespace RuoYi.System.Controllers
{
    /// <summary>
    /// 注册验证
    /// </summary>
    [ApiDescriptionSettings("System")]
    public class SysRegisterController : ControllerBase
    {
        private readonly ILogger<SysRegisterController> _logger;
        private readonly SysRegisterService _sysRegisterService;
        private readonly SysConfigService _sysConfigService;

        public SysRegisterController(ILogger<SysRegisterController> logger,
            SysRegisterService sysRegisterService,
            SysConfigService sysConfigService)
        {
            _logger = logger;
            _sysRegisterService = sysRegisterService;
            _sysConfigService = sysConfigService;
        }

        /// <summary>
        /// 头像上传
        /// </summary>
        [HttpPost("register")]
        public async Task<AjaxResult> Register([FromBody] RegisterBody user)
        {
            if (!("true".Equals(_sysConfigService.SelectConfigByKey("sys.account.registerUser"))))
            {
                return AjaxResult.Error("当前系统没有开启注册功能！");
            }
            string msg = await _sysRegisterService.RegisterAsync(user);
            return StringUtils.IsEmpty(msg) ? AjaxResult.Success() : AjaxResult.Error(msg);
        }
    }
}