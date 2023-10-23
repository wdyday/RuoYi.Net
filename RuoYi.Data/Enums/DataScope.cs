using System.ComponentModel;

namespace RuoYi.Data.Enums
{
    /// <summary>
    /// 数据范围（1：全部数据权限 2：自定数据权限 3：本部门数据权限 4：本部门及以下数据权限）
    /// </summary>
    public enum DataScope
    {
        [Description("全部数据权限")]
        All = 1,
        [Description("自定数据权限")]
        Custom = 2,
        [Description("本部门数据权限")]
        Department = 3,
        [Description("本部门及以下数据权限")]
        DepartmentAndSub = 4
    }
}
