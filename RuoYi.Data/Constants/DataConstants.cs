namespace RuoYi.Data
{
    public class DataConstants
    {
        // 主库
        public const string Master = "master";
        // 从库
        public const string Slave = "slave";

        // 登录用户信息
        public const string USER_NAME = "UserName";
        public const string USER_ID = "UserId";
        public const string USER_DEPT_ID = "DeptId";
        public const string LOGIN_USER_KEY = "";
    }

    /// <summary>
    /// 删除标志（0代表存在 2代表删除）
    /// </summary>
    public class DelFlag
    {
        public const string No = "0";
        public const string Yes = "2";

        public static string ToDesc(string? val)
        {
            return val switch
            {
                No => "未删除",
                Yes => "已删除",
                _ => "",
            };
        }
    }

    /// <summary>
    /// 1.状态（0正常 1停用）; 2.是否为外链（0是 1否）; 3. 是否缓存（0缓存 1不缓存）, 4.显示状态（0显示 1隐藏）
    /// </summary>
    public class Status
    {
        public const string Enabled = "0";
        public const string Disabled = "1";

        public static string ToDesc(string? val)
        {
            return val switch
            {
                Enabled => "正常",
                Disabled => "停用",
                _ => "",
            };
        }
        public static string ToVal(string? desc)
        {
            return desc switch
            {
                "正常" => Enabled,
                "停用" => Disabled,
                _ => ""
            };
        }
    }

    /// <summary>
    /// 用户性别（0男 1女 2未知）
    /// </summary>
    public class Sex
    {
        public const string Male = "0";
        public const string Female = "1";
        public const string Unknown = "2";

        public static string ToDesc(string? val)
        {
            return val switch
            {
                Male => "男",
                Female => "女",
                _ => "未知"
            };
        }
        public static string ToVal(string? desc)
        {
            return desc switch
            {
                 "男"=> Male,
                 "女" => Female,
                _ => Unknown
            };
        }
    }

    ///// <summary>
    ///// 操作类别（0其它 1后台用户 2手机端用户）
    ///// </summary>
    //public class OperatorType
    //{
    //    public const string Other = "0";
    //    public const string Web = "1";
    //    public const string Mobile = "2";

    //    public static string ToDesc(string? val)
    //    {
    //        return val switch
    //        {
    //            Other => "其它",
    //            Web => "后台用户",
    //            Mobile => "手机端用户",
    //            _ => "未知"
    //        };
    //    }
    //}

    /// <summary>
    /// 数据范围（1：全部数据权限 2：自定数据权限 3：本部门数据权限 4：本部门及以下数据权限）
    /// </summary>
    public class DataScope
    {
        public const string All = "1";
        public const string Custom = "2";
        public const string Department = "3";
        public const string DepartmentAndSub = "4";

        public static string ToDesc(string? val)
        {
            return val switch
            {
                All => "全部数据权限",
                Custom => "自定数据权限",
                Department => "本部门数据权限",
                DepartmentAndSub => "本部门及以下数据权限",
                _ => ""
            };
        }
    }

    public class YesNo
    {
        public const string No = "0";
        public const string Yes = "1";

        public static string ToDesc(string? val)
        {
            return val switch
            {
                No => "否",
                Yes => "是",
                "N" => "否",
                "Y" => "是",
                _ => ""
            };
        }
        public static string ToVal(string? desc)
        {
            return desc switch
            {
                "否" => No,
                "是" => Yes,
                _ => ""
            };
        }
    }
}
