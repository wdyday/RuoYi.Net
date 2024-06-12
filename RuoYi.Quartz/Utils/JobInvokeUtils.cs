using System.Reflection;
using System.Text.RegularExpressions;

namespace RuoYi.Quartz.Utils
{
    public class JobInvokeUtils
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="sysJob">系统任务</param>
        public static void InvokeMethod(SysJobDto sysJob)
        {
            string invokeTarget = sysJob.InvokeTarget!;
            string taskName = GetTaskName(invokeTarget);
            string methodName = GetMethodName(invokeTarget);
            List<object?>? methodParams = GetMethodParams(invokeTarget);

            Type? target = AssemblyUtils.GetTaskAttributeClassType(taskName);
            if(target == null) { return; }

            InvokeMethod(target, methodName, methodParams?.ToArray());
        }

        public static void InvokeMethod(Type target, string methodName, object?[]? methodParams)
        {
            MethodInfo? openMethodInfo = target.GetMethod(methodName);
            if(openMethodInfo == null) { return; }

            var instance = ReflectUtils.CreateInstance(target);
            openMethodInfo.Invoke(instance, methodParams);
        }

        #region private methods
        // 取 任务名
        private static string GetTaskName(string invokeTarget)
        {
            string beanName = StringUtils.SubstringBefore(invokeTarget, "(")!;
            return StringUtils.SubstringBeforeLast(beanName, ".");
        }

        // 取方法名
        private static string GetMethodName(string invokeTarget)
        {
            string methodName = StringUtils.SubstringBefore(invokeTarget, "(")!;
            return StringUtils.SubstringAfterLast(methodName, ".");
        }

        // 获取method方法参数相关列表
        private static List<object?>? GetMethodParams(string invokeTarget)
        {
            string methodStr = StringUtils.SubstringBetween(invokeTarget, "(", ")")!;
            if (StringUtils.IsEmpty(methodStr))
            {
                return null;
            }
            string[] methodParams = Regex.Split(methodStr, ",(?=([^\"']*[\"'][^\"']*[\"'])*[^\"']*$)");
            //List<object[]> classs = new List<object[]>();
            List<object?> classs = new List<object?>();
            for (int i = 0; i < methodParams.Length; i++)
            {
                string str = StringUtils.TrimToEmpty(methodParams[i]);
                // string字符串类型，以'或"开头
                if (StringUtils.StartsWithAny(str, new string[] { "'", "\"" }))
                {
                    //classs.Add(new object[] { StringUtils.Substring(str, 1, str.Length - 1)!, typeof(string) });
                    classs.Add(StringUtils.Substring(str, 1, str.Length - 1)!);
                }
                // boolean布尔类型，等于true或者false
                else if ("true".EqualsIgnoreCase(str) || "false".EqualsIgnoreCase(str))
                {
                    //classs.Add(new object[] { Convert.ToBoolean(str), typeof(bool) });
                    classs.Add(Convert.ToBoolean(str));
                }
                // long长整形，以L结尾
                else if (str.EndsWith("L") || str.EndsWith("l"))
                {
                    //classs.Add(new object[] { Convert.ToInt64(StringUtils.Substring(str, 0, str.Length - 1)), typeof(long) });
                    classs.Add(Convert.ToInt64(StringUtils.Substring(str, 0, str.Length - 1)));
                }
                // double浮点类型，以D结尾
                else if (str.EndsWith("D") || str.EndsWith("d"))
                {
                    //classs.Add(new object[] { Convert.ToDouble(StringUtils.Substring(str, 0, str.Length - 1)), typeof(double) });
                    classs.Add(Convert.ToDouble(StringUtils.Substring(str, 0, str.Length - 1)));
                }
                // decimal类型，以D结尾
                else if (str.EndsWith("M") || str.EndsWith("m"))
                {
                    //classs.Add(new object[] { Convert.ToDouble(StringUtils.Substring(str, 0, str.Length - 1)), typeof(decimal) });
                    classs.Add(Convert.ToDouble(StringUtils.Substring(str, 0, str.Length - 1)));
                }
                // 其他类型归类为整形
                else
                {
                    //classs.Add(new object[] { Convert.ToInt32(str), typeof(int) });
                    classs.Add(Convert.ToInt32(str));
                }
            }

            return classs;
        }

        // 校验是否为为有效class名
        public static bool IsValidClassName(string invokeTarget)
        {
            return StringUtils.CountMatches(invokeTarget, ".") > 1;
        }
        #endregion
    }
}
