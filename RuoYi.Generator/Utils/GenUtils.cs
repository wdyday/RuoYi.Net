using RuoYi.Framework.Exceptions;
using RuoYi.Generator.Options;

namespace RuoYi.Generator.Utils
{
    /// <summary>
    /// 代码生成器 工具类
    /// </summary>
    internal static class GenUtils
    {
        private static readonly GenOptions _genConfig = App.GetOptionsMonitor<GenOptions>();

        /// <summary>
        /// 初始化表信息
        /// </summary>
        public static void InitTable(GenTable genTable, string operName)
        {
            genTable.ClassName = ConvertClassName(genTable.TableName);
            genTable.PackageName = _genConfig.PackageName;
            genTable.ModuleName = GetModuleName(_genConfig.PackageName);
            genTable.BusinessName = GetBusinessName(genTable.TableName);
            genTable.FunctionName = ReplaceText(genTable.TableComment);
            genTable.FunctionAuthor = _genConfig.Author;
            genTable.CreateBy = operName;

            genTable.GenType = genTable.GenType ?? "0";
            genTable.TplCategory = genTable.TplCategory ?? "crud";
        }

        /**
         * 初始化列属性字段
         */
        public static void InitColumnField(GenTableColumn column, GenTable table)
        {
            string dataType = GetDbType(column.ColumnType);
            string columnName = column.ColumnName;
            column.TableId = table.TableId;
            column.CreateBy = table.CreateBy;
            // 设置 .net字段名
            column.NetField = columnName.ToUpperCamelCase2();
            // 设置默认类型
            column.NetType = GenConstants.TYPE_STRING;
            column.QueryType = GenConstants.QUERY_EQ;

            if (ArraysContains(GenConstants.COLUMNTYPE_STR, dataType) || ArraysContains(GenConstants.COLUMNTYPE_TEXT, dataType))
            {
                // 字符串长度超过500设置为文本域
                int columnLength = GetColumnLength(column.ColumnType);
                string htmlType = columnLength >= 500 || ArraysContains(GenConstants.COLUMNTYPE_TEXT, dataType) ? GenConstants.HTML_TEXTAREA : GenConstants.HTML_INPUT;
                column.QueryType = htmlType;
            }
            else if (ArraysContains(GenConstants.COLUMNTYPE_TIME, dataType))
            {
                column.NetType = GenConstants.TYPE_DATE;
                column.HtmlType = GenConstants.HTML_DATETIME;
            }
            else if (ArraysContains(GenConstants.COLUMNTYPE_NUMBER, dataType))
            {
                column.HtmlType = GenConstants.HTML_INPUT;

                // 如果是浮点型 统一用Decimal, 如: decimal(18,2)
                string[] str = column.ColumnType.SubstringBetween("(", ")").Split(",");
                if (str != null && str.Length == 2 && int.Parse(str[1]) > 0)
                {
                    column.NetType = GenConstants.TYPE_DECIMAL;
                }
                // 如果是整形
                else if (str != null && str.Length == 1 && int.Parse(str[0]) <= 11)
                {
                    column.NetType = GenConstants.TYPE_INTEGER;
                }
                // 长整形
                else
                {
                    column.NetType = GenConstants.TYPE_LONG;
                }
            }
            else
            {
                // 设置默认类型
                column.NetType = GenConstants.TYPE_STRING;
            }
            if (!column.Is_Required() && !column.Is_Pk())
            {
                column.NetType = $"{column.NetType}?";
            }

            // 插入字段（默认所有字段都需要插入）
            column.IsInsert = YesNo.Yes;

            // 编辑字段
            if (!ArraysContains(GenConstants.COLUMNNAME_NOT_EDIT, columnName) && column.IsPk != YesNo.Yes)
            {
                column.IsEdit = YesNo.Yes;
            }
            // 列表字段
            if (!ArraysContains(GenConstants.COLUMNNAME_NOT_LIST, columnName) && column.IsPk != YesNo.Yes)
            {
                column.IsList = YesNo.Yes;
            }
            // 查询字段
            if (!ArraysContains(GenConstants.COLUMNNAME_NOT_QUERY, columnName) && column.IsPk != YesNo.Yes)
            {
                column.IsQuery = YesNo.Yes;
            }

            // 查询字段类型
            if (columnName.EndsWith("name", true, null))
            {
                column.QueryType = GenConstants.QUERY_LIKE;
            }
            // 状态字段设置单选框
            if (columnName.EndsWith("status", true, null))
            {
                column.HtmlType = GenConstants.HTML_RADIO;
            }
            // 类型&性别字段设置下拉框
            else if (columnName.EndsWith("type", true, null) || columnName.EndsWith("sex", true, null))
            {
                column.HtmlType = GenConstants.HTML_SELECT;
            }
            // 图片字段设置图片上传控件
            else if (columnName.EndsWith("image", true, null))
            {
                column.HtmlType = GenConstants.HTML_IMAGE_UPLOAD;
            }
            // 文件字段设置文件上传控件
            else if (columnName.EndsWith("file", true, null))
            {
                column.HtmlType = GenConstants.HTML_FILE_UPLOAD;
            }
            // 内容字段设置富文本控件
            else if (columnName.EndsWith("content", true, null))
            {
                column.HtmlType = GenConstants.HTML_EDITOR;
            }
        }

