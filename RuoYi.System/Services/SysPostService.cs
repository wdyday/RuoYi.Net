using RuoYi.Common.Constants;
using RuoYi.Framework.Exceptions;
using RuoYi.System.Repositories;

namespace RuoYi.System.Services;

/// <summary>
///  岗位信息表 Service
///  author ruoyi
///  date   2023-08-30 13:21:36
/// </summary>
public class SysPostService : BaseService<SysPost, SysPostDto>, ITransient
{
    private readonly ILogger<SysPostService> _logger;
    private readonly SysPostRepository _sysPostRepository;
    private readonly SysUserPostRepository _sysUserPostRepository;

    public SysPostService(ILogger<SysPostService> logger,
        SysPostRepository sysPostRepository,
        SysUserPostRepository sysUserPostRepository)
    {
        BaseRepo = sysPostRepository;

        _logger = logger;
        _sysPostRepository = sysPostRepository;
        _sysUserPostRepository = sysUserPostRepository;
    }

    /// <summary>
    /// 查询所有岗位
    /// </summary>
    public async Task<List<SysPost>> SelectPostAllAsync()
    {
        return await base.GetListAsync(new SysPostDto());
    }

    /// <summary>
    /// 查询 岗位信息表 详情
    /// </summary>
    public async Task<SysPost> GetAsync(long? id)
    {
        return await base.FirstOrDefaultAsync(e => e.PostId == id);
    }

    /// <summary>
    /// 查询 岗位信息表 详情
    /// </summary>
    public async Task<SysPostDto> GetDtoAsync(long? id)
    {
        var entity = await base.FirstOrDefaultAsync(e => e.PostId == id);
        var dto = entity.Adapt<SysPostDto>();
        // TODO 填充关联表数据
        return dto;
    }

    /// <summary>
    /// 查询用户所属岗位组
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <returns></returns>
    public List<SysPostDto> GetPostsByUserName(string userName)
    {
        var dto = new SysPostDto { UserName = userName };
        return this.GetDtoList(dto);
    }

    public List<SysPostDto> GetPostsListByUserId(long userId)
    {
        var dto = new SysPostDto { UserId = userId };
        return this.GetDtoList(dto);
    }

    public List<long> GetPostIdsListByUserId(long userId)
    {
        var dtos = this.GetPostsListByUserId(userId);
        return dtos.Where(p => p.PostId.HasValue).Select(p => p.PostId!.Value).Distinct().ToList();
    }

    /// <summary>
    /// 校验岗位名称是否唯一
    /// </summary>
    public async Task<bool> CheckPostNameUniqueAsync(SysPostDto post)
    {
        long postId = post.PostId ?? 0;
        SysPost info = await _sysPostRepository.CheckPostNameUniqueAsync(post.PostName);
        if (info != null && info.PostId != postId)
        {
            return UserConstants.NOT_UNIQUE;
        }
        return UserConstants.UNIQUE;
    }

    /// <summary>
    /// 校验岗位编码是否唯一
    /// </summary>
    public async Task<bool> CheckPostCodeUniqueAsync(SysPostDto post)
    {
        long postId = post.PostId ?? 0;
        SysPost info = await _sysPostRepository.CheckPostCodeUniqueAsync(post.PostCode);
        if (info != null && info.PostId != postId)
        {
            return UserConstants.NOT_UNIQUE;
        }
        return UserConstants.UNIQUE;
    }

    /// <summary>
    /// 批量删除岗位信息
    /// </summary>
    /// <param name="postIds">需要删除的岗位ID</param>
    public async Task<int> DeletePostByIdsAsync(long[] postIds)
    {
        foreach (long postId in postIds)
        {
            SysPost post = await this.GetAsync(postId);
            if (await CountUserPostByIdAsync(postId) > 0)
            {
                throw new ServiceException($"{post.PostName}已分配,不能删除");
            }
        }
        return await _sysPostRepository.DeleteAsync(postIds);
    }

    /// <summary>
    /// 通过岗位ID查询岗位使用数量
    /// </summary>
    /// <param name="postId">岗位ID</param>
    public async Task<int> CountUserPostByIdAsync(long postId)
    {
        return await _sysUserPostRepository.CountUserPostByIdAsync(postId);
    }
}