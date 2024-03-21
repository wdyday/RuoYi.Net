using RuoYi.Common.Constants;
using RuoYi.Common.Utils;
using RuoYi.Data.Models;
using RuoYi.System.Repositories;

namespace RuoYi.System.Services;

/// <summary>
///  菜单权限表 Service
///  author ruoyi
///  date   2023-08-21 14:40:23
/// </summary>
public class SysMenuService : BaseService<SysMenu, SysMenuDto>, ITransient
{
    private readonly ILogger<SysMenuService> _logger;
    private readonly SysMenuRepository _sysMenuRepository;
    private readonly SysRoleRepository _sysRoleRepository;
    private readonly SysRoleMenuRepository _sysRoleMenuRepository;

    public SysMenuService(ILogger<SysMenuService> logger,
        SysMenuRepository sysMenuRepository,
        SysRoleRepository sysRoleRepository,
        SysRoleMenuRepository sysRoleMenuRepository)
    {
        BaseRepo = sysMenuRepository;
        _logger = logger;
        _sysMenuRepository = sysMenuRepository;
        _sysRoleRepository = sysRoleRepository;
        _sysRoleMenuRepository = sysRoleMenuRepository;
    }

    /// <summary>
    /// 根据用户查询系统菜单列表
    /// </summary>
    /// <param name="userId">用户ID</param>
    public async Task<List<SysMenu>> SelectMenuListAsync(long userId)
    {
        return await SelectMenuListAsync(new SysMenuDto(), userId);
    }

    /// <summary>
    /// 查询系统菜单列表
    /// </summary>
    public async Task<List<SysMenu>> SelectMenuListAsync(SysMenuDto menu, long userId)
    {
        List<SysMenu> menuList = null;
        // 管理员显示所有菜单信息
        if (SecurityUtils.IsAdmin(userId))
        {
            menuList = await _sysMenuRepository.SelectMenuListAsync(menu);
        }
        else
        {
            menu.UserId = userId;
            menuList = await _sysMenuRepository.SelectMenuListByUserIdAsync(menu);
        }
        return menuList;
    }

    /// <summary>
    /// 查询 菜单权限表 详情
    /// </summary>
    public async Task<SysMenu> GetAsync(long? id)
    {
        var entity = await base.FirstOrDefaultAsync(e => e.MenuId == id);
        //var dto = entity.Adapt<SysMenuDto>();
        return entity;
    }

    /// <summary>
    /// 根据用户ID查询权限
    /// </summary>
    public List<string> SelectMenuPermsByUserId(long userId)
    {
        List<string> perms = _sysMenuRepository.SelectMenuPermsByUserId(userId);
        List<string> permsSet = new List<string>();
        foreach (string perm in perms)
        {
            if (!string.IsNullOrEmpty(perm))
            {
                permsSet.AddRange(perm.Trim().Split(","));
            }
        }
        return permsSet;
    }

    /// <summary>
    /// 根据角色ID查询权限
    /// </summary>
    public List<string> SelectMenuPermsByRoleId(long roleId)
    {
        List<string> perms = _sysMenuRepository.SelectMenuPermsByRoleId(roleId);
        List<string> permsSet = new List<string>();
        foreach (string perm in perms)
        {
            if (!string.IsNullOrEmpty(perm))
            {
                permsSet.AddRange(perm.Trim().Split(","));
            }
        }
        return permsSet;
    }

    /// <summary>
    /// 根据用户ID查询菜单
    /// </summary>
    public List<SysMenu> SelectMenuTreeByUserId(long userId)
    {
        List<SysMenu> menus = null;
        if (SecurityUtils.IsAdmin(userId))
        {
            menus = _sysMenuRepository.SelectMenuTreeAll();
        }
        else
        {
            menus = _sysMenuRepository.SelectMenuTreeByUserId(userId);
        }
        return GetChildPerms(menus, 0);
    }

    /// <summary>
    /// 根据角色ID查询菜单树信息
    /// </summary>
    /// <param name="roleId">角色ID</param>
    /// <returns></returns>
    public List<long> SelectMenuListByRoleId(long roleId)
    {
        SysRole role = _sysRoleRepository.GetRoleById(roleId);
        return _sysMenuRepository.SelectMenuListByRoleId(roleId, role.MenuCheckStrictly);
    }

