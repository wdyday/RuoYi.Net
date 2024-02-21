using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.RegularExpressions;

namespace RuoYi.Framework.Utils
{
    public static class IpUtils
    {
        public static string REGX_0_255 = "(25[0-5]|2[0-4]\\d|1\\d{2}|[1-9]\\d|\\d)";
        // 匹配 ip
        public static string REGX_IP = "((" + REGX_0_255 + "\\.){3}" + REGX_0_255 + ")";
        public static string REGX_IP_WILDCARD = "(((\\*\\.){3}\\*)|(" + REGX_0_255 + "(\\.\\*){3})|(" + REGX_0_255 + "\\." + REGX_0_255 + ")(\\.\\*){2}" + "|((" + REGX_0_255 + "\\.){3}\\*))";
        // 匹配网段
        public static string REGX_IP_SEG = "(" + REGX_IP + "\\-" + REGX_IP + ")";

        /// <summary>
        /// 检查是否为内部IP地址
        /// </summary>
        /// <param name="ipv4"></param>
        /// <returns></returns>
        public static bool IsInternalIp(string ipv4)
        {
            if (IPAddress.TryParse(ipv4, out var ip))
            {
                byte[] ipBytes = ip.GetAddressBytes();
                if (ipBytes[0] == 10) return true;
                if (ipBytes[0] == 172 && ipBytes[1] >= 16 && ipBytes[1] <= 31) return true;
                if (ipBytes[0] == 192 && ipBytes[1] == 168) return true;
            }
            return false;
        }

        /// <summary>
        /// 校验ip是否符合过滤串规则
        /// </summary>
        /// <param name="filter">过滤IP列表,支持后缀'*'通配,支持网段如:`10.10.10.1-10.10.10.99`</param>
        /// <param name="ip">校验IP地址</param>
        /// <returns></returns>
        public static bool IsMatchedIp(string? filter, string? ip)
        {
            if (string.IsNullOrEmpty(filter) || string.IsNullOrEmpty(ip))
            {
                return false;
            }
            string[] ips = filter.Split(";");
            foreach (string iStr in ips)
            {
                if (IsIP(iStr) && iStr.Equals(ip))
                {
                    return true;
                }
                else if (IsIpWildCard(iStr) && IpIsInWildCardNoCheck(iStr, ip))
                {
                    return true;
                }
                else if (IsIPSegment(iStr) && IpIsInNetNoCheck(iStr, ip))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否为IP
        /// </summary>
        public static bool IsIP(string ip)
        {
            Match m = Regex.Match(ip, REGX_IP);
            return !string.IsNullOrWhiteSpace(ip) && m.Success;
        }

        /// <summary>
        /// 是否为IP，或 *为间隔的通配符地址
        /// </summary>
        public static bool IsIpWildCard(string ip)
        {
            Match m = Regex.Match(ip, REGX_IP_WILDCARD);
            return !string.IsNullOrWhiteSpace(ip) && m.Success;
        }

        /// <summary>
        /// 检测参数是否在ip通配符里
        /// </summary>
        public static bool IpIsInWildCardNoCheck(string ipWildCard, string ip)
        {
            string[] s1 = ipWildCard.Split("\\.");
            string[] s2 = ip.Split("\\.");
            bool isMatchedSeg = true;
            for (int i = 0; i < s1.Length && !s1[i].Equals("*"); i++)
            {
                if (!s1[i].Equals(s2[i]))
                {
                    isMatchedSeg = false;
                    break;
                }
            }
            return isMatchedSeg;
        }

        /// <summary>
        /// 是否为特定格式如:“10.10.10.1-10.10.10.99”的ip段字符串
        /// </summary>
        public static bool IsIPSegment(string ipSeg)
        {
            Match m = Regex.Match(ipSeg, REGX_IP_SEG);
            return !string.IsNullOrWhiteSpace(ipSeg) && m.Success;
        }

        /// <summary>
        /// 判断ip是否在指定网段中
        /// </summary>
        public static bool IpIsInNetNoCheck(string iparea, string ip)
        {
            int idx = iparea.IndexOf('-');
            string[] sips = iparea.Substring(0, idx).Split("\\.");
            string[] sipe = iparea.Substring(idx + 1).Split("\\.");
            string[] sipt = ip.Split("\\.");
            long ips = 0L, ipe = 0L, ipt = 0L;
            for (int i = 0; i < 4; ++i)
            {
                ips = ips << 8 | Convert.ToInt32(sips[i]);
                ipe = ipe << 8 | Convert.ToInt32(sipe[i]);
                ipt = ipt << 8 | Convert.ToInt32(sipt[i]);
            }
            if (ips > ipe)
            {
                long t = ips;
                ips = ipe;
                ipe = t;
            }
            return ips <= ipt && ipt <= ipe;
        }

        public static string GetIpAddr()
        {
            return App.HttpContext.GetRemoteIpAddressToIPv4();
        }

        public static string GetHostIpAddr()
        {
            return App.HttpContext.GetLocalIpAddressToIPv4();
        }
    }
}
