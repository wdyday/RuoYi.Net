using Hardware.Info;
using RuoYi.Data.Models;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RuoYi.System.Services;

public class ServerService : ITransient
{
    private const string NO_DATA = "暂无";

    // https://github.com/Jinjinov/Hardware.Info
    private readonly IHardwareInfo _hardwareInfo;

    public ServerService()
    {
        _hardwareInfo = new HardwareInfo(true);
    }

    public Server GetServerInfo()
    {
        _hardwareInfo.RefreshCPUList();
        _hardwareInfo.RefreshMemoryList();
        _hardwareInfo.RefreshMemoryStatus();
        _hardwareInfo.RefreshDriveList();

        // cpu
        var cpuUsed = Convert.ToDouble(_hardwareInfo.CpuList.FirstOrDefault()?.PercentProcessorTime ?? 0);
        var cpuFree = MathUtils.Round((1 - cpuUsed / 100) * 100, 0);
        var cpu = new Cpu
        {
            CpuNum = Convert.ToInt32(_hardwareInfo.CpuList.FirstOrDefault()?.NumberOfCores ?? 0),
            Total = cpuUsed,
            Free = cpuFree,
            // 其他值暂无法得到
            Used = NO_DATA,
            Sys = NO_DATA
        };

        // 内存
        var memTotal = _hardwareInfo.MemoryStatus.TotalPhysical;
        var memFree = _hardwareInfo.MemoryStatus.AvailablePhysical;
        var mem = new Mem
        {
            Total = CalculateMem(memTotal),
            Used = CalculateMem(memTotal - memFree),
            Free = CalculateMem(memFree),
            Usage = MathUtils.Round(Convert.ToDecimal(memTotal - memFree) / memTotal, 4) * 100
        };

        // 系统相关信息
        var sys = new Sys
        {
            ComputerName = Environment.MachineName,
            ComputerIp = IpUtils.GetHostIpAddr(),
            UserDir = Environment.CurrentDirectory,
            OsName = RuntimeInformation.OSDescription + Environment.OSVersion.ToString(),
            OsArch = RuntimeInformation.OSArchitecture.ToString()
        };

        // Clr
        // 当前系统已运行的毫秒数
        //var tickCount = Environment.TickCount64 > 0 ? Environment.TickCount64 : Environment.TickCount;
        var process = Process.GetCurrentProcess();

        var clrUsed = CalculateClrMem(process.WorkingSet64);
        //var clrFree = CalculateMem(process.MaxWorkingSet - process.WorkingSet64);
        var clrVersion = Environment.Version.ToString();
        var clrHome = GetClrHome();
        var clr = new Clr
        {
            Name = RuntimeInformation.FrameworkDescription.Replace(clrVersion, ""),
            Version = clrVersion,
            Home = clrHome,
            Total = NO_DATA, // 暂无
            Max = process.MaxWorkingSet,
            Used = clrUsed,
            Free = NO_DATA, // 暂无
            //StartTime = GetStartTime(tickCount),
            //RunTime = GetRunTime(tickCount),
            StartTime = process.StartTime.To_YmdHms(),
            RunTime = GetRunTime(DateTime.Now, process.StartTime),
            InputArgs = string.Join(", ", Environment.GetCommandLineArgs()),
            Usage = NO_DATA
        };

        // 磁盘相关信息
        var sysFiles = _hardwareInfo.DriveList.Select(d =>
        {
            var total = d.Size;
            var free = GetDriveFreeSpace(d);
            var used = total - free;
            return new SysFile
            {
                DirName = d.Name,
                SysTypeName = d.Description,
                TypeName = GetFileSystem(d.PartitionList),
                Total = ConvertFileSize(total),
                Free = ConvertFileSize(free),
                Used = ConvertFileSize(used),
                Usage = MathUtils.Round(Convert.ToDecimal(used) / total, 2) * 100
            };
        }).ToList();

        return new Server
        {
            Cpu = cpu,
            Mem = mem,
            Sys = sys,
            Clr = clr,
            SysFiles = sysFiles
        };
    }

    /// <summary>
    /// 计算内存 , 转换成 G
    /// </summary>
    /// <returns></returns>
    private double CalculateMem(ulong mem)
    {
        return MathUtils.Round(mem / (1024 * 1024 * 1024.0), 2);
    }
    private double CalculateClrMem(long mem)
    {
        return MathUtils.Round(mem / (1024 * 1024.0), 2);
    }

