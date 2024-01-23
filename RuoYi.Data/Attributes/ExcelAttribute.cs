namespace RuoYi.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ExcelAttribute : Attribute
    {
        public ExcelOperationType Type { get; set; }

        public string? Name { get; set; }

        public string[]? Aliases { get; set; }

        public double Width { get; set; }

        public string? Format { get; set; }

        public bool Ignore { get; set; }

        public int? Index { get; set; }
    }

    /// <summary>
    /// 操作类型（0：导出导入；1：仅导出；2：仅导入）
    /// </summary>
    public enum ExcelOperationType
    {
        All,
        Export,
        Import
    }
}
