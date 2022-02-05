using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace SudoItApi.Controllers
{
    #region 内存使用率模块
    /// <summary>
    /// 获取使用率
    /// </summary>
    public class GetRAMUsage
    {
        /// <summary>
        /// 获取CPU使用率
        /// </summary>
        /// <returns></returns>
        public string GetCPUUsage()
        {
            PerformanceCounter cpuCounter;
            //PerformanceCounter ramCounter;
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time","_Total");
            cpuCounter.NextValue();//获取下一结果
            //ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            //PerformanceCounter diskCounter = new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", "_Total");
            return cpuCounter.NextValue() + "%";
            //这里注意!一定要获取两次NextValue
            //因为NextValue指的是上一次和这一次检查的平均值,因此第一次结果为0%
        }
        /// <summary>
        /// 全部内存大小
        /// </summary>
        public string FullRAM;
        /// <summary>
        /// 已使用
        /// </summary>
        public string UsedRAM;
        /// <summary>
        /// 空闲内存
        /// </summary>
        public string FreeRAM;
        /// <summary>
        /// CPU使用率
        /// </summary>
        public string CPUUsage;
        /// <summary>
        /// 获取使用情况
        /// </summary>
        public void Get()
        {
            FullRAM = FormatSize(GetTotalPhys());
            UsedRAM = FormatSize(GetUsedPhys());
            FreeRAM = FormatSize(GetAvailPhys());
            CPUUsage = GetCPUUsage();
        }

        #region 获得内存信息API
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mi"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GlobalMemoryStatusEx(ref MEMORY_INFO mi);

        //定义内存的信息结构
        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_INFO
        {
            /// <summary>
            /// 当前结构体大小
            /// </summary>
            public uint dwLength; //当前结构体大小
            /// <summary>
            /// 当前内存使用率
            /// </summary>
            public uint dwMemoryLoad; //当前内存使用率
            /// <summary>
            /// 总计物理内存大小
            /// </summary>
            public ulong ullTotalPhys; //总计物理内存大小
            /// <summary>
            /// 可用物理内存大小
            /// </summary>
            public ulong ullAvailPhys; //可用物理内存大小
            /// <summary>
            /// 总计交换文件大小
            /// </summary>
            public ulong ullTotalPageFile; //总计交换文件大小
            /// <summary>
            /// 总计交换文件大小
            /// </summary>
            public ulong ullAvailPageFile; //总计交换文件大小
            /// <summary>
            /// 总计虚拟内存大小
            /// </summary>
            public ulong ullTotalVirtual; //总计虚拟内存大小
            /// <summary>
            /// 可用虚拟内存大小
            /// </summary>
            public ulong ullAvailVirtual; //可用虚拟内存大小
            /// <summary>
            /// 0
            /// </summary>
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
            Log.SaveLog(ip + " 测试了服务连通性");//获取IP并加入日志
            string Result = "{\"status\":\"服务在线\"}";//返回结果
            return Plugins.ProcessResult("Test", Result);
        }
        /// <summary>
        /// 获取操作系统,机器名称,架构,用户名,内存使用量,CPU使用率等信息
        /// </summary>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> Status(string Password)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password, ip))//通过Auth方法验证密码,!表示"不",即为:如果输入的密码不正确
            {
                HttpContext.Response.StatusCode = 403;//返回403状态码
                Log.SaveLog(ip + " 尝试获取本机信息 ,但是他/她输入了错误的密码");//记录异常行为到日志
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            try
            {
                string MacName = Environment.MachineName;
                string OSName = "";//初始化OSName,如果去除"",即为null
                                   //使用if判断表达式可以有效减少代码量
                                   //if (条件表达式.true) 语句;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD)) OSName = "FreeBSD";
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) OSName = "Linux";
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) OSName = "MacOS";
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) OSName = "Windows";
                if (OSName == "") OSName = "Unknown";
                //如果OSName仍未被赋值,说明该系统未被IsOSPlatform收录
                string OSBit = "32Bit";//初始化架构为32位
                if (Environment.Is64BitOperatingSystem) OSBit = "64Bit";
                //如果是64位操作系统,设置OSBit为64位
                string UserName = Environment.UserName;
                //初始化用户名
                GetRAMUsage getRAMUsage = new GetRAMUsage();//构建新的GetRAMUsage类
                getRAMUsage.Get();//获取RAM使用率
                string CPUUsage = getRAMUsage.CPUUsage;
                //获取CPU使用率
                string info = "{\n\"MacName\":\"" + MacName + "\",\n\"OS\":\"" + OSName + "\",\n\"OSBit\":\"" + OSBit + "\",\n\"UserName\":\"" + UserName + "\",\n\"FreeRAM\":\"" + getRAMUsage.FreeRAM + "\",\n\"FullRAM\":\"" + getRAMUsage.FullRAM + "\",\n\"UsedRAM\":\"" + getRAMUsage.UsedRAM + "\",\n\"CPUUsage\":\"" + CPUUsage.ToString() + "\"\n}";
                Log.SaveLog(ip + "获取了设备信息");
                return Plugins.ProcessResult("Status", info);
                //返回词典
            }
            catch(Exception ex)
            {
                Log.SaveLog(ip + "在获取设备状态时触发异常" + ex.ToString());
                return "{\"status\":\"Error\",\"msg\":\"不支持的体系架构或操作系统.Unsupported platform or OS.\",\"exception\":\"" + ex.Message + "\"}";
            }
        }
    }
    #endregion
}
