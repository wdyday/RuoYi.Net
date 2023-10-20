using RuoYi.Data;
using RuoYi.Data.Dtos;
using RuoYi.Data.Entities;

namespace RuoYi.System.Repositories
{
    /// <summary>
    ///  角色和菜单关联表 Repository
    ///  author ruoyi.net
    ///  date   2023-08-23 09:43:53
    /// </summary>
    public class SysRoleMenuRepository : BaseRepository<SysRoleMenu, SysRoleMenuDto>
    {
        public SysRoleMenuRepository(ISqlSugarRepository<SysRoleMenu> sqlSugarRepository)
        {
            Repo = sqlSugarRepository;
        }

        public override ISugarQueryable<SysRoleMenu> Queryable(SysRoleMenuDto dto)
        {
            return Repo.AsQueryable()
                .WhereIF(dto.MenuId > 0, (t) => t.MenuId == dto.MenuId)
                .WhereIF(dto.RoleId > 0, (t) => t.RoleId == dto.RoleId)
            ;
        }

        public override ISugarQueryable<SysRoleMenuDto> DtoQueryable(SysRoleMenuDto dto)
        {
            return Repo.AsQueryable()
                .WhereIF(dto.MenuId > 0, (t) => t.MenuId == dto.MenuId)
                .WhereIF(dto.RoleId > 0, (t) => t.RoleId == dto.RoleId)
                .Select((t) => new SysRoleMenuDto
                {
                    MenuId = t.MenuId,
                    RoleId = t.RoleId
                });
        }

        /// <summary>
        /// 查询菜单使用数量
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <returns></returns>
        public bool CheckMenuExistRole(long menuId)
        {
            return base.Count(r => r.MenuId == menuId) > 0;
        }

        /// <summary>
        /// 按角色ID删除
        /// </summary>
        /// <param name="roleId">角色ID</param>
        public async Task<int> DeleteByRoleIdAsync(long roleId)
        {
            return await base.DeleteAsync(m => m.RoleId == roleId);
        }

        /// <summary>
        /// 按角色ID删除
        /// </summary>
        public async Task<int> DeleteByRoleIdsAsync(List<long> roleIds)
        {
            return await base.DeleteAsync(m => roleIds.Contains(m.RoleId));
        }
    }
}