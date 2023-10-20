namespace RuoYi.Common.Constants
{
    /// <summary>
    /// 用户常量信息
    /// </summary>
    public class UserConstants
    {
        /**
         * 平台内系统用户的唯一标志
         */
        public const string SYS_USER = "SYS_USER";

        /** 正常状态 */
        public const string NORMAL = "0";

        /** 异常状态 */
        public const string EXCEPTION = "1";

        /** 用户封禁状态 */
        public const string USER_DISABLE = "1";

        /** 角色封禁状态 */
        public const string ROLE_DISABLE = "1";

        /** 部门正常状态 */
        public const string DEPT_NORMAL = "0";

        /** 部门停用状态 */
        public const string DEPT_DISABLE = "1";

        /** 字典正常状态 */
        public const string DICT_NORMAL = "0";

        /** 是否为系统默认（是） */
        public const string YES = "Y";

        /** 是否菜单外链（是） */
        public const string YES_FRAME = "0";

        /** 是否菜单外链（否） */
        public const string NO_FRAME = "1";

        /** 菜单类型（目录） */
        public const string TYPE_DIR = "M";

        /** 菜单类型（菜单） */
        public const string TYPE_MENU = "C";

        /** 菜单类型（按钮） */
        public const string TYPE_BUTTON = "F";

        /** Layout组件标识 */
        public const string LAYOUT = "Layout";

        /** ParentView组件标识 */
        public const string PARENT_VIEW = "ParentView";

        /** InnerLink组件标识 */
        public const string INNER_LINK = "InnerLink";

        /** 校验是否唯一的返回标识 */
        public const bool UNIQUE = true;
        public const bool NOT_UNIQUE = false;

        /**
         * 用户名长度限制
         */
        public const int USERNAME_MIN_LENGTH = 2;
        public const int USERNAME_MAX_LENGTH = 20;

        /**
         * 密码长度限制
         */
        public const int PASSWORD_MIN_LENGTH = 5;
        public const int PASSWORD_MAX_LENGTH = 20;
    }
}
