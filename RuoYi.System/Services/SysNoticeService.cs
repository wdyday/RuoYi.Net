using RuoYi.System.Repositories;

namespace RuoYi.System.Services;

/// <summary>
///  通知公告表 Service
///  author ruoyi
///  date   2023-09-04 17:50:00
/// </summary>
public class SysNoticeService : BaseService<SysNotice, SysNoticeDto>, ITransient
{
    private readonly ILogger<SysNoticeService> _logger;
    private readonly SysNoticeRepository _sysNoticeRepository;

    public SysNoticeService(ILogger<SysNoticeService> logger,
        SysNoticeRepository sysNoticeRepository)
    {
        BaseRepo = sysNoticeRepository;

        _logger = logger;
        _sysNoticeRepository = sysNoticeRepository;
    }

    /// <summary>
    /// 查询 通知公告表 详情
    /// </summary>
    public async Task<SysNotice> GetAsync(int id)
    {
        var entity = await base.FirstOrDefaultAsync(e => e.NoticeId == id);
        return entity;
    }

    public async Task<SysNoticeDto> GetDtoAsync(int id)
    {
        var dto = new SysNoticeDto { NoticeId = id };
        return await _sysNoticeRepository.GetDtoFirstAsync(dto);
    }
}