using RuoYi.Common.Interceptors;
using RuoYi.Common.Utils;
using RuoYi.Framework.DataValidation;
using RuoYi.Framework.Exceptions;
using RuoYi.Framework.Interceptors;
using RuoYi.System.Repositories;
using System.Text;

namespace RuoYi.System.Services;

/// <summary>
///  用户信息表 Service
///  author ruoyi
///  date   2023-08-21 14:40:20
/// </summary>
public class SysUserService : BaseService<SysUser, SysUserDto>, ITransient
{
    private readonly ILogger<SysUserService> _logger;
    private readonly SysUserRepository _sysUserRepository;
    private readonly SysUserPostRepository _sysUserPostRepository;
    private readonly SysUserRoleRepository _sysUserRoleRepository;

    private readonly SysRoleService _sysRoleService;
    private readonly SysPostService _sysPostService;
    private readonly SysConfigService _sysConfigService;

    public SysUserService(ILogger<SysUserService> logger,
        SysUserRepository sysUserRepository,
        SysUserPostRepository sysUserPostRepository,
        SysUserRoleRepository sysUserRoleRepository,
        SysRoleService sysRoleService,
        SysPostService sysPostService,
        SysConfigService sysConfigService)
    {
        BaseRepo = sysUserRepository;

        _logger = logger;
        _sysUserRepository = sysUserRepository;
        _sysUserPostRepository = sysUserPostRepository;
        _sysUserRoleRepository = sysUserRoleRepository;

        _sysRoleService = sysRoleService;
        _sysPostService = sysPostService;
        _sysConfigService = sysConfigService;
    }

    /// <summary>
    /// 查询 用户信息表 详情
    /// </summary>
    public async Task<SysUserDto> GetDtoAsync(long? id)
    {
        var dto = new SysUserDto { UserId = id ?? 0 };
        var user = await _sysUserRepository.GetUserDtoAsync(dto);
        return user;
    }

    /// <summary>
    /// 查询 用户
    /// </summary>
    /// <param name="username">用户名</param>
    /// <returns></returns>
    public async Task<SysUserDto> GetDtoByUsernameAsync(string username)
    {
        var dto = new SysUserDto { UserName = username, DelFlag = DelFlag.No };
        var user = await _sysUserRepository.GetUserDtoAsync(dto);
        return user;
    }

    /// <summary>
    /// 查询 用户
    /// </summary>
    /// <param name="username">用户名</param>
    /// <returns></returns>
    public async Task<SysUser> GetByUsernameAsync(string username)
    {
        return await _sysUserRepository.GetUserByUserNameAsync(username);
    }

    /// <summary>
    /// 查询 用户
    /// </summary>
    /// <param name="phoneNumber">手机号</param>
    /// <returns></returns>
    public async Task<SysUserDto> GetByPhoneAsync(string phoneNumber)
    {
        var dto = new SysUserDto { Phonenumber = phoneNumber, DelFlag = DelFlag.No };
        var user = await _sysUserRepository.GetUserDtoAsync(dto);
        return user;
    }

    /// <summary>
    /// 查询 用户
    /// </summary>
    /// <param name="email">email</param>
    /// <returns></returns>
    public async Task<SysUserDto> GetByEmailAsync(string email)
    {
        var dto = new SysUserDto { Email = email, DelFlag = DelFlag.No };
        var user = await _sysUserRepository.GetUserDtoAsync(dto);
        return user;
    }

    /// <summary>
    /// 查询用户列表
    /// </summary>
    [DataScope(DeptAlias = "d", UserAlias = "u")]
    public virtual async Task<List<SysUser>> GetUserListAsync(SysUserDto dto)
    {
        return await _sysUserRepository.GetListAsync(dto);
    }

    // entity 转换成 dto
    public List<SysUserDto> ToDtos(List<SysUser> entities)
    {
        var dtos = entities.Adapt<List<SysUserDto>>();
        foreach (var d in dtos)
        {
            d.DeptName = d.Dept?.DeptName;
            d.DeptLeader = d.Dept?.Leader;

            d.SexDesc = Sex.ToDesc(d.Sex);
            d.StatusDesc = Status.ToDesc(d.Status);
        }
        return dtos;
    }

