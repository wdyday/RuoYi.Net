using System.Collections.Generic;
namespace RuoYi.Data.Dtos
{
    /// <summary>
    ///  用户与岗位关联表 对象 sys_user_post
    ///  author ruoyi.net
    ///  date   2023-08-23 09:43:50
    /// </summary>
    public class SysUserPostDto : BaseDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long? UserId { get; set; }
        /// <summary>
        /// 岗位ID
        /// </summary>
        public long? PostId { get; set; }
    }
}