    /// <summary>
    /// 构建前端路由所需要的菜单
    /// </summary>
    public List<RouterVo> BuildMenus(List<SysMenu> menus)
    {
        List<RouterVo> routers = new List<RouterVo>();
        foreach (SysMenu menu in menus)
        {
            RouterVo router = new RouterVo();
            router.Hidden = Status.Disabled.Equals(menu.Visible);
            router.Name = GetRouteName(menu);
            router.Path = GetRouterPath(menu);
            router.Component = GetComponent(menu);
            router.Query = menu.Query;
            router.Meta = new MetaVo(menu.MenuName, menu.Icon, Status.Disabled.Equals(menu.IsCache), menu.Path);
            List<SysMenu> cMenus = menu.Children;
            if (cMenus != null && UserConstants.TYPE_DIR.Equals(menu.MenuType))
            {
                router.AlwaysShow = true;
                router.Redirect = "noRedirect";
                router.Children = BuildMenus(cMenus);
            }
            else if (IsMenuFrame(menu))
            {
                router.Meta = null;
                List<RouterVo> childrenList = new List<RouterVo>();
                RouterVo children = new RouterVo();
                children.Path = menu.Path;
                children.Component = menu.Component;
                children.Name = menu.Path.ToUpperCamelCase();
                children.Meta = new MetaVo(menu.MenuName, menu.Icon, Status.Disabled.Equals(menu.IsCache), menu.Path);
                children.Query = menu.Query;
                childrenList.Add(children);
                router.Children = childrenList;
            }
            else if (menu.ParentId == 0 && IsInnerLink(menu))
            {
                router.Meta = new MetaVo(menu.MenuName, menu.Icon);
                router.Path = "/";
                List<RouterVo> childrenList = new List<RouterVo>();
                RouterVo children = new RouterVo();
                string routerPath = InnerLinkReplaceEach(menu.Path);
                children.Path = routerPath;
                children.Component = UserConstants.INNER_LINK;
                children.Name = routerPath.ToUpperCamelCase();
                children.Meta = new MetaVo(menu.MenuName, menu.Icon, menu.Path);
                childrenList.Add(children);
                router.Children = childrenList;
            }
            routers.Add(router);
        }
        return routers;
    }


    /// <summary>
    /// 构建前端所需要树结构
    /// </summary>
    /// <param name="menus">菜单列表</param>
    public List<SysMenu> BuildMenuTree(List<SysMenu> menus)
    {
        List<SysMenu> returnList = new List<SysMenu>();
        List<long> tempList = menus.Select(m => m.MenuId).ToList();
        foreach (SysMenu menu in menus)
        {
            // 如果是顶级节点, 遍历该父节点的所有子节点
            if (!tempList.Contains(menu.ParentId))
            {
                RecursionFn(menus, menu);
                returnList.Add(menu);
            }
        }
        if (returnList.IsEmpty())
        {
            returnList = menus;
        }
        return returnList;
    }

    /// <summary>
    /// 构建前端所需要下拉树结构
    /// </summary>
    /// <param name="menus">菜单列表</param>
    public List<TreeSelect> BuildMenuTreeSelect(List<SysMenu> menus)
    {
        List<SysMenu> menuTrees = BuildMenuTree(menus);
        return menuTrees.Select(m => new TreeSelect(m)).ToList();
    }

    /// <summary>
    /// 根据菜单ID查询信息
    /// </summary>
    /// <param name="menuId">菜单ID</param>
    public SysMenu SelectMenuById(long menuId)
    {
        return _sysMenuRepository.SelectMenuById(menuId);
    }

    /// <summary>
    /// 是否存在菜单子节点
    /// </summary>
    /// <param name="menuId">菜单ID</param>
    public bool HasChildByMenuId(long menuId)
    {
        return _sysMenuRepository.HasChildByMenuId(menuId);
    }

    /// <summary>
    /// 查询菜单使用数量
    /// </summary>
    /// <param name="menuId">菜单ID</param>
    public bool CheckMenuExistRole(long menuId)
    {
        return _sysRoleMenuRepository.CheckMenuExistRole(menuId);
    }

    /// <summary>
    /// 校验菜单名称是否唯一
    /// </summary>
    public bool CheckMenuNameUnique(SysMenuDto menu)
    {
        long menuId = menu.MenuId;
        SysMenu info = _sysMenuRepository.CheckMenuNameUnique(menu.MenuName!, menu.ParentId);
        if (info != null && info.MenuId != menuId)
        {
            return UserConstants.NOT_UNIQUE;
        }
        return UserConstants.UNIQUE;
    }

