using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace RuoYi.Data.Dtos
{
    public class GenTableDto : BaseDto
    {
        /** 编号 */
        public long TableId { get; set; }

        /** 表名称 */
        [Required(ErrorMessage = "表名称不能为空")]
        public string? TableName { get; set; }

        /** 表描述 */
        [Required(ErrorMessage = "表描述不能为空")]
        public string? TableComment { get; set; }

        /** 关联父表的表名 */
        public string? SubTableName { get; set; }

        /** 本表关联父表的外键名 */
        public string? SubTableFkName { get; set; }

        /** 实体类名称(首字母大写) */
        [Required(ErrorMessage = "实体类名称不能为空")]
        public string? ClassName { get; set; }

        /** 使用的模板（crud单表操作 tree树表操作 sub主子表操作） */
        public string? TplCategory { get; set; }

        /** 生成包路径 */
        [Required(ErrorMessage = "生成包路径不能为空")]
        public string? PackageName { get; set; }

        /** 生成模块名 */
        [Required(ErrorMessage = "生成模块名不能为空")]
        public string? ModuleName { get; set; }

        /** 生成业务名 */
        [Required(ErrorMessage = "生成业务名不能为空")]
        public string? BusinessName { get; set; }

        /** 生成功能名 */
        [Required(ErrorMessage = "生成功能名不能为空")]
        public string? FunctionName { get; set; }

        /** 生成作者 */
        [Required(ErrorMessage = "作者不能为空")]
        public string? FunctionAuthor { get; set; }

        /** 生成代码方式（0zip压缩包 1自定义路径） */
        public string? GenType { get; set; }

        /** 生成路径（不填默认项目路径） */
        public string? GenPath { get; set; }

        /** 其它生成选项 */
        public string? Options { get; set; }

        /* -------------------- Options -------------------- */
        /** 树编码字段 */
        public string? TreeCode { get; set; }

        /** 树父编码字段 */
        public string? TreeParentCode { get; set; }

        /** 树名称字段 */
        public string? TreeName { get; set; }

        /** 上级菜单ID字段 */
        public string? ParentMenuId { get; set; }

        /** 上级菜单名称字段 */
        public string? ParentMenuName { get; set; }
        /* -------------------- Options -------------------- */

        /** 主键信息 */
        public GenTableColumnDto? PkColumn { get; set; }

        /** 子表信息 */
        public GenTableDto? SubTable { get; set; }

        /** 表列信息 */
        public List<GenTableColumnDto>? Columns { get; set; }
    }

    public class GenTableOptions
    {
        /** 树编码字段 */
        public string? TreeCode { get; set; }

        /** 树父编码字段 */
        [SugarColumn(IsIgnore = true)]
        public string? TreeParentCode { get; set; }

        /** 树名称字段 */
        public string? TreeName { get; set; }

        /** 上级菜单ID字段 */
        public string? ParentMenuId { get; set; }

        /** 上级菜单名称字段 */
        public string? ParentMenuName { get; set; }
    }
}
