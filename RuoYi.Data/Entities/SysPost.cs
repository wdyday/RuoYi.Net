using SqlSugar;
using System;
using System.Collections.Generic;
using RuoYi.Data.Entities;

namespace RuoYi.Data.Entities
{
    /// <summary>
    ///  岗位信息表 对象 sys_post
    ///  author ruoyi
    ///  date   2023-08-30 13:21:36
    /// </summary>
    [SugarTable("sys_post", "岗位信息表")]
    public class SysPost : UserBaseEntity
    {
        /// <summary>
        /// 岗位ID (post_id)
        /// </summary>
        [SugarColumn(ColumnName = "post_id", ColumnDescription = "岗位ID", IsPrimaryKey = true, IsIdentity = true)]
        public long PostId { get; set; }
        /// <summary>
        /// 岗位编码 (post_code)
        /// </summary>
        [SugarColumn(ColumnName = "post_code", ColumnDescription = "岗位编码")]
        public string PostCode { get; set; }
        /// <summary>
        /// 岗位名称 (post_name)
        /// </summary>
        [SugarColumn(ColumnName = "post_name", ColumnDescription = "岗位名称")]
        public string PostName { get; set; }
        /// <summary>
        /// 显示顺序 (post_sort)
        /// </summary>
        [SugarColumn(ColumnName = "post_sort", ColumnDescription = "显示顺序")]
        public int PostSort { get; set; }
        /// <summary>
        /// 状态（0正常 1停用） (status)
        /// </summary>
        [SugarColumn(ColumnName = "status", ColumnDescription = "状态（0正常 1停用）")]
        public string Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注")]
        public string? Remark { get; set; }
    }
}