    /// <summary>
    /// 系统启动时间
    /// </summary>
    /// <param name="tickCount">当前系统已运行的毫秒数</param>
    /// <returns></returns>
    private string GetStartTime(long tickCount)
    {
        var now = DateTime.Now.ToUnixTimeMilliseconds();
        var startDateTime = (now - tickCount).FromUnixTimeMilliseconds();
        return startDateTime.To_YmdHms();
    }

    /// <summary>
    /// 取 sdk 路径
    /// </summary>
    private string GetClrHome()
    {
        var clrSdks = CmdUtils.Run("dotnet", "--list-sdks");

        // 2.2.110 [C:\Program Files\dotnet\sdk]\r\n7.0.306 [C:\Program Files\dotnet\sdk]\r\n
        var sdks = clrSdks.Split(Environment.NewLine);
        
        var path = sdks.Where(info => info.StartsWith(Environment.Version.Major.ToString())).FirstOrDefault();

        return path ?? "";
    }

    /// <summary>
    /// 系统运行时间
    /// </summary>
    /// <param name="tickCount">当前系统已运行的毫秒数</param>
    /// <returns>X天X小时X分钟</returns>
    private string GetRunTime(long tickCount)
    {
        var daySeconds = 3600 * 24;  // 一天的秒数
        var hourSeconds = 3600; // 一小时的秒数

        var totalSeconds = tickCount / 1000; // 总秒数

        var days = totalSeconds / daySeconds; // 天
        var hours = (totalSeconds - days * daySeconds) / hourSeconds; // 小时
        var minutes = (totalSeconds - days * daySeconds - hours * hourSeconds) / 60; // 分钟
        //var seconds = totalSeconds % 60;

        return $"{days}天{hours}小时{minutes}分钟";
    }

    /// <summary>
    /// 系统运行时间
    /// </summary>
    /// <param name="tickCount">当前系统已运行的毫秒数</param>
    /// <returns>X天X小时X分钟</returns>
    private string GetRunTime(DateTime endTime, DateTime startTime)
    {
        var timeSpan = endTime - startTime;
        return $"{timeSpan.Days}天{timeSpan.Hours}小时{timeSpan.Minutes}分钟";

        //long nd = 1000 * 24 * 60 * 60;
        //long nh = 1000 * 60 * 60;
        //long nm = 1000 * 60;
        //// long ns = 1000;
        //// 获得两个时间的毫秒时间差异
        //long diff = (endTime - startTime).TotalMilliseconds;
        //// 计算差多少天
        //long day = diff / nd;
        //// 计算差多少小时
        //long hour = diff % nd / nh;
        //// 计算差多少分钟
        //long min = diff % nd % nh / nm;
        //// 计算差多少秒//输出结果
        //// long sec = diff % nd % nh % nm / ns;
        //return day + "天" + hour + "小时" + min + "分钟";
    }

    /// <summary>
    /// 字节转换
    /// </summary>
    /// <param name="size">字节大小</param>
    /// <returns>转换后值</returns>
    private string ConvertFileSize(ulong size)
    {
        ulong kb = 1024;
        ulong mb = kb * 1024;
        ulong gb = mb * 1024;
        if (size >= gb)
        {
            return string.Format("{0:#} GB", (float)size / gb);
        }
        else if (size >= mb)
        {
            float f = (float)size / mb;
            return string.Format(f > 100 ? "{0:#} MB" : "{0:##} MB", f);
        }
        else if (size >= kb)
        {
            float f = (float)size / kb;
            return string.Format(f > 100 ? "{0:#} KB" : "{0:##} KB", f);
        }
        else
        {
            return string.Format("{0} B", size);
        }
    }

    /// <summary>
    /// 取系统文件类型(NTFS/FAT)
    /// </summary>
    private string GetFileSystem(List<Partition> partitions)
    {
        // 文件类型(NTFS/FAT)
        string? fileSystem = "";
        foreach (var partition in partitions)
        {
            fileSystem = partition.VolumeList.Where(v => !string.IsNullOrEmpty(v.FileSystem)).FirstOrDefault()?.FileSystem;
            if (!string.IsNullOrEmpty(fileSystem))
                break;
        }
        return fileSystem ?? "";
    }

    /// <summary>
    /// 取磁盘剩余空间
    /// </summary>
    private ulong GetDriveFreeSpace(Drive drive)
    {
        long freeSpace = 0;
        foreach (var partition in drive.PartitionList)
        {
            freeSpace += partition.VolumeList.Sum(v => (long)v.FreeSpace);
        }
        return Convert.ToUInt64(freeSpace);
    }
}