    /// <summary>
    /// 分页查询用户列表
    /// </summary>
    [DataScope(DeptAlias = "d", UserAlias = "u")]
    public virtual async Task<SqlSugarPagedList<SysUser>> GetPagedUserListAsync(SysUserDto dto)
    {
        return await _sysUserRepository.GetPagedListAsync(dto);
    }

    /// <summary>
    /// 根据条件分页查询已分配用户角色列表
    /// </summary>
    [DataScope(DeptAlias = "d", UserAlias = "u")]
    public virtual async Task<SqlSugarPagedList<SysUserDto>> GetPagedAllocatedListAsync(SysUserDto dto)
    {
        dto.IsAllocated = true;
        return await _sysUserRepository.GetDtoPagedListAsync(dto);
    }

    /// <summary>
    /// 根据条件分页查询未分配用户角色列表
    /// </summary>
    [DataScope(DeptAlias = "d", UserAlias = "u")]
    public virtual async Task<SqlSugarPagedList<SysUserDto>> GetPagedUnallocatedListAsync(SysUserDto dto)
    {
        dto.IsAllocated = false;
        return await _sysUserRepository.GetDtoPagedListAsync(dto);
    }

    /// <summary>
    /// 查询用户所属角色组
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <returns></returns>
    public string SelectUserRoleGroup(string userName)
    {
        List<SysRoleDto> list = _sysRoleService.GetRolesByUserName(userName);
        if (!list.IsNotEmpty())
        {
            return string.Empty;
        }
        return string.Join(",", list.Select(r => r.RoleName).ToList());
    }

    /// <summary>
    /// 查询用户所属岗位组
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <returns></returns>
    public string SelectUserPostGroup(string userName)
    {
        List<SysPostDto> list = _sysPostService.GetPostsByUserName(userName);
        if (!list.IsNotEmpty())
        {
            return string.Empty;
        }
        return string.Join(",", list.Select(r => r.PostName).ToList());
    }

