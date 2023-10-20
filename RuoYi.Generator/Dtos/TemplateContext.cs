namespace RuoYi.Generator.Dtos
{
    public class TemplateContext
    {
        public string? TplCategory { get; set; }
        public string? TableName { get; set; }
        public string? TableComment { get; set; }
        public string? FunctionName { get; set; }
        public string? ClassName { get; set; }
        public string? className { get; set; }
        public string? ModuleName { get; set; }
        public string? moduleName { get; set; }
        public string? BusinessName { get; set; }
        public string? businessName { get; set; }
        public string? BasePackage { get; set; }
        public string? PackageName { get; set; }
        public string? Author { get; set; }
        public string? DateTime { get; set; }
        public GenTableColumn PkColumn { get; set; }
        // 主键列 首字母小写
        public string? PkColumn_netField { get; set; }
        public List<string> UsingList { get; set; }
        public string? PermissionPrefix { get; set; }
        public List<GenTableColumn> Columns { get; set; }
        public GenTable Table { get; set; }
        public string? Dicts { get; set; }

        public string? ParentMenuId { get; set; }
        public string? TreeCode { get; set; }
        public string? TreeParentCode { get; set; }
        public string? treeParentCode { get; set; }
        public string? TreeName { get; set; }
        public string? treeName { get; set; }
        public int ExpandColumn { get; set; }

        public GenTable SubTable { get; set; }
        public string? SubTableName { get; set; }
        public string? SubTableFkName { get; set; }
        public string? SubTableFkClassName { get; set; }
        public string? SubClassName { get; set; }
        public string? subClassName { get; set; }
        public List<string> SubUsingList { get; set; }
    }
}
