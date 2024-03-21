using RuoYi.Common.Constants;
using RuoYi.Common.Interceptors;
using RuoYi.Common.Utils;
using RuoYi.Data.Models;
using RuoYi.Framework.Exceptions;
using RuoYi.System.Repositories;

namespace RuoYi.System.Services;

/// <summary>
///  部门表 Service
///  author ruoyi
///  date   2023-09-04 17:49:57
/// </summary>
public class SysDeptService : BaseService<SysDept, SysDeptDto>, ITransient
{
    private readonly ILogger<SysDeptService> _logger;
    private readonly SysDeptRepository _sysDeptRepository;
    private readonly SysUserRepository _sysUserRepository;
    private readonly SysRoleRepository _sysRoleRepository;

    public SysDeptService(ILogger<SysDeptService> logger,
        SysDeptRepository sysDeptRepository,
        SysUserRepository sysUserRepository,
        SysRoleRepository sysRoleRepository)
    {
        BaseRepo = sysDeptRepository;

        _logger = logger;
        _sysDeptRepository = sysDeptRepository;
        _sysUserRepository = sysUserRepository;
        _sysRoleRepository = sysRoleRepository;
    }

    /// <summary>
    /// 查询 部门表 详情
    /// </summary>
    public async Task<SysDept> GetAsync(long id)
    {
        return await base.FirstOrDefaultAsync(e => e.DeptId == id);
    }
    public async Task<SysDeptDto> GetDtoAsync(long id)
    {
        var entity = await GetAsync(id);
        return entity.Adapt<SysDeptDto>();
    }

    [DataScope(DeptAlias = "d")]
    public override async Task<List<SysDeptDto>> GetDtoListAsync(SysDeptDto dto)
    {
        return await _sysDeptRepository.GetDtoListAsync(dto);
    }

    /// <summary>
    /// 根据角色ID查询部门树信息
    /// </summary>
    /// <param name="roleId">角色ID</param>
    /// <returns>选中部门列表</returns>
    public async Task<List<long>> GetDeptListByRoleIdAsync(long roleId)
    {
        SysRole role = _sysRoleRepository.GetRoleById(roleId);

        return await _sysDeptRepository.GetDeptListByRoleIdAsync(roleId, role.DeptCheckStrictly);
    }

    /// <summary>
    /// 根据ID查询所有子部门（正常状态）数量
    /// </summary>
    /// <param name="deptId">部门ID</param>
    /// <returns>子部门数</returns>
    public async Task<int> CountNormalChildrenDeptByIdAsync(long deptId)
    {
        return await _sysDeptRepository.CountNormalChildrenDeptByIdAsync(deptId);
    }

    #region TreeSelect
    /// <summary>
    /// 查询部门树结构信息
    /// </summary>
    public async Task<List<TreeSelect>> GetDeptTreeListAsync(SysDeptDto dto)
    {
        List<SysDeptDto> depts = await this.GetDtoListAsync(dto);
        return BuildDeptTreeSelect(depts);
    }

    /// <summary>
    /// 构建前端所需要下拉树结构
    /// </summary>
    private List<TreeSelect> BuildDeptTreeSelect(List<SysDeptDto> depts)
    {
        List<SysDeptDto> deptTrees = BuildDeptTree(depts);
        return deptTrees.Select(dept => new TreeSelect(dept)).ToList();
    }

    /// <summary>
    /// 构建前端所需要树结构
    /// </summary>
    private List<SysDeptDto> BuildDeptTree(List<SysDeptDto> depts)
    {
        List<SysDeptDto> returnList = new List<SysDeptDto>();
        List<long> tempList = depts.Where(d => d.DeptId.HasValue).Select(d => d.DeptId!.Value).ToList();
        foreach (SysDeptDto dept in depts)
        {
            // 如果是顶级节点, 遍历该父节点的所有子节点
            if (dept.ParentId.HasValue && !tempList.Contains(dept.ParentId.Value))
            {
                RecursionFn(depts, dept);
                returnList.Add(dept);
            }
        }
        if (returnList.IsEmpty())
        {
            returnList = depts;
        }
        return returnList;
    }

    /// <summary>
    /// 递归列表
    /// </summary>
    private void RecursionFn(List<SysDeptDto> list, SysDeptDto t)
    {
        // 得到子节点列表
        List<SysDeptDto> childList = GetChildList(list, t);
        t.Children = childList;
        foreach (SysDeptDto tChild in childList)
        {
            if (HasChild(list, tChild))
            {
                RecursionFn(list, tChild);
            }
        }
    }

    /// <summary>
    /// 得到子节点列表
    /// </summary>
    private List<SysDeptDto> GetChildList(List<SysDeptDto> list, SysDeptDto t)
    {
        List<SysDeptDto> tList = new List<SysDeptDto>();
        foreach (SysDeptDto n in list)
        {
            if (n.ParentId > 0 && n.ParentId == t.DeptId)
            {
                tList.Add(n);
            }
        }
        return tList;
    }

