using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SudoItApi.Controllers
{
    #region 内存使用率模块
    public class GetRAMUsage
    {
        public string GetCPUUsage()
        {
            PerformanceCounter cpuCounter;
            //PerformanceCounter ramCounter;
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            //ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            //PerformanceCounter diskCounter = new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", "_Total");
            return cpuCounter.NextValue() + "%";
        }
        public string FullRAM;
        public string UsedRAM;
        public string FreeRAM;
        public void Get()
        {
            FullRAM = FormatSize(GetTotalPhys());
            UsedRAM = FormatSize(GetUsedPhys());
            FreeRAM = FormatSize(GetAvailPhys());
        }

        #region 获得内存信息API
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GlobalMemoryStatusEx(ref MEMORY_INFO mi);

        //定义内存的信息结构
        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_INFO
        {
            public uint dwLength; //当前结构体大小
            public uint dwMemoryLoad; //当前内存使用率
            public ulong ullTotalPhys; //总计物理内存大小
            public ulong ullAvailPhys; //可用物理内存大小
            public ulong ullTotalPageFile; //总计交换文件大小
            public ulong ullAvailPageFile; //总计交换文件大小
            public ulong ullTotalVirtual; //总计虚拟内存大小
            public ulong ullAvailVirtual; //可用虚拟内存大小
            public ulong ullAvailExtendedVirtual; //保留 这个值始终为0
        }
        #endregion

        #region 格式化容量大小
        /// <summary>
        /// 格式化容量大小
        /// </summary>
        /// <param name="size">容量（B）</param>
        /// <returns>已格式化的容量</returns>
        private static string FormatSize(double size)
        {
            double d = (double)size;
            int i = 0;
            while ((d > 1024) && (i < 5))
            {
                d /= 1024;
                i++;
            }
            string[] unit = { "B", "KB", "MB", "GB", "TB" };
            return (string.Format("{0} {1}", Math.Round(d, 2), unit[i]));
        }
        #endregion

        #region 获得当前内存使用情况
        /// <summary>
        /// 获得当前内存使用情况
        /// </summary>
        /// <returns></returns>
        public static MEMORY_INFO GetMemoryStatus()
        {
            MEMORY_INFO mi = new MEMORY_INFO();
            mi.dwLength = (uint)System.Runtime.InteropServices.Marshal.SizeOf(mi);
            GlobalMemoryStatusEx(ref mi);
            return mi;
        }
        #endregion

        #region 获得当前可用物理内存大小
        /// <summary>
        /// 获得当前可用物理内存大小
        /// </summary>
        /// <returns>当前可用物理内存（B）</returns>
        public static ulong GetAvailPhys()
        {
            MEMORY_INFO mi = GetMemoryStatus();
            return mi.ullAvailPhys;
        }
        #endregion

        #region 获得当前已使用的内存大小
        /// <summary>
        /// 获得当前已使用的内存大小
        /// </summary>
        /// <returns>已使用的内存大小（B）</returns>
        public static ulong GetUsedPhys()
        {
            MEMORY_INFO mi = GetMemoryStatus();
            return (mi.ullTotalPhys - mi.ullAvailPhys);
        }
        #endregion

        #region 获得当前总计物理内存大小
        /// <summary>
        /// 获得当前总计物理内存大小
        /// </summary>
        /// <returns&amp;gt;总计物理内存大小（B）&amp;lt;/returns&amp;gt;
        public static ulong GetTotalPhys()
        {
            MEMORY_INFO mi = GetMemoryStatus();
            return mi.ullTotalPhys;
        }
        #endregion
    }
    #endregion
    #region 状态机
    /// <summary>
    /// 服务端状态反馈器
    /// </summary>
    [ApiController]
    [Route("SudoIt/[controller]/[action]")]
    public class ServiceController : ControllerBase
    {
        /// <summary>
        /// 测试服务端连通性
        /// </summary>
        /// <returns>词典</returns>
        [HttpGet]
        public ActionResult<string> Test()
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            Log.SaveLog(ip + " got your server status.");
            return "{\"status\":\"服务在线\"}";
        }
        /// <summary>
        /// 获取操作系统,机器名称,架构,用户名,内存使用量,CPU使用率等信息
        /// </summary>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> Status(string Password)
        {
            if (!SetAndAuth.Auth(Password))
            {
                string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                Log.SaveLog(ip + " 尝试获取本机信息 ,但是他/她输入了错误的密码");
                HttpContext.Response.StatusCode = 403;
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            string MacName = Environment.MachineName;
            string OSName = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD)) OSName = "FreeBSD";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) OSName = "Linux";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) OSName = "MacOS";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) OSName = "Windows";
            if (OSName == "") OSName = "Unknown";
            string OSBit = "32Bit";
            if (Environment.Is64BitOperatingSystem) OSBit = "64Bit";
            string UserName = Environment.UserName;
            GetRAMUsage getRAMUsage = new GetRAMUsage();
            getRAMUsage.Get();
            string CPUUsage = getRAMUsage.GetCPUUsage();
            string info = "{\n\"MacName\":\"" + MacName + "\",\n\"OS\":\"" + OSName + "\",\n\"OSBit\":\"" + OSBit + "\",\n\"UserName\":\"" + UserName + "\",\n\"FreeRAM\":\"" + getRAMUsage.FreeRAM + "\",\n\"CPUUsage\":\"" + CPUUsage.ToString() + "\"\n}";
            return info;
        }
    }
    #endregion
}
