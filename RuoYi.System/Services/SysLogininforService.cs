using RuoYi.Common.Utils;
using RuoYi.System.Repositories;
using System.Text;
using UAParser.Interfaces;

namespace RuoYi.System.Services;

/// <summary>
///  系统访问记录 Service
///  author ruoyi
///  date   2023-08-22 10:07:36
/// </summary>
public class SysLogininforService : BaseService<SysLogininfor, SysLogininforDto>, ITransient
{
    private readonly ILogger<SysLogininforService> _logger;
    private readonly IUserAgentParser _userAgentParser;
    private readonly SysLogininforRepository _sysLogininforRepository;

    public SysLogininforService(ILogger<SysLogininforService> logger,
        IUserAgentParser userAgentParser,
        SysLogininforRepository sysLogininforRepository)
    {
        _logger = logger;
        _userAgentParser = userAgentParser;
        _sysLogininforRepository = sysLogininforRepository;
        BaseRepo = sysLogininforRepository;
    }

    /// <summary>
    /// 查询 系统访问记录 详情
    /// </summary>
    public async Task<SysLogininforDto> GetDtoAsync(long? id)
    {
        var entity = await base.FirstOrDefaultAsync(e => e.InfoId == id);
        var dto = entity.Adapt<SysLogininforDto>();
        // TODO 填充关联表数据
        return dto;
    }

    /// <summary>
    /// 记录登录信息
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="status">状态</param>
    /// <param name="message">消息</param>
    public async Task AddAsync(string username, string status, string message)
    {
        try
        {
            var clientInfo = this._userAgentParser.ClientInfo;

            string ip = App.HttpContext?.GetRemoteIpAddressToIPv4()!;
            string address = await AddressUtils.GetRealAddressByIPAsync(ip);

            StringBuilder s = new StringBuilder();
            s.Append(LogUtils.GetBlock(ip));
            s.Append(address);
            s.Append(LogUtils.GetBlock(username));
            s.Append(LogUtils.GetBlock(status));
            s.Append(LogUtils.GetBlock(message));
            // 打印信息到日志
            _logger.LogInformation(s.ToString());

            // 获取客户端操作系统
            string os = clientInfo?.OS?.Family?.ToString()!;
            // 获取客户端浏览器
            string browser = clientInfo?.Browser?.ToString()!;
            // 封装对象
            SysLogininfor logininfor = new SysLogininfor();
            logininfor.UserName = username;
            logininfor.Ipaddr = ip;
            logininfor.LoginLocation = address;
            logininfor.Browser = browser;
            logininfor.Os = os;
            logininfor.Msg = message;
            logininfor.LoginTime = DateTime.Now;

            // 日志状态
            var statuses = new List<string> { Constants.LOGIN_SUCCESS, Constants.LOGOUT, Constants.REGISTER };
            if (statuses.Contains(status))
            {
                logininfor.Status = Status.Enabled;
            }
            else if (Constants.LOGIN_FAIL.Equals(status))
            {
                logininfor.Status = Status.Disabled;
            }
            // 插入数据
            await _sysLogininforRepository.InsertAsync(logininfor);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "insert sysLogininfor error");
        }
    }
}