using System.ComponentModel;

namespace RuoYi.Common.Enums
{
    /// <summary>
    /// 业务操作类型
    /// </summary>
    public enum BusinessType
    {
        [Description("其它")]
        OTHER,

        [Description("新增")]
        INSERT,

        [Description("修改")]
        UPDATE,

        [Description("删除")]
        DELETE,

        [Description("授权")]
        GRANT,

        [Description("导出")]
        EXPORT,

        [Description("导入")]
        IMPORT,

        [Description("强退")]
        FORCE,

        [Description("生成代码")]
        GENCODE,

        [Description("清空数据")]
        CLEAN,
    }
}