        /**
         * 校验数组是否包含指定值
         * 
         * @param arr 数组
         * @param targetValue 值
         * @return 是否包含
         */
        public static bool ArraysContains(string[] arr, string targetValue)
        {
            return arr.Contains(targetValue);
        }

        /**
         * 获取模块名
         * 
         * @param packageName 包名
         * @return 模块名
         */
        public static string GetModuleName(string packageName)
        {
            int lastIndex = packageName.LastIndexOf(".");
            int nameLength = packageName.Length;
            return packageName.Substring(lastIndex + 1, nameLength - (lastIndex + 1));
        }

        /**
         * 获取业务名
         * 
         * @param tableName 表名
         * @return 业务名
         */
        public static string GetBusinessName(string tableName)
        {
            int lastIndex = tableName.LastIndexOf("_");
            int nameLength = tableName.Length;
            return tableName.Substring(lastIndex + 1, nameLength - (lastIndex + 1));
        }

        /**
         * 表名转换成 类名
         * 
         * @param tableName 表名称
         * @return 类名
         */
        public static string ConvertClassName(string tableName)
        {
            var genConfig = App.GetOptionsMonitor<GenOptions>();
            bool autoRemovePre = genConfig.AutoRemovePre;
            string tablePrefix = genConfig.TablePrefix;
            if (autoRemovePre && !string.IsNullOrEmpty(tablePrefix))
            {
                string[] searchList = tablePrefix.Split(",");
                tableName = ReplaceFirst(tableName, searchList);
            }
            return tableName.ToUpperCamelCase2();
        }


        /// <summary>
        /// 批量替换前缀
        /// </summary>
        /// <param name="replacementm">替换值</param>
        /// <param name="searchList">替换列表</param>
        /// <returns></returns>
        public static string ReplaceFirst(string replacementm, string[] searchList)
        {
            string text = replacementm;
            foreach (string searchString in searchList)
            {
                if (replacementm.StartsWith(searchString))
                {
                    text = replacementm.ReplaceFirst(searchString, "");
                    break;
                }
            }
            return text;
        }

        /**
         * 关键字替换
         * 
         * @param text 需要被替换的名字
         * @return 替换后的名字
         */
        public static string ReplaceText(string text)
        {
            return text.Replace("(?:表|若依)", "");
        }

        /**
         * 获取数据库类型字段
         * 
         * @param columnType 列类型
         * @return 截取后的列类型
         */
        public static string GetDbType(string columnType)
        {
            if (columnType.IndexOf("(") > 0)
            {
                return columnType.SubstringBefore("(");
            }
            else
            {
                return columnType;
            }
        }

        /**
         * 获取字段长度
         * 
         * @param columnType 列类型
         * @return 截取后的列类型
         */
        public static int GetColumnLength(string? columnType)
        {
            if (columnType != null && columnType.IndexOf("(") > 0)
            {
                string length = columnType.SubstringBetween("(", ")");
                return int.Parse(length);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 导出zip
        /// </summary>
        public static async Task ExportZipAsync(HttpResponse response, byte[] data, string? fileName = null)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = $"ruoyi_net_{DateTime.Now.ToYmdHms()}.zip";
            }

            try
            {
                response.ContentType = "application/octet-stream; charset=UTF-8";
                response.Headers["Access-Control-Allow-Origin"] = "*";
                response.Headers["Access-Control-Expose-Headers"] = "Content-Disposition";
                response.Headers["Content-Length"] = data.Length.ToString();
                response.Headers["Content-Disposition"] = $"attachment;filename=\"{fileName}\"";

                using var stream = new MemoryStream();
                await stream.WriteAsync(data, 0, data.Length);
                stream.Seek(0, SeekOrigin.Begin);
                await stream.CopyToAsync(response.Body);
                await response.CompleteAsync();
            }
            catch (Exception ex)
            {
                RuoYi.Framework.Logging.Log.Error("Export zip error.", ex);
                throw new ServiceException("导出zip失败");
            }
        }
    }
}
