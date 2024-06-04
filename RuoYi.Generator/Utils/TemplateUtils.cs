using Newtonsoft.Json;
using RazorEngineCore;
using RuoYi.Generator.Dtos;

namespace RuoYi.Generator.Utils
{
    /// <summary>
    /// 模板处理工具类
    /// </summary>
    internal static class TemplateUtils
    {
        /** 根路径 */
        private static string ROOT_PATH = "RuoYi";

        /** mybatis空间路径 */
        //private static string MYBATIS_PATH = "main/resources/mapper";

        /** 默认上级菜单，系统工具 */
        private static string DEFAULT_PARENT_MENU_ID = "3";

        /**
         * 设置模板变量信息
         *
         * @return 模板列表
         */
        public static TemplateContext PrepareContext(GenTable genTable)
        {
            var context = new TemplateContext
            {
                TplCategory = genTable.TplCategory,
                TableName = genTable.TableName,
                TableComment = genTable.TableComment,
                FunctionName = genTable.FunctionName,
                ClassName = genTable.ClassName,
                className = genTable?.ClassName.ToLowerCamelCase(),
                ModuleName = genTable?.ModuleName,
                moduleName = genTable?.ModuleName.ToLowerCamelCase(),
                BusinessName = genTable?.BusinessName.ToUpperCamelCase(),
                businessName = genTable?.BusinessName.ToLowerCamelCase(),
                PackageName = !string.IsNullOrEmpty(genTable?.PackageName) ? genTable.PackageName : "RuoYi",
                BasePackage = GetPackagePrefix(genTable?.PackageName),
                Author = genTable?.FunctionAuthor,
                DateTime = DateTime.Now.To_YmdHms(),
                PkColumn = genTable?.PkColumn,
                PkColumn_netField = genTable?.PkColumn?.NetField.ToLowerCamelCase(),
                UsingList = GetUsingList(genTable),
                PermissionPrefix = GetPermissionPrefix(genTable?.ModuleName, genTable?.BusinessName),
                Columns = genTable?.Columns,
                Table = genTable,
                Dicts = GetDicts(genTable)
            };

            SetMenuContext(context, genTable);
            if (GenConstants.TPL_TREE.Equals(genTable.TplCategory))
            {
                SetTreeContext(context, genTable);
            }
            if (GenConstants.TPL_SUB.Equals(genTable.TplCategory))
            {
                SetSubContext(context, genTable);
            }

            return context;
        }

        public static void SetMenuContext(TemplateContext context, GenTable genTable)
        {
            string options = genTable.Options;  // {"parentMenuId":"2009"}
            string parentMenuId = GetParentMenuId(options);
            context.ParentMenuId = parentMenuId;
        }

        public static void SetTreeContext(TemplateContext context, GenTable genTable)
        {
            string options = genTable.Options;
            string treeCode = GetTreeCode(options);
            string treeParentCode = GetTreeParentCode(options);
            string treeName = GetTreeName(options);

            context.TreeCode = treeCode;
            context.TreeParentCode = treeParentCode.ToUpperCamelCase2();
            context.TreeName = treeName.ToUpperCamelCase2();
            context.ExpandColumn = GetExpandColumn(genTable);
            if (options.Contains(GenConstants.TREE_PARENT_CODE))
            {
                context.treeParentCode = treeParentCode;
            }
            if (options.Contains(GenConstants.TREE_NAME))
            {
                context.treeName = treeName;
            }
        }

        public static void SetSubContext(TemplateContext context, GenTable genTable)
        {
            GenTable subTable = genTable.SubTable;
            string subTableName = genTable.SubTableName;
            string subTableFkName = genTable.SubTableFkName;
            string subClassName = genTable.SubTable.ClassName;
            //string subTableFkClassName = subTableFkName.ToLowerCamelCase();

            context.SubTable = subTable;
            context.SubTableName = subTableName;
            context.SubTableFkName = subTableFkName;
            context.SubClassName = subClassName;
            context.subClassName = subClassName.ToLowerCamelCase();
            context.SubUsingList = GetUsingList(genTable.SubTable);
            //context.SubTableFkClassName = subTableFkClassName;
        }

        /**
         * 获取模板信息
         *
         * @return 模板列表
         */
        public static List<string> GetTemplateList(DbType dbType, string? tplCategory)
        {
            List<string> templates = new()
            {
                "Vm/Net/Entity.cs.cshtml",
                "Vm/Net/Dto.cs.cshtml",
                "Vm/Net/Repository.cs.cshtml",
                "Vm/Net/Service.cs.cshtml",
                "Vm/Net/Controller.cs.cshtml",
                "Vm/Js/api.js.cshtml"
            };
            switch (dbType)
            {
                case DbType.MySql:
                    templates.Add("Vm/Sql/mysql.sql.cshtml");
                    break;
                case DbType.SqlServer:
                    templates.Add("Vm/Sql/sqlserver.sql.cshtml");
                    break;
                //case DbType.Oracle:
                //    break;
                default:
                    templates.Add("Vm/Sql/mysql.sql.cshtml");
                    break;

            }
            if (GenConstants.TPL_CRUD.Equals(tplCategory))
            {
                templates.Add("Vm/Vue/index.vue.cshtml");
            }
            else if (GenConstants.TPL_TREE.Equals(tplCategory))
            {
                templates.Add("Vm/Vue/index-tree.vue.cshtml");
            }
            else if (GenConstants.TPL_SUB.Equals(tplCategory))
            {
                templates.Add("Vm/Vue/index.vue.cshtml");
                templates.Add("Vm/Net/SubEntity.cs.cshtml");
                templates.Add("Vm/Net/SubDto.cs.cshtml");
            }
            return templates;
        }

        /**
         * 获取文件名
         */
        public static string GetFileName(string template, GenTable genTable)
        {
            // 文件名称
            string fileName = "";
            // 包路径
            //string packageName = !string.IsNullOrEmpty(genTable.PackageName) ? genTable.PackageName : "RuoYi";
            // 模块名
            string moduleName = genTable.ModuleName;
            // 大写类名
            string className = genTable.ClassName;
            // 业务名称
            string businessName = genTable.BusinessName;

            string netPath = ROOT_PATH + "/net";
            //string mybatisPath = MYBATIS_PATH + "/" + moduleName;
            string vuePath = ROOT_PATH + "/vue";

            if (template.Contains("Entity.cs.cshtml"))
            {
                fileName = $"{netPath}/Entities/{className}.cs";
            }
            else if (template.Contains("Dto.cs.cshtml"))
            {
                fileName = $"{netPath}/Dtos/{className}Dto.cs";
            }
            else if (template.Contains("SubEntity.cs.cshtml") && GenConstants.TPL_SUB.Equals(genTable.TplCategory))
            {
                fileName = $"{netPath}/Entities/{genTable.SubTable.ClassName}.cs";
            }
            else if (template.Contains("Repository.cs.cshtml"))
            {
                fileName = $"{netPath}/Repositories/{className}Repository.cs";
            }
            else if (template.Contains("Service.cs.cshtml"))
            {
                fileName = $"{netPath}/Services/{className}Service.cs";
            }
            else if (template.Contains("Controller.cs.cshtml"))
            {
                fileName = $"{netPath}/Controllers/{className}Controller.cs";
            }
            else if (template.Contains("sql.cshtml"))
            {
                fileName = $"{ROOT_PATH}/{businessName}Menu.sql";
            }
            else if (template.Contains("api.js.cshtml"))
            {
                fileName = $"{vuePath}/api/{moduleName}/{businessName}.js";
            }
            else if (template.Contains("index.vue.cshtml"))
            {
                fileName = $"{vuePath}/views/{moduleName}/{businessName}/index.vue";
            }
            else if (template.Contains("index-tree.vue.cshtml"))
            {
                fileName = $"{vuePath}/views/{moduleName}/{businessName}/index.vue";
            }
            return fileName;
        }

        /**
         * 获取包前缀
         *
         * @param packageName 包名称
         * @return 包前缀名称
         */
        public static string? GetPackagePrefix(string? packageName)
        {
            if (string.IsNullOrEmpty(packageName) || !packageName.Contains(".")) return packageName;
            int lastIndex = packageName.LastIndexOf(".");
            return packageName.Substring(0, lastIndex);
        }

        /**
         * 根据列类型获取导入包
         * 
         * @param genTable 业务表对象
         * @return 返回需要导入的包列表
         */
        public static List<string> GetUsingList(GenTable genTable)
        {
            List<GenTableColumn> columns = genTable.Columns ?? new List<GenTableColumn>();
            GenTable subGenTable = genTable.SubTable;
            List<string> importList = new List<string>();
            if (subGenTable != null)
            {
                importList.Add("System.Collections.Generic");
            }
            //foreach (GenTableColumn column in columns)
            //{
            //    if (!GenUtils.IsSuperColumn(column.NetField) && GenConstants.TYPE_DATE.Equals(column.NetType))
            //    {
            //        importList.Add("System");
            //    }
            //    //else if (!GenUtils.IsSuperColumn(column.NetField) && GenConstants.TYPE_DOUBLE.Equals(column.NetType))
            //    //{
            //    //    importList.add("java.math.BigDecimal");
            //    //}
            //}
            return importList;
        }

        /**
         * 根据列类型获取字典组
         * 
         * @param genTable 业务表对象
         * @return 返回字典组
         */
        public static string GetDicts(GenTable genTable)
        {
            List<GenTableColumn> columns = genTable.Columns ?? new List<GenTableColumn>();
            HashSet<string> dicts = new HashSet<string>();
            AddDicts(dicts, columns);
            if (genTable.SubTable != null)
            {
                List<GenTableColumn> subColumns = genTable.SubTable.Columns;
                AddDicts(dicts, subColumns);
            }

            return string.Join(", ", dicts);
        }

        /**
         * 添加字典列表
         * 
         * @param dicts 字典列表
         * @param columns 列集合
         */
        public static void AddDicts(HashSet<string> dicts, List<GenTableColumn> columns)
        {
            var htmlTypes = new List<string> { GenConstants.HTML_SELECT, GenConstants.HTML_RADIO, GenConstants.HTML_CHECKBOX };
            foreach (GenTableColumn column in columns)
            {
                if (!column.IsSuperColumn() && !string.IsNullOrEmpty(column.DictType) && htmlTypes.Contains(column.HtmlType))
                {
                    dicts.Add("'" + column.DictType + "'");
                }
            }
        }

        /**
         * 获取权限前缀
         *
         * @param moduleName 模块名称
         * @param businessName 业务名称
         * @return 返回权限前缀
         */
        public static string GetPermissionPrefix(string moduleName, string businessName)
        {
            return $"{moduleName.ToLowerCamelCase()}:{businessName.ToLowerCamelCase()}";
        }

        /**
         * 获取上级菜单ID字段
         *
         * @param options 配置json, 如: {"parentMenuId":"2009"}
         * @return 上级菜单ID字段
         */
        public static string GetParentMenuId(string options)
        {
            if (!string.IsNullOrEmpty(options) && options.Contains(GenConstants.PARENT_MENU_ID))
            {
                var opt = JsonConvert.DeserializeObject<GenTableOptions>(options);
                if (opt != null && !string.IsNullOrEmpty(opt.ParentMenuId))
                {
                    return opt.ParentMenuId;
                }
            }
            return DEFAULT_PARENT_MENU_ID;
        }

        /**
         * 获取树编码
         *
         * @param options 配置json, 如: {"treeCode":"2009"}
         * @return 树编码
         */
        public static string GetTreeCode(string options)
        {
            if (!string.IsNullOrEmpty(options) && options.Contains(GenConstants.TREE_CODE))
            {
                var json = JsonConvert.DeserializeObject<dynamic>(options);
                if (json != null && !string.IsNullOrEmpty(json.treeCode))
                {
                    return json.treeCode.ToLowerCamelCase();
                }
            }
            return "";
        }

        /**
         * 获取树父编码
         *
         * @param options 配置json, 如: {"treeParentCode":"2009"}
         * @return 树父编码
         */
        public static string GetTreeParentCode(string options)
        {
            if (!string.IsNullOrEmpty(options) && options.Contains(GenConstants.TREE_PARENT_CODE))
            {
                var json = JsonConvert.DeserializeObject<dynamic>(options);
                if (json != null && !string.IsNullOrEmpty(json.treeParentCode))
                {
                    return json.treeParentCode.ToLowerCamelCase();
                }
            }
            return "";
        }

        /**
         * 获取树名称
         *
         * @param @param options 配置json, 如: {"treeName":"2009"}
         * @return 树名称
         */
        public static string GetTreeName(string options)
        {
            if (!string.IsNullOrEmpty(options) && options.Contains(GenConstants.TREE_NAME))
            {
                var json = JsonConvert.DeserializeObject<dynamic>(options);
                if (json != null && !string.IsNullOrEmpty(json.treeName))
                {
                    return json.treeName.ToLowerCamelCase();
                }
            }
            return "";
        }

        /**
         * 获取需要在哪一列上面显示展开按钮
         *
         * @param genTable 业务表对象
         * @return 展开按钮列序号
         */
        public static int GetExpandColumn(GenTable genTable)
        {
            string options = genTable.Options;
            string treeName = GetTreeName(options);
            int num = 0;
            foreach (GenTableColumn column in genTable.Columns)
            {
                if (column.Is_List())
                {
                    num++;
                    string columnName = column.ColumnName;
                    if (columnName.EqualsIgnoreCase(treeName))
                    {
                        break;
                    }
                }
            }
            return num;
        }

        /// <summary>
        /// 渲染模板
        /// </summary>
        /// <param name="context">模板信息</param>
        /// <param name="templatePath">模板相对路径, 如: Vm/Net/Entity.cshtml</param>
        /// <returns></returns>
        public static string Compile(TemplateContext context, string templatePath)
        {
            IRazorEngine razorEngine = new RazorEngine();

            // 渲染模板
            string tpl = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "StaticFiles", templatePath));
            // https://github.com/adoconnection/RazorEngineCore
            var template = razorEngine.Compile<RazorEngineTemplateBase<TemplateContext>>(tpl, builder =>
            {
                //builder.AddUsing("RuoYi.Data.Entities"); // using

                builder.AddAssemblyReferenceByName("System.Collections");  // by name
                builder.AddAssemblyReferenceByName("RuoYi.Data");  // by name

                builder.AddAssemblyReference(typeof(RuoYi.Generator.Dtos.TemplateContext)); // by type

                //builder.AddAssemblyReference(Assembly.Load("source")); // by reference
            });
            string text = template.Run(instance =>
            {
                instance.Model = context;
            });

            return text;
        }
    }
}
