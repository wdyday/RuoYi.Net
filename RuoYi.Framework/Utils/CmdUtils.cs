using System.Diagnostics;

namespace RuoYi.Framework.Utils
{
    public static class CmdUtils
    {
        public static string Run(string fileName, string arguments)
        {
            Process process = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    FileName = fileName, // 如: dotnet
                    Arguments = arguments // 如: --info
                }
            };
            process.Start();
            process.WaitForExit();
            if (process.HasExited)
            {
                return process.StandardOutput.ReadToEnd();
            }

            return string.Empty;
        }
    }
}
