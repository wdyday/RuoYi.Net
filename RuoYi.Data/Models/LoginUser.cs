using RuoYi.Data.Dtos;

namespace RuoYi.Data.Models
{
    public class LoginUser
    {
        public LoginUser() { }

        public LoginUser(long userId, long deptId, SysUserDto user, List<string> permissions)
        {
            this.UserId = userId;
            this.DeptId = deptId;
            //this.UserName = userName;
            this.User = user;
            this.Permissions = permissions;
        }

        /// <summary>
        /// 用户名
        /// </summary>
        [Newtonsoft.Json.JsonProperty(Order = 0)] 
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public string Password { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId{ get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public long DeptId{ get; set; }

        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public string Token{ get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public long LoginTime{ get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public long ExpireTime{ get; set; }

        /// <summary>
        /// 登录IP地址
        /// </summary>
        public string IpAddr{ get; set; }

        /// <summary>
        /// 登录地点
        /// </summary>
        public string LoginLocation{ get; set; }

        /// <summary>
        /// 浏览器类型
        /// </summary>
        public string Browser{ get; set; }

        /// <summary>
        /// 操作系统
        /// </summary>
        public string OS{ get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public SysUserDto User { get; set; }

        /// <summary>
        /// 权限列表
        /// </summary>
        public List<string> Permissions{ get; set; }
    }
}
