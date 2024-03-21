namespace RuoYi.Common.Data;

public class PageDomain
{
    /** 当前记录起始索引 */
    public int PageNum { get; set; }

    /** 每页显示记录数 */
    public int PageSize { get; set; }

    /** 排序列 */
    public string? OrderByColumn { get; set; }

    /** 排序的方向desc或者asc */
    public string IsAsc { get; set; } = "asc";

    ///** 分页参数合理化 */
    //public bool Reasonable { get; set; } = true;

    // order by a.id asc
    public string OrderBy { get; set; }

    // 字段属性名
    public string PropertyName { get; set; }
}
