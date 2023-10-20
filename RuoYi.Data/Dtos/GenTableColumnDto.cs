namespace RuoYi.Data.Dtos
{
    public class GenTableColumnDto : BaseDto
    {
        /** 编号 */
        public long ColumnId { get; set; }

        /** 归属表编号 */
        public long? TableId { get; set; }

        /** 列名称 */
        public string? ColumnName { get; set; }

        /** 列描述 */
        public string? ColumnComment { get; set; }

        /** 列类型 */
        public string? ColumnType { get; set; }

        /** .NET类型 */
        public string? NetType { get; set; }

        /** .NET字段名 */
        //@NotBlank(message = ".Net属性不能为空")
        public string? NetField { get; set; }

        /** 是否主键（1是） */
        public string? IsPk { get; set; }

        /** 是否自增（1是） */
        public string? IsIncrement { get; set; }

        /** 是否必填（1是） */
        public string? IsRequired { get; set; }

        /** 是否为插入字段（1是） */
        public string? IsInsert { get; set; }

        /** 是否编辑字段（1是） */
        public string? IsEdit { get; set; }

        /** 是否列表字段（1是） */
        public string? IsList { get; set; }

        /** 是否查询字段（1是） */
        public string? IsQuery { get; set; }

        /** 查询方式（EQ等于、NE不等于、GT大于、LT小于、LIKE模糊、BETWEEN范围） */
        public string? QueryType { get; set; }

        /** 显示类型（input文本框、textarea文本域、select下拉框、checkbox复选框、radio单选框、datetime日期控件、image图片上传控件、upload文件上传控件、editor富文本控件） */
        public string? HtmlType { get; set; }

        /** 字典类型 */
        public string? DictType { get; set; }

        /** 排序 */
        public int? Sort { get; set; }

    }
}
