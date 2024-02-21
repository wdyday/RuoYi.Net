using ICSharpCode.SharpZipLib.Zip;
using RuoYi.Framework.Logging;
using System.Text;

namespace RuoYi.Framework.Utils
{
    public static class ZipUtils
    {
        /// <summary>
        /// 添加文本到zip
        /// </summary>
        /// <param name="stream">zip流</param>
        /// <param name="fileName">文件名(路径+名字)</param>
        /// <param name="text">文本</param>
        public static void PutTextEntry(ZipOutputStream stream, string fileName, string text)
        {
            stream.PutNextEntry(new ZipEntry(fileName));

            byte[] bytes = Encoding.UTF8.GetBytes(text);
            stream.Write(bytes, 0, bytes.Length);

            stream.CloseEntry();
        }

        /// <summary>
        /// 关闭 zip流
        /// </summary>
        /// <param name="stream"></param>
        public static void Close(ZipOutputStream stream)
        {
            if (stream != null)
            {
                try
                {
                    stream.IsStreamOwner = false;
                    stream.Finish();
                    stream.Close();
                }
                catch (IOException e)
                {
                    Log.Error("Zip close error", e);
                }
            }
        }
    }
}
