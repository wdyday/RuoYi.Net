using SqlSugar;

namespace RuoYi.Data.Entities
{
    [SugarTable("sys_role", "角色表")]
    public class SysRole : UserBaseEntity
    {
        /** 角色ID */
        [SugarColumn(ColumnName = "role_id", ColumnDescription = "角色ID", IsPrimaryKey = true, IsIdentity = true)]
        public long RoleId { get; set; }

        /** 角色名称 */
        [SugarColumn(ColumnName = "role_name", ColumnDescription = "角色名称")]
        public string? RoleName { get; set; }

        /** 角色权限(admin/common) */
        [SugarColumn(ColumnName = "role_key", ColumnDescription = "角色权限")]
        public string? RoleKey { get; set; }

        /** 角色排序 */
        [SugarColumn(ColumnName = "role_sort", ColumnDescription = "角色排序")]
        public int RoleSort { get; set; }

        /** 数据范围（1：所有数据权限；2：自定义数据权限；3：本部门数据权限；4：本部门及以下数据权限；5：仅本人数据权限） */
        [SugarColumn(ColumnName = "data_scope", ColumnDescription = "数据范围（1：所有数据权限；2：自定义数据权限；3：本部门数据权限；4：本部门及以下数据权限；5：仅本人数据权限）")]
        public string? DataScope { get; set; }

        /** 菜单树选择项是否关联显示（ 0：父子不互相关联显示 1：父子互相关联显示） */
        [SugarColumn(ColumnName = "menu_check_strictly", ColumnDescription = "菜单树选择项是否关联显示（ 0：父子不互相关联显示 1：父子互相关联显示）")]
        public bool MenuCheckStrictly { get; set; }

        /** 部门树选择项是否关联显示（0：父子不互相关联显示 1：父子互相关联显示 ） */
        [SugarColumn(ColumnName = "dept_check_strictly", ColumnDescription = "部门树选择项是否关联显示（0：父子不互相关联显示 1：父子互相关联显示 ）")]
        public bool DeptCheckStrictly { get; set; }

        /** 角色状态（0正常 1停用） */
        [SugarColumn(ColumnName = "status", ColumnDescription = "角色状态（0正常 1停用）")]
        public string? Status { get; set; }

        /** 删除标志（0代表存在 2代表删除） */
        [SugarColumn(ColumnName = "del_flag", ColumnDescription = "删除标志（0代表存在 2代表删除）")]
        public string? DelFlag { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注")]
        public string? Remark { get; set; }

        /** 用户是否存在此角色标识 默认不存在 */
        //public bool flag = false{ get; set; }

        /** 菜单组 */
        //public long[] menuIds{ get; set; }

        /** 部门组（数据权限） */
        //public long[] deptIds{ get; set; }

        /** 角色菜单权限 */
        //public Set<string?> permissions{ get; set; }

    }
}
