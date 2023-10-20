namespace RuoYi.Generator.Constants
{
    /// <summary>
    /// 代码生成通用常量
    /// </summary>
    public class GenConstants
    {
        /** 单表（增删改查） */
        public const string TPL_CRUD = "crud";

        /** 树表（增删改查） */
        public const string TPL_TREE = "tree";

        /** 主子表（增删改查） */
        public const string TPL_SUB = "sub";

        /** 树编码字段 */
        public const string TREE_CODE = "treeCode";

        /** 树父编码字段 */
        public const string TREE_PARENT_CODE = "treeParentCode";

        /** 树名称字段 */
        public const string TREE_NAME = "treeName";

        /** 上级菜单ID字段 */
        public const string PARENT_MENU_ID = "parentMenuId";

        /** 上级菜单名称字段 */
        public const string PARENT_MENU_NAME = "parentMenuName";

        /** 数据库字符串类型 */
        public static string[] COLUMNTYPE_STR = new string[] { "char", "varchar", "nvarchar", "varchar2" };

        /** 数据库文本类型 */
        public static string[] COLUMNTYPE_TEXT = new string[] { "tinytext", "text", "mediumtext", "longtext" };

        /** 数据库时间类型 */
        public static string[] COLUMNTYPE_TIME = new string[] { "datetime", "time", "date", "timestamp" };

        /** 数据库数字类型 */
        public static string[] COLUMNTYPE_NUMBER = new string[] { "tinyint", "smallint", "mediumint", "int", "number", "integer",
            "bit", "bigint", "float", "double", "decimal" };

        /** 页面不需要编辑字段 */
        public static string[] COLUMNNAME_NOT_EDIT = new string[] { "id", "create_by", "create_time", "del_flag" };

        /** 页面不需要显示的列表字段 */
        public static string[] COLUMNNAME_NOT_LIST = new string[] { "id", "create_by", "create_time", "del_flag", "update_by",
            "update_time" };

        /** 页面不需要查询字段 */
        public static string[] COLUMNNAME_NOT_QUERY = new string[] { "id", "create_by", "create_time", "del_flag", "update_by", "update_time", "remark" };

        ///** Entity基类字段 */
        //public static string[] BASE_ENTITY = new string[] { "createBy", "createTime", "updateBy", "updateTime", "remark" };

        ///** Tree基类字段 */
        //public static string[] TREE_ENTITY = new string[] { "parentName", "parentId", "orderNum", "ancestors", "children" };

        /** 文本框 */
        public const string HTML_INPUT = "input";

        /** 文本域 */
        public const string HTML_TEXTAREA = "textarea";

        /** 下拉框 */
        public const string HTML_SELECT = "select";

        /** 单选框 */
        public const string HTML_RADIO = "radio";

        /** 复选框 */
        public const string HTML_CHECKBOX = "checkbox";

        /** 日期控件 */
        public const string HTML_DATETIME = "datetime";

        /** 图片上传控件 */
        public const string HTML_IMAGE_UPLOAD = "imageUpload";

        /** 文件上传控件 */
        public const string HTML_FILE_UPLOAD = "fileUpload";

        /** 富文本控件 */
        public const string HTML_EDITOR = "editor";

        /** 字符串类型 */
        public const string TYPE_STRING = "string";

        /** 整型 */
        public const string TYPE_INTEGER = "int";

        /** 长整型 */
        public const string TYPE_LONG = "long";

        /** 浮点型 */
        public const string TYPE_DOUBLE = "double";

        /** 高精度计算类型 */
        public const string TYPE_DECIMAL = "decimal";

        /** 时间类型 */
        public const string TYPE_DATE = "DateTime";

        /** 模糊查询 */
        public const string QUERY_LIKE = "LIKE";

        /** 相等查询 */
        public const string QUERY_EQ = "EQ";

        /** 需要 */
        public const string REQUIRE = "1";
    }
}