    /// <summary>
    /// 获取路由名称
    /// </summary>
    public string GetRouteName(SysMenu menu)
    {
        string routerName = menu.Path.ToUpperCamelCase();
        // 非外链并且是一级目录（类型为目录）
        if (IsMenuFrame(menu))
        {
            routerName = string.Empty;
        }
        return routerName;
    }

    /// <summary>
    /// 获取路由地址
    /// </summary>
    public string GetRouterPath(SysMenu menu)
    {
        string routerPath = menu.Path;
        // 内链打开外网方式
        if (menu.ParentId != 0 && IsInnerLink(menu))
        {
            routerPath = InnerLinkReplaceEach(routerPath);
        }
        // 非外链并且是一级目录（类型为目录）
        if (0 == menu.ParentId && UserConstants.TYPE_DIR.Equals(menu.MenuType)
                && UserConstants.NO_FRAME.Equals(menu.IsFrame))
        {
            routerPath = "/" + menu.Path;
        }
        // 非外链并且是一级目录（类型为菜单）
        else if (IsMenuFrame(menu))
        {
            routerPath = "/";
        }
        return routerPath;
    }

    /// <summary>
    /// 获取组件信息
    /// </summary>
    public string GetComponent(SysMenu menu)
    {
        string component = UserConstants.LAYOUT;
        if (StringUtils.IsNotEmpty(menu.Component) && !IsMenuFrame(menu))
        {
            component = menu.Component;
        }
        else if (StringUtils.IsEmpty(menu.Component) && menu.ParentId != 0 && IsInnerLink(menu))
        {
            component = UserConstants.INNER_LINK;
        }
        else if (StringUtils.IsEmpty(menu.Component) && IsParentView(menu))
        {
            component = UserConstants.PARENT_VIEW;
        }
        return component;
    }

    /// <summary>
    /// 是否为菜单内部跳转
    /// </summary>
    public bool IsMenuFrame(SysMenu menu)
    {
        return menu.ParentId == 0 && UserConstants.TYPE_MENU.Equals(menu.MenuType)
                && menu.IsFrame.Equals(UserConstants.NO_FRAME);
    }

    /// <summary>
    /// 是否为内链组件
    /// </summary>
    public bool IsInnerLink(SysMenu menu)
    {
        return menu.IsFrame.Equals(UserConstants.NO_FRAME) && StringUtils.IsHttp(menu.Path);
    }

    /// <summary>
    /// 是否为parent_view组件
    /// </summary>
    public bool IsParentView(SysMenu menu)
    {
        return menu.ParentId != 0 && UserConstants.TYPE_DIR.Equals(menu.MenuType);
    }

    /// <summary>
    /// 根据父节点的ID获取所有子节点
    /// </summary>
    /// <param name="list">分类表</param>
    /// <param name="parentId">传入的父节点ID</param>
    /// <returns></returns>
    public List<SysMenu> GetChildPerms(List<SysMenu> list, int parentId)
    {
        List<SysMenu> returnList = new List<SysMenu>();
        foreach (SysMenu t in list)
        {
            // 一、根据传入的某个父节点ID,遍历该父节点的所有子节点
            if (t.ParentId == parentId)
            {
                RecursionFn(list, t);
                returnList.Add(t);
            }
        }
        return returnList;
    }

    /// <summary>
    /// 递归列表
    /// </summary>
    /// <param name="list">分类表</param>
    /// <param name="t">子节点</param>
    private void RecursionFn(List<SysMenu> list, SysMenu t)
    {
        // 得到子节点列表
        List<SysMenu> childList = GetChildList(list, t);
        t.Children = childList;
        foreach (SysMenu tChild in childList)
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
    private List<SysMenu> GetChildList(List<SysMenu> list, SysMenu t)
    {
        List<SysMenu> tList = new List<SysMenu>();
        foreach (SysMenu n in list)
        {
            if (n.ParentId == t.MenuId)
            {
                tList.Add(n);
            }
        }
        return tList;
    }

    /// <summary>
    /// 判断是否有子节点
    /// </summary>
    private bool HasChild(List<SysMenu> list, SysMenu t)
    {
        return GetChildList(list, t).Count > 0;
    }

    public string InnerLinkReplaceEach(string path)
    {
        var searchList = new string[] { Constants.HTTP, Constants.HTTPS, Constants.WWW, "." };
        var replacementList = new string[] { "", "", "", "/" };

        for (var i = 0; i < searchList.Length; i++)
        {
            path.Replace(searchList[i], replacementList[i]);
        }

        return path;
    }
}