    /// <summary>
    /// 是否存在子节点
    /// </summary>
    /// <param name="deptId">部门ID</param>
    /// <returns></returns>
    public async Task<bool> HasChildByDeptIdAsync(long deptId)
    {
        return await _sysDeptRepository.HasChildByDeptIdAsync(deptId);
    }

    /// <summary>
    /// 查询部门是否存在用户
    /// </summary>
    /// <param name="deptId">部门ID</param>
    /// <returns></returns>
    public async Task<bool> CheckDeptExistUserAsync(long deptId)
    {
        return await _sysUserRepository.CheckDeptExistUserAsync(deptId);
    }

    private bool HasChild(List<SysDeptDto> list, SysDeptDto t)
    {
        return GetChildList(list, t).Count > 0;
    }

    #endregion

    /// <summary>
    /// 校验部门名称是否唯一
    /// </summary>
    public async Task<bool> CheckDeptNameUniqueAsync(SysDeptDto dept)
    {
        SysDept info = await _sysDeptRepository.GetFirstAsync(new SysDeptDto { DeptName = dept.DeptName, ParentId = dept.ParentId });
        if (info != null && info.DeptId != dept.DeptId)
        {
            return UserConstants.NOT_UNIQUE;
        }
        return UserConstants.UNIQUE;
    }

    /// <summary>
    /// 校验部门是否有数据权限
    /// </summary>
    /// <param name="deptId">部门id</param>
    public async Task CheckDeptDataScopeAsync(long deptId)
    {
        if (!SecurityUtils.IsAdmin())
        {
            SysDeptDto dto = new SysDeptDto { DeptId = deptId };
            List<SysDept> depts = await _sysDeptRepository.GetDeptListAsync(dto);
            if (depts.IsEmpty())
            {
                throw new ServiceException("没有权限访问部门数据！");
            }
        }
    }

    /// <summary>
    /// 新增保存部门信息
    /// </summary>
    public async Task<bool> InsertDeptAsync(SysDeptDto dept)
    {
        SysDept info = await _sysDeptRepository.FirstOrDefaultAsync(d => d.DeptId == dept.ParentId); // 父节点
        // 如果父节点不为正常状态,则不允许新增子节点
        if (!UserConstants.DEPT_NORMAL.Equals(info.Status))
        {
            throw new ServiceException("部门停用，不允许新增");
        }
        dept.Ancestors = info.Ancestors + "," + dept.ParentId;
        dept.DelFlag = DelFlag.No;
        return await _sysDeptRepository.InsertAsync(dept);
    }

    /// <summary>
    /// 修改保存部门信息
    /// </summary>
    public async Task<int> UpdateDeptAsync(SysDeptDto dept)
    {
        SysDept newParentDept = await this.GetAsync(dept.ParentId.Value);
        SysDept oldDept = await this.GetAsync(dept.DeptId.Value);
        if (newParentDept != null && oldDept != null)
        {
            string newAncestors = newParentDept.Ancestors + "," + newParentDept.DeptId;
            string oldAncestors = oldDept.Ancestors!;
            dept.Ancestors = newAncestors;
            await UpdateDeptChildrenAsync(dept.DeptId.Value, newAncestors, oldAncestors);
        }
        int result = await _sysDeptRepository.UpdateAsync(dept, true);
        if (UserConstants.DEPT_NORMAL.Equals(dept.Status) && StringUtils.IsNotEmpty(dept.Ancestors)
                && !StringUtils.Equals("0", dept.Ancestors))
        {
            // 如果该部门是启用状态，则启用该部门的所有上级部门
            await UpdateParentDeptStatusNormalAsync(dept);
        }
        return result;
    }

    /// <summary>
    /// 修改子元素关系
    /// </summary>
    /// <param name="deptId">被修改的部门ID</param>
    /// <param name="newAncestors">新的父ID集合</param>
    /// <param name="oldAncestors">旧的父ID集合</param>
    public async Task UpdateDeptChildrenAsync(long deptId, string newAncestors, string oldAncestors)
    {
        List<SysDept> children = await _sysDeptRepository.GetChildrenDeptByIdAsync(deptId);
        foreach (SysDept child in children)
        {
            child.Ancestors = child.Ancestors!.ReplaceFirst(oldAncestors, newAncestors);
        }
        if (children.Count > 0)
        {
            await _sysDeptRepository.UpdateAsync(children);
        }
    }

    /// <summary>
    /// 修改该部门的父级部门状态
    /// </summary>
    /// <param name="dept">当前部门</param>
    private async Task UpdateParentDeptStatusNormalAsync(SysDeptDto dept)
    {
        string ancestors = dept.Ancestors!;
        long[] deptIds = ConvertUtils.ToLongArray(ancestors);
        await _sysDeptRepository.UpdateDeptStatusNormalAsync(deptIds);
    }

    /// <summary>
    /// 删除部门管理信息
    /// </summary>
    public async Task<int> DeleteDeptByIdAsync(long deptId)
    {
        return await _sysDeptRepository.DeleteDeptByIdAsync(deptId);
    }
}