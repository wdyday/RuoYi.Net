using RuoYi.Data;
using RuoYi.Data.Slave.Dtos;
using RuoYi.Data.Slave.Entities;
using RuoYi.System.Slave.Repositories;

namespace RuoYi.System.Slave.Services
{
    /// <summary>
    ///  用户信息表 Service
    ///  author ruoyi
    ///  date   2023-08-21 14:40:20
    /// </summary>
    public class SysUserService : BaseService<SysUser, SysUserDto>, ITransient
    {
        private readonly ILogger<SysUserService> _logger;
        private readonly SysUserRepository _sysUserRepository;

        public SysUserService(ILogger<SysUserService> logger,
            SysUserRepository sysUserRepository)
        {
            _logger = logger;
            _sysUserRepository = sysUserRepository;
            BaseRepo = sysUserRepository;
        }

        /// <summary>
        /// 查询 用户信息表 详情
        /// </summary>
        public async Task<SysUserDto> GetAsync(long? id)
        {
            var entity = await base.FirstOrDefaultAsync(e => e.UserId == id);
            var dto = entity.Adapt<SysUserDto>();
            // TODO 填充关联表数据
            return dto;
        }

        /// <summary>
        /// 查询 用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public async Task<SysUser> GetByUsernameAsync(string username)
        {
            return await base.FirstOrDefaultAsync(e => e.UserName == username);
        }
    }
}