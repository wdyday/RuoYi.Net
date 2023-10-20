using RuoYi.Data;
using RuoYi.Data.Dtos;
using RuoYi.Data.Entities;

namespace RuoYi.System.Repositories
{
    /// <summary>
    ///  角色信息表 Repository
    ///  author ruoyi
    ///  date   2023-08-21 14:40:22
    /// </summary>
    public class SysRoleRepository : BaseRepository<SysRole, SysRoleDto>
    {
        public SysRoleRepository(ISqlSugarRepository<SysRole> sqlSugarRepository)
        {
            Repo = sqlSugarRepository;
        }

        public override ISugarQueryable<SysRole> Queryable(SysRoleDto dto)
        {
            return Repo.AsQueryable()
                .OrderBy((r) => r.RoleSort)
                .Where((r) => r.DelFlag == DelFlag.No)
                .WhereIF(dto.RoleId > 0, (r) => r.RoleId == dto.RoleId)
                .WhereIF(!string.IsNullOrEmpty(dto.RoleName), (r) => r.RoleName!.Contains(dto.RoleName!))
                .WhereIF(!string.IsNullOrEmpty(dto.RoleKey), (r) => r.RoleKey!.Contains(dto.RoleKey!))
                .WhereIF(!string.IsNullOrEmpty(dto.Status), (r) => r.Status == dto.Status)
                .WhereIF(dto.Params.BeginTime != null, (r) => r.CreateTime >= dto.Params.BeginTime)
                .WhereIF(dto.Params.EndTime != null, (r) => r.CreateTime <=  dto.Params.EndTime)
                .WhereIF(!string.IsNullOrEmpty(dto.Params.DataScopeSql), dto.Params.DataScopeSql)
            ;
        }

        public override ISugarQueryable<SysRoleDto> DtoQueryable(SysRoleDto dto)
        {
            return Repo.AsQueryable()
                .LeftJoin<SysUserRole>((r, ur) => r.RoleId == ur.RoleId)
                .LeftJoin<SysUser>((r, ur, u) => ur.UserId == u.UserId)
                .LeftJoin<SysDept>((r, ur, u, d) => u.DeptId == d.DeptId)
                .OrderBy((r) => r.RoleSort)
                .Where((r) => r.DelFlag == DelFlag.No)
                .WhereIF(dto.RoleId > 0, (r) => r.RoleId == dto.RoleId)
                .WhereIF(!string.IsNullOrEmpty(dto.RoleName), (r) => r.RoleName!.Contains(dto.RoleName!))
                .WhereIF(!string.IsNullOrEmpty(dto.RoleKey), (r) => r.RoleKey!.Contains(dto.RoleKey!))
                .WhereIF(!string.IsNullOrEmpty(dto.Status), (r) => r.Status == dto.Status)
                .WhereIF(dto.Params.BeginTime != null, (r) => r.CreateTime >= dto.Params.BeginTime)
                .WhereIF(dto.Params.EndTime != null, (r) => r.CreateTime <= dto.Params.EndTime)
                .WhereIF(!string.IsNullOrEmpty(dto.UserName), (r, ur, u) => u.UserName == dto.UserName)
                .WhereIF(!string.IsNullOrEmpty(dto.Params.DataScopeSql), dto.Params.DataScopeSql)
                .Select((r) => new SysRoleDto
                {
                    CreateBy = r.CreateBy,
                    CreateTime = r.CreateTime,
                    UpdateBy = r.UpdateBy,
                    UpdateTime = r.UpdateTime,

                    RoleId = r.RoleId,
                    RoleName = r.RoleName,
                    RoleKey = r.RoleKey,
                    RoleSort = r.RoleSort,
                    DataScope = r.DataScope,
                    MenuCheckStrictly = r.MenuCheckStrictly,
                    DeptCheckStrictly = r.DeptCheckStrictly,
                    Status = r.Status,
                    DelFlag = r.DelFlag,
                    Remark = r.Remark
                }).Distinct();
        }

        protected override async Task FillRelatedDataAsync(IEnumerable<SysRoleDto> dtos)
        {
            await base.FillRelatedDataAsync(dtos);

            foreach (var d in dtos)
            {
                d.StatusDesc = Status.ToDesc(d.Status);
                d.DataScopeDesc = DataScope.ToDesc(d.DataScope);
            }
        }

        public SysRole GetRoleById(long roleId)
        {
            return this.FirstOrDefault(r => r.RoleId == roleId);
        }

        public async Task<SysRole> GetByRoleNameAsync(string roleName)
        {
            var query = new SysRoleDto { RoleName = roleName };
            return await base.GetFirstAsync(query);
        }

        public async Task<SysRole> GetByRoleKeyAsync(string roleKey)
        {
            var query = new SysRoleDto { RoleKey = roleKey };
            return await base.GetFirstAsync(query);
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