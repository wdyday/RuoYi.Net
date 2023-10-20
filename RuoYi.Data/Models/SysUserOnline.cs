namespace RuoYi.Data.Models;

/// <summary>
/// 当前在线会话
/// </summary>
public class SysUserOnline
{
    /** 会话编号 */
    public string? TokenId { get; set; }

    /** 部门名称 */
    public string? DeptName{ get; set; }

    /** 用户名称 */
    public string? UserName{ get; set; }

    /** 登录IP地址 */
    public string? Ipaddr{ get; set; }

    /** 登录地址 */
    public string? LoginLocation{ get; set; }

    /** 浏览器类型 */
    public string? Browser{ get; set; }

    /** 操作系统 */
    public string? Os{ get; set; }

    /** 登录时间 */
    public long LoginTime{ get; set; }
}
