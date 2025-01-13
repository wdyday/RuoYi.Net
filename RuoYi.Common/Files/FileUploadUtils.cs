using Microsoft.AspNetCore.Http;
using RuoYi.Common.Constants;
using RuoYi.Common.Utils;
using RuoYi.Framework;
using RuoYi.Framework.Exceptions;
using RuoYi.Framework.Extensions;
using RuoYi.Framework.Utils;
using System.IO.Compression;

namespace RuoYi.Common.Files
{
    public static class FileUploadUtils
    {

        /// <summary>
        /// 默认大小 50M
        /// </summary>
        public static long DEFAULT_MAX_SIZE = 50 * 1024 * 1024;

        /// <summary>
        /// 默认的文件名最大长度 100
        /// </summary>
        public static int DEFAULT_FILE_NAME_LENGTH = 100;

        /**
         * 默认上传的地址
         */
        private static string DefaultBaseDir = RyApp.RuoYiConfig.Profile ?? "";

        #region Directory

        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static void CreateAppDirectory(string path)
        {
            var subDirectory = GetResourcePhysicalPath(path).Replace(Path.GetFileName(path), "");
            var target = Path.Combine(App.WebHostEnvironment.ContentRootPath, subDirectory);
            CreateDirectory(target);
        }

        public static string GetResourcePhysicalPath(string relativePath)
        {
            relativePath = relativePath.TrimStart('/').TrimStart('\\');
            relativePath = relativePath.ToLower().StartsWith(AppConstants.StaticFileFolder.ToLower())
                ? relativePath
                : Path.Combine(AppConstants.StaticFileFolder, relativePath);

            return GetPhysicalPath(relativePath);
        }

        public static string GetPhysicalPath(string relativePath)
        {
            relativePath = relativePath.TrimStart('/').TrimStart('\\');

            return Path.Combine(App.WebHostEnvironment.ContentRootPath, relativePath);
        }

        #endregion

        #region Upload
        public static async Task<string> UploadAsync(IFormFile file)
        {
            return await UploadAsync(file, DefaultBaseDir, MimeTypeUtils.DEFAULT_ALLOWED_EXTENSION);
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="subDirectory">子路径</param>
        /// <param name="allowedExtension">允许上传的文件类型</param>
        /// <returns>文件相对路径</returns>
        public static async Task<string> UploadAsync(IFormFile file, string subDirectory, string[] allowedExtension)
        {
            int fileNameLength = file.FileName.Length;
            if (fileNameLength > DEFAULT_FILE_NAME_LENGTH)
            {
                throw new ServiceException($"文件名最大长度不能超过{DEFAULT_FILE_NAME_LENGTH}");
            }

            AssertAllowed(file, allowedExtension);

            // 当前文件物理路径
            var physicalPath = GetResourcePhysicalPath(subDirectory);

            string fileName = ExtractFilename(file);
            string filePath = Path.Combine(physicalPath, fileName);

            // 当前文件目录
            string folderPath = Directory.GetParent(filePath)?.FullName;
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            var relativePath = Path.Combine(subDirectory, fileName);
            return relativePath;
        }

        /// <summary>
        /// 编码文件名
        /// </summary>
        public static string ExtractFilename(IFormFile file)
        {
            var baseName = Path.GetFileNameWithoutExtension(file.Name);
            return $"{baseName}_{Guid.NewGuid():N}.{GetExtension(file)}";
        }

        /// <summary>
        /// 文件大小校验
        /// </summary>
        public static void AssertAllowed(IFormFile file, string[] allowedExtension)
        {
            long size = file.Length;
            if (size > DEFAULT_MAX_SIZE)
            {
                throw new ServiceException($"上传的文件大小超出限制的文件大小！<br/>允许的文件最大大小是：{DEFAULT_MAX_SIZE / 1024 / 1024}MB！");
            }

            string fileName = file.FileName;
            string extension = GetExtension(file);
            if (allowedExtension != null && !IsAllowedExtension(extension, allowedExtension))
            {
                var msg = $"文件[{fileName}]后缀[{extension}]不正确，请上传[{string.Join(",", allowedExtension)}]格式";
                throw new ServiceException(msg);
            }
        }

        public static bool IsAllowedExtension(string extension, string[] allowedExtension)
        {
            foreach (string str in allowedExtension)
            {
                if (str.EqualsIgnoreCase(extension))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取文件名的后缀
        /// </summary>
        public static string GetExtension(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName);
            if (StringUtils.IsEmpty(extension))
            {
                extension = MimeTypeUtils.GetExtension(file.ContentType);
            }
            else
            {
                // 去除 后缀前的点, 如: .jpg -> jpg
                extension = extension.Substring(1, extension.Length - 1);
            }
            return extension;
        }

        #endregion

        #region Download

        public static (string fileType, byte[] archiveData, string archiveName) DownloadFiles(string subDirectory)
        {
            var zipName = $"archive-{DateTime.Now.ToYmdHms()}.zip";

            // 文件物理路径
            var physicalPath = GetResourcePhysicalPath(subDirectory);
            var files = Directory.GetFiles(physicalPath).ToList();

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    files.ForEach(filePath =>
                    {
                        var entry = archive.CreateEntry(Path.GetFileName(filePath));

                        using var stream = entry.Open();
                        var buffer = File.ReadAllBytes(filePath);
                        stream.Write(buffer, 0, buffer.Length);
                    });
                }

                return ("application/zip", memoryStream.ToArray(), zipName);
            }
        }

        public static (string fileType, byte[] byteData, string name) DownloadFile(string relativePath)
        {
            var data = ReadFile(relativePath);

            var fName = Path.GetFileName(relativePath);

            return ("application/octet-stream", data, fName);
        }

        public static string SizeConverter(long bytes)
        {
            var fileSize = new decimal(bytes);
            var kilobyte = new decimal(1024);
            var megabyte = new decimal(1024 * 1024);
            var gigabyte = new decimal(1024 * 1024 * 1024);

            return fileSize switch
            {
                var _ when fileSize < kilobyte => $"Less then 1KB",
                var _ when fileSize < megabyte => $"{Math.Round(fileSize / kilobyte, 0, MidpointRounding.AwayFromZero):##,###.##}KB",
                var _ when fileSize < gigabyte => $"{Math.Round(fileSize / megabyte, 2, MidpointRounding.AwayFromZero):##,###.##}MB",
                var _ when fileSize >= gigabyte => $"{Math.Round(fileSize / gigabyte, 2, MidpointRounding.AwayFromZero):##,###.##}GB",
                _ => "n/a",
            };
        }

        public static byte[] ReadFile(string relativePath)
        {
            var physicalPath = GetResourcePhysicalPath(relativePath);
            var data = File.ReadAllBytes(physicalPath);
            return data;
        }

        #endregion
    }
}