    /// <summary>
    /// 校验用户名称是否唯一
    /// </summary>
    /// <param name="user">用户信息</param>
    /// <returns></returns>
    public async Task<bool> CheckUserNameUniqueAsync(SysUserDto user)
    {
        var userDto = await this.GetByUsernameAsync(user.UserName!);
        if (userDto != null && userDto.UserId != user.UserId)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 校验手机号码是否唯一
    /// </summary>
    /// <param name="user">用户信息</param>
    /// <returns></returns>
    public async Task<bool> CheckPhoneUniqueAsync(SysUserDto user)
    {
        var userDto = await this.GetByPhoneAsync(user.Phonenumber!);
        if (userDto != null && userDto.UserId != user.UserId)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 校验email是否唯一
    /// </summary>
    /// <param name="user">用户信息</param>
    /// <returns></returns>
    public async Task<bool> CheckEmailUniqueAsync(SysUserDto user)
    {
        var userDto = await this.GetByEmailAsync(user.Email!);
        if (userDto != null && userDto.UserId != user.UserId)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 校验用户是否允许操作
    /// </summary>
    /// <param name="user"></param>
    public void CheckUserAllowed(SysUserDto user)
    {
        if (user.UserId > 0 && SecurityUtils.IsAdmin(user.UserId))
        {
            throw new ServiceException("不允许操作超级管理员用户");
        }
    }

    /// <summary>
    /// 校验用户是否有数据权限
    /// </summary>
    /// <param name="userId">用户id</param>
    public async Task CheckUserDataScope(long? userId)
    {
        if (!SecurityUtils.IsAdmin(SecurityUtils.GetUserId()) && userId.HasValue)
        {
            SysUserDto dto = new SysUserDto { UserId = userId };
            List<SysUser> users = await this.GetUserListAsync(dto);
            if (users.IsEmpty())
            {
                throw new ServiceException("没有权限访问用户数据！");
            }
        }
    }

    /// <summary>
    /// 新增保存用户信息
    /// </summary>
    /// <param name="user">用户信息</param>
    /// <returns></returns>
    [Transactional]
    public virtual bool InsertUser(SysUserDto user)
    {
        // 新增用户信息
        user.Password = SecurityUtils.EncryptPassword(user.Password!);
        user.DelFlag = DelFlag.No;
        bool succees = _sysUserRepository.Insert(user);
        // 新增用户岗位关联
        InsertUserPost(user);
        // 新增用户与角色管理
        InsertUserRole(user);
        return succees;
    }

    /// <summary>
    /// 注册用户信息
    /// </summary>
    public async Task<bool> RegisterUserAsync(SysUserDto user)
    {
        return await _sysUserRepository.InsertAsync(user);
    }

    /// <summary>
    /// 修改保存用户信息
    /// </summary>
    [Transactional]
    public virtual int UpdateUser(SysUserDto user)
    {
        // 删除用户与角色关联
        _sysUserRoleRepository.DeleteUserRoleByUserId(user.UserId ?? 0);
        // 新增用户与角色管理
        InsertUserRole(user);
        // 删除用户与岗位关联
        _sysUserPostRepository.DeleteUserPostByUserId(user.UserId ?? 0);
        // 新增用户与岗位管理
        InsertUserPost(user);

        return _sysUserRepository.UpdateUser(user);
    }

    /// <summary>
    /// 用户授权角色
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="roleIds">角色组</param>
    public void InsertUserAuth(long userId, List<long> roleIds)
    {
        _sysUserRoleRepository.DeleteUserRoleByUserId(userId);
        InsertUserRole(userId, roleIds);
    }

    /// <summary>
    /// 修改用户状态
    /// </summary>
    public async Task<int> UpdateUserStatus(SysUserDto user)
    {
        return await _sysUserRepository.UpdateUserStatusAsync(user);
    }

    /// <summary>
    /// 修改用户基本信息
    /// </summary>
    public async Task<int> UpdateUserProfileAsync(SysUserDto user)
    {
        return await _sysUserRepository.UpdateAsync(user, true);
    }

    /// <summary>
    /// 更新登录信息: 登录IP/登录时间
    /// </summary>
    public async Task<int> UpdateUserLoginInfoAsync(SysUserDto user)
    {
        return await _sysUserRepository.UpdateUserLoginInfoAsync(user);
    }


    /// <summary>
    /// 新增用户角色信息
    /// </summary>
    /// <param name="user"></param>
    public void InsertUserRole(SysUserDto user)
    {
        this.InsertUserRole(user.UserId ?? 0, user.RoleIds);
    }

    /// <summary>
    /// 修改用户头像
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <param name="avatar">头像地址</param>
    /// <returns></returns>
    public async Task<bool> UpdateUserAvatar(string userName, string avatar)
    {
        return await _sysUserRepository.UpdateUserAvatarAsync(userName, avatar) > 0;
    }

    /// <summary>
    /// 重置用户密码
    /// </summary>
    public int ResetPwd(SysUserDto user)
    {
        user.Password = SecurityUtils.EncryptPassword(user.Password!);
        return _sysUserRepository.Update(user, true);
    }

    /// <summary>
    /// 重置用户密码
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <param name="password">加密的密码</param>
    public async Task<int> ResetUserPwdAsync(string userName, string password)
    {
        return await _sysUserRepository.ResetPasswordAsync(userName, password);
    }

    /// <summary>
    /// 新增用户角色信息
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="roleIds">角色组</param>
    public void InsertUserRole(long userId, List<long> roleIds)
    {
        if (roleIds.IsNotEmpty())
        {
            // 新增用户与角色管理
            List<SysUserRole> list = new List<SysUserRole>();
            foreach (long roleId in roleIds)
            {
                SysUserRole ur = new SysUserRole();
                ur.UserId = userId;
                ur.RoleId = roleId;
                list.Add(ur);
            }
            _sysUserRoleRepository.Insert(list);
        }
    }

    /// <summary>
    /// 新增用户岗位信息
    /// </summary>
    /// <param name="user"></param>
    public void InsertUserPost(SysUserDto user)
    {
        if (user.PostIds.IsNotEmpty())
        {
            // 新增用户与岗位管理
            List<SysUserPost> list = new List<SysUserPost>();
            foreach (long postId in user.PostIds)
            {
                SysUserPost up = new SysUserPost();
                up.UserId = user.UserId ?? 0;
                up.PostId = postId;
                list.Add(up);
            }
            _sysUserPostRepository.Insert(list);
        }
    }

    /// <summary>
    /// 通过用户ID删除用户
    /// </summary>
    /// <param name="userId">用户ID</param>
    [Transactional]
    public virtual int DeleteUserByIdAsync(long userId)
    {
        // 删除用户与角色关联
        _sysUserRoleRepository.DeleteUserRoleByUserId(userId);
        // 删除用户与岗位表
        _sysUserPostRepository.DeleteUserPostByUserId(userId);
        return _sysUserRepository.DeleteUserById(userId);
    }

    /// <summary>
    /// 批量删除用户信息
    /// </summary>
    [Transactional]
    public virtual async Task<int> DeleteUserByIdsAsync(List<long> userIds)
    {
        foreach (long userId in userIds)
        {
            CheckUserAllowed(new SysUserDto { UserId = userId });
            await CheckUserDataScope(userId);
        }
        // 删除用户与角色关联
        _sysUserRoleRepository.DeleteUserRole(userIds);
        // 删除用户与岗位关联
        _sysUserPostRepository.DeleteUserPost(userIds);
        return _sysUserRepository.DeleteUserByIds(userIds);
    }

    /// <summary>
    /// 导入
    /// </summary>
    /// <param name="dtos"></param>
    /// <returns></returns>
    public async Task<string> ImportDtosAsync(List<SysUserDto> dtos, bool isUpdateSupport, string operName)
    {
        if (dtos.IsEmpty())
        {
            throw new ServiceException("导入用户数据不能为空！");
        }
        int successNum = 0;
        int failureNum = 0;
        StringBuilder successMsg = new StringBuilder();
        StringBuilder failureMsg = new StringBuilder();
        string password = _sysConfigService.SelectConfigByKey("sys.user.initPassword");
        foreach (SysUserDto user in dtos)
        {
            user.Sex = Sex.ToVal(user.SexDesc);
            user.Status = Status.ToVal(user.StatusDesc);

            try
            {
                // 验证是否存在这个用户
                SysUserDto u = await _sysUserRepository.GetUserDtoByUserNameAsync(user.UserName!);
                if (u == null)
                {
                    user.Validate();

                    user.Password = SecurityUtils.EncryptPassword(password);
                    user.CreateBy = operName;
                    user.DelFlag = DelFlag.No;
                    await _sysUserRepository.InsertAsync(user);

                    successNum++;
                    successMsg.Append("<br/>" + successNum + "、账号 " + user.UserName + " 导入成功");
                }
                else if (isUpdateSupport)
                {
                    user.Validate();

                    CheckUserAllowed(u);
                    await CheckUserDataScope(u.UserId ?? 0);

                    user.UserId = u.UserId;
                    user.UpdateBy = operName;
                    await _sysUserRepository.UpdateAsync(user, true);

                    successNum++;
                    successMsg.Append("<br/>" + successNum + "、账号 " + user.UserName + " 更新成功");
                }
                else
                {
                    failureNum++;
                    failureMsg.Append("<br/>" + failureNum + "、账号 " + user.UserName + " 已存在");
                }
            }
            catch (Exception e)
            {
                failureNum++;
                string msg = "<br/>" + failureNum + "、账号 " + user.UserName + " 导入失败：";
                failureMsg.Append(msg + e.Message);
                _logger.LogError(e, msg);
            }
        }
        if (failureNum > 0)
        {
            failureMsg.Insert(0, "很抱歉，导入失败！共 " + failureNum + " 条数据格式不正确，错误如下：");
            throw new ServiceException(failureMsg.ToString());
        }
        else
        {
            successMsg.Insert(0, "恭喜您，数据已全部导入成功！共 " + successNum + " 条，数据如下：");
        }

        return successMsg.ToString();
    }
}