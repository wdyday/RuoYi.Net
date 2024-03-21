using Microsoft.AspNetCore.Http;
using MiniExcelLibs;
using MiniExcelLibs.Attributes;
using MiniExcelLibs.OpenXml;
using RuoYi.Data.Attributes;
using RuoYi.Framework.Exceptions;
using RuoYi.Framework.Extensions;
using RuoYi.Framework.Logging;
using SqlSugar;
using System.Text;
using System.Web;

namespace RuoYi.Common.Utils
{
    public static class ExcelUtils
    {
        #region Import by stream

        /// <summary>
        /// Excel 导入
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="sheetName">sheet名称, 默认取第一个</param>
        /// <param name="startCell">指定单元格开始读取数据, 如 A2, 从 A列的第二行开始读取</param>
        public static async Task<IEnumerable<T>> ImportAsync<T>(MemoryStream stream, string? sheetName = null, string? startCell = "A1") where T : class, new()
        {
            var config = GetOpenXmlConfiguration<T>(ExcelOperationType.Import);
            return await stream.QueryAsync<T>(sheetName, ExcelType.XLSX, startCell: startCell, configuration: config);
        }

        /// <summary>
        /// Excel 导入所有sheet
        /// </summary>
        /// <param name="stream">文件流</param>
        public static async Task<List<T>> ImportAllAsync<T>(MemoryStream stream, string? startCell = "A1") where T : class, new()
        {
            var list = new List<T>();

            var config = GetOpenXmlConfiguration<T>(ExcelOperationType.Import);

            var sheetNames = MiniExcel.GetSheetNames(stream);
            foreach (var sheetName in sheetNames)
            {
                var rows = await stream.QueryAsync<T>(sheetName, ExcelType.XLSX, startCell: startCell, configuration: config);
                list.AddRange(rows);
            }

            return list;
        }

        #endregion

        #region Import by filePath

        /// <summary>
        /// Excel 导入
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="sheetName">sheet名称, 默认取第一个</param>
        public static async Task<IEnumerable<T>> ImportAsync<T>(string filePath, string? sheetName = null) where T : class, new()
        {
            return await MiniExcel.QueryAsync<T>(filePath, sheetName, ExcelType.XLSX);
        }

        /// <summary>
        /// Excel 导入所有sheet
        /// </summary>
        /// <param name="stream">文件流</param>
        public static async Task<List<T>> ImportAllAsync<T>(string filePath) where T : class, new()
        {
            var list = new List<T>();

            var sheetNames = MiniExcel.GetSheetNames(filePath);
            foreach (var sheetName in sheetNames)
            {
                var rows = await MiniExcel.QueryAsync<T>(filePath, sheetName, ExcelType.XLSX);
                list.AddRange(rows);
            }

            return list;
        }

        #endregion

        #region Export
        public static async Task GetImportTemplateAsync<T>(HttpResponse response, string? sheetName = null) where T : class, new()
        {
            await GetImportTemplateAsync<T>(response, null, sheetName);
        }

        public static async Task GetImportTemplateAsync<T>(HttpResponse response, string? fileName = null, string? sheetName = null) where T : class, new()
        {
            var list = new List<T>();
            await ExportAsync<T>(response, list, fileName, sheetName, ExcelOperationType.Import);
        }

        public static async Task ExportAsync<T>(HttpResponse response, IEnumerable<T> list, string? fileName = null, string? sheetName = null, ExcelOperationType optType = ExcelOperationType.Export) where T : class, new()
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = $"{Guid.NewGuid():N}.xlsx";
            }
            if (string.IsNullOrEmpty(sheetName))
            {
                sheetName = "Sheet1";
            }

