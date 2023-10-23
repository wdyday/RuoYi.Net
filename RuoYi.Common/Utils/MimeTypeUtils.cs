namespace RuoYi.Common.Utils
{
    public static class MimeTypeUtils
    {
        public const string IMAGE_PNG = "image/png";

        public const string IMAGE_JPG = "image/jpg";

        public const string IMAGE_JPEG = "image/jpeg";

        public const string IMAGE_BMP = "image/bmp";

        public const string IMAGE_GIF = "image/gif";

        public static string[] IMAGE_EXTENSION = { "bmp", "gif", "jpg", "jpeg", "png" };

        public static string[] FLASH_EXTENSION = { "swf", "flv" };

        public static string[] MEDIA_EXTENSION = { "swf", "flv", "mp3", "wav", "wma", "wmv", "mid", "avi", "mpg", "asf", "rm", "rmvb" };

        public static string[] VIDEO_EXTENSION = { "mp4", "avi", "rmvb" };

        public static string[] DEFAULT_ALLOWED_EXTENSION = {
            // 图片
            "bmp", "gif", "jpg", "jpeg", "png",
            // word excel powerpoint
            "doc", "docx", "xls", "xlsx", "ppt", "pptx", "html", "htm", "txt",
            // 压缩文件
            "rar", "zip", "gz", "bz2",
            // 视频格式
            "mp4", "avi", "rmvb",
            // pdf
            "pdf"
        };

        public static string GetExtension(string prefix)
        {
            return prefix switch
            {
                IMAGE_PNG => "png",
                IMAGE_JPG => "jpg",
                IMAGE_JPEG => "jpeg",
                IMAGE_BMP => "bmp",
                IMAGE_GIF => "gif",
                _ => "",
            };
        }
    }
}