            try
            {
                fileName = HttpUtility.UrlEncode(fileName, Encoding.UTF8);
                response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                response.Headers["Content-Disposition"] = $"attachment;filename=\"{fileName}\"";

                var config = GetOpenXmlConfiguration<T>(optType);

                using var stream = new MemoryStream();
                await stream.SaveAsAsync(list, sheetName: sheetName, configuration: config);
                stream.Seek(0, SeekOrigin.Begin);
                await stream.CopyToAsync(response.Body);
                await response.CompleteAsync();
            }
            catch (Exception ex)
            {
                Log.Error("Export error.", ex);
                throw new ServiceException("导出失败");
            }
        }

        #endregion

        /// <summary>
        /// 动态列设置
        /// </summary>
        private static OpenXmlConfiguration GetOpenXmlConfiguration<T>(ExcelOperationType optType)
        {
            var dynamicColumns = new List<DynamicExcelColumn>();

            var props = typeof(T).GetProperties();
            foreach (var prop in props)
            {
                var excelAttribute = prop.CustomAttributes.Where(a => a.AttributeType == typeof(ExcelAttribute)).FirstOrDefault();
                if (excelAttribute != null)
                {
                    // 当前列是否忽略
                    var argIgnore = excelAttribute.NamedArguments.Where(arg => arg.MemberInfo.Name.Equals("Ignore")).FirstOrDefault();
                    var isIgnore = argIgnore.TypedValue.Value != null ? Convert.ToBoolean(argIgnore.TypedValue.Value) : false;
                    if (isIgnore) continue;

                    var argName = excelAttribute.NamedArguments.Where(arg => arg.MemberInfo.Name.Equals("Name")).FirstOrDefault();
                    var argIndex = excelAttribute.NamedArguments.Where(arg => arg.MemberInfo.Name.Equals("Index")).FirstOrDefault();
                    var argFormat = excelAttribute.NamedArguments.Where(arg => arg.MemberInfo.Name.Equals("Format")).FirstOrDefault();
                    var argWidth = excelAttribute.NamedArguments.Where(arg => arg.MemberInfo.Name.Equals("Width")).FirstOrDefault();
                    var argOptType = excelAttribute.NamedArguments.Where(arg => arg.MemberInfo.Name.Equals("Type")).FirstOrDefault();

                    var dynamicExcelColumn = new DynamicExcelColumn(prop.Name);
                    dynamicExcelColumn.Name = argName.TypedValue.Value != null ? argName.TypedValue.Value.ToString() : prop.Name;

                    if (argIndex.TypedValue.Value != null)
                    {
                        dynamicExcelColumn.Index = Convert.ToInt32(argIndex.TypedValue.Value);
                    }
                    if (argIgnore.TypedValue.Value != null)
                    {
                        dynamicExcelColumn.Ignore = Convert.ToBoolean(argIgnore.TypedValue.Value);
                    }
                    if (argFormat.TypedValue.Value != null)
                    {
                        dynamicExcelColumn.Format = Convert.ToString(argFormat.TypedValue.Value);
                    }
                    if (argWidth.TypedValue.Value != null)
                    {
                        dynamicExcelColumn.Width = Convert.ToDouble(argWidth.TypedValue.Value);
                    }
                    else
                    {
                        dynamicExcelColumn.Width = 12;
                    }

                    // ExcelOperationType == All || ExcelOperationType == null 时 添加 dynamicExcelColumn
                    if (argOptType.TypedValue.Value != null)
                    {
                        // optType: 导入(Import), 导出(Export)
                        if (optType.ToShort() == Convert.ToInt16(argOptType.TypedValue.Value))
                        {
                            dynamicColumns.Add(dynamicExcelColumn);
                        }
                    }
                    else
                    {
                        dynamicColumns.Add(dynamicExcelColumn);
                    }
                }
                else
                {
                    var dynamicExcelColumn = new DynamicExcelColumn(prop.Name);
                    dynamicExcelColumn.Ignore = true;
                    dynamicColumns.Add(dynamicExcelColumn);
                }
            }

            var config = new OpenXmlConfiguration
            {
                DynamicColumns = dynamicColumns.ToArray()
            };

            return config;
        }
    }
}
