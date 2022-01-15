using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
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
    #region 命令系统
    /// <summary>
    /// 服务器命令控制器
    /// </summary>
    [ApiController]
    [Route("SudoIt/[controller]/[action]")]
    public class CommandController : ControllerBase
    {
        /// <summary>
        /// 执行命令并获取返回值
        /// </summary>
        /// <param name="Command">命令文本</param>
        /// <param name="Password">密码</param>
        /// <returns>词典(结果或错误)</returns>
        [HttpGet]
        public ActionResult<string> ExecuteCommand(string Command, string Password)
        {
            if (SetAndAuth.Auth(Password))
            {
                string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                Log.SaveLog(ip + " execated \"" + Command + "\"");
                string result = Cmd.RunCmd(Command, true);
                return result;
            }
            else
            {
                string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                Log.SaveLog(ip + " 尝试执行命令 \"" + Command + "\" ,但是他/她输入了错误的密码");
                HttpContext.Response.StatusCode = 403;
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
        }
        /// <summary>
        /// 执行命令(不等待返回值)
        /// </summary>
        /// <param name="Command">命令文本</param>
        /// <param name="Password">密码</param>
        /// <returns>词典(成功或错误)</returns>
        [HttpGet]
        public ActionResult<string> SafeExecute(string Command, string Password)
        {
            if (SetAndAuth.Auth(Password))
            {
                string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                Log.SaveLog(ip + " SafeExecated \"" + Command + "\"");
                Cmd.RunCmd(Command, false);
                return "{\"status\":\"OK\",\"msg\":\"Done.\"}";
            }
            else
            {
                string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                Log.SaveLog(ip + " 尝试执行命令 \"" + Command + "\" ,但是他/她输入了错误的密码");
                HttpContext.Response.StatusCode = 403;
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
        }
        /// <summary>
        /// 延时执行命令(不等待返回值)
        /// </summary>
        /// <param name="Command">命令文本</param>
        /// <param name="Password">密码</param>
        /// <param name="DelayTime">延时时长(毫秒)</param>
        /// <returns>词典(成功或错误)</returns>
        [HttpGet]
        public ActionResult<string> TimeDelayExecute(string Command, string Password, string DelayTime)
        {
            if (SetAndAuth.Auth(Password))
            {
                string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                Log.SaveLog(ip + " SafeExecated \"" + Command + "\"");
                Cmd.DelayRunCmd(Command, Convert.ToInt32(DelayTime));
                return "{\"status\":\"OK\",\"msg\":\"Done.\"}";
            }
            else
            {
                string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                Log.SaveLog(ip + " 尝试执行命令 \"" + Command + "\" ,但是他/她输入了错误的密码");
                HttpContext.Response.StatusCode = 403;
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
        }
    }
    #endregion
    #region 文件系统
    /// <summary>
    /// 文件系统控制器
    /// </summary>
    [ApiController]
    [Route("SudoIt/[controller]/[action]")]
    public class FileSystemController : ControllerBase
    {
        #region 文件列表模块
        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="Path">路径</param>
        /// <param name="Password">密码</param>
        /// <returns>词典</returns>
        [HttpGet]
        public ActionResult<string> GetList(string Path, string Password)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password))
            {
                Log.SaveLog(ip + " 尝试获取该路径的文件列表: \"" + Path + "\" ,但是他/她输入了错误的密码");
                HttpContext.Response.StatusCode = 403;
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            try
            {
                string[] Files = Directory.GetFiles(Path);
                string[] Directories = Directory.GetDirectories(Path);
                string Dictionary = "{\n\"..\":\"Back\",\n";
                for (int i = 0; i < Files.Length; i++)
                {
                    string fileNow = Files[i];
                    Dictionary = Dictionary + "\"" + fileNow.Split("\\")[^1].Split("/")[^1] + "\":\"File\",\n";
                }
                for (int i = 0; i < Directories.Length; i++)
                {
                    string fileNow = Directories[i];
                    Dictionary = Dictionary + "\"" + fileNow.Split("\\")[^1].Split("/")[^1] + "\":\"Direction\",\n";
                }
                Dictionary = Dictionary[0..^2];
                Dictionary += "\n}";
                Log.SaveLog(ip + " 获取了该路径的文件列表: \"" + Path + "\"");
                return Dictionary;
            }
            catch (Exception ex)
            {
                Log.SaveLog(ip + " 尝试获取该路径的文件列表: \"" + Path + "\",但是触发了异常:" + ex.ToString());
                return "{\"status\":\"Error\",\"msg\":\"无法列出该路径索引.请检查路径名称是否正确,是否以正确的用户账户运行服务端及是否具有该路径的访问权限.\"}";
            }
        }
        #endregion
        #region 文件模块
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> Mkfile(string Path, string Password)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password))
            {
                Log.SaveLog(ip + " 尝试创建该路径的文件: \"" + Path + "\" ,但是他/她输入了错误的密码");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            try
            {
                System.IO.File.Create(Path);
                Log.SaveLog(ip + " 创建了以下路径的文件:" + Path);
                return "{\"status\":\"Success.\"}";
            }
            catch (Exception ex)
            {
                Log.SaveLog("在处理位于" + ip + "的创建文件请求时发生了异常:" + ex.ToString());
                return "{\"status\":\"Error\",\"msg\":\"无法创建该文件.请检查路径名称是否正确,是否以正确的用户账户运行服务端及是否具有该路径的访问权限.\"}"; ;
            }
        }
        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="FromPath">起始路径</param>
        /// <param name="ToPath">目的路径</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> MoveFile(string FromPath, string ToPath, string Password)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password))
            {
                Log.SaveLog(ip + " 尝试将该路径的文件: \"" + FromPath + "\"移动到\"" + ToPath + "\" ,但是他/她输入了错误的密码");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            try
            {
                System.IO.File.Move(FromPath, ToPath);
                Log.SaveLog(ip + " 尝试将该路径的文件: \"" + FromPath + "\"移动到\"" + ToPath + "\".");
                return "{\"status\":\"Success.\"}";
            }
            catch (Exception ex)
            {
                Log.SaveLog("在处理位于" + ip + "的移动文件请求时发生了异常:" + ex.ToString());
                return "{\"status\":\"Error\",\"msg\":\"无法移动该文件.请检查路径名称是否正确,是否以正确的用户账户运行服务端及是否具有该路径的访问权限.\"}"; ;
            }
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> DeleteFile(string Path, string Password)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password))
            {
                Log.SaveLog(ip + " 尝试删除该路径的文件: \"" + Path + "\" ,但是他/她输入了错误的密码");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            try
            {
                System.IO.File.Delete(Path);
                Log.SaveLog(ip + " 删除了以下路径的文件:" + Path);
                return "{\"status\":\"Success.\"}";
            }
            catch (Exception ex)
            {
                Log.SaveLog("在处理位于" + ip + "的删除文件请求时发生了异常:" + ex.ToString());
                return "{\"status\":\"Error\",\"msg\":\"无法删除该文件.请检查路径名称是否正确,是否以正确的用户账户运行服务端及是否具有该路径的访问权限.\"}"; ;
            }
        }
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="FromPath">起始路径</param>
        /// <param name="ToPath">目的路径</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> CopyFile(string FromPath, string ToPath, string Password)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password))
            {
                Log.SaveLog(ip + " 尝试将该路径的文件: \"" + FromPath + "\"复制到\"" + ToPath + "\" ,但是他/她输入了错误的密码");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            try
            {
                System.IO.File.Copy(FromPath, ToPath);
                Log.SaveLog(ip + " 尝试将该路径的文件: \"" + FromPath + "\"复制到\"" + ToPath + "\".");
                return "{\"status\":\"Success.\"}";
            }
            catch (Exception ex)
            {
                Log.SaveLog("在处理位于" + ip + "的复制文件请求时发生了异常:" + ex.ToString());
                return "{\"status\":\"Error\",\"msg\":\"无法复制该文件.请检查路径名称是否正确,是否以正确的用户账户运行服务端及是否具有该路径的访问权限.\"}"; ;
            }
        }
        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> GetFileInfo(string Path, string Password)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password))
            {
                Log.SaveLog(ip + " 尝试获取该路径的文件信息: \"" + Path + "\" ,但是他/她输入了错误的密码");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            try
            {
                Log.SaveLog(ip + " 获取了以下路径的文件信息:" + Path);
                string CreationTime = System.IO.File.GetCreationTime(Path).ToString("yyyy-MM-dd-HH:mm:ss");
                string LastAccessTime = System.IO.File.GetLastAccessTime(Path).ToString("yyyy-MM-dd-HH:mm:ss");
                string LastWriteTime = System.IO.File.GetLastWriteTime(Path).ToString("yyyy-MM-dd-HH:mm:ss");
                return "{\"CreationTime\":\"" + CreationTime + "\",\"LastAccess\":\"" + LastAccessTime + "\",\"LastWrite\":\"" + LastWriteTime + "\"}";
            }
            catch (Exception ex)
            {
                Log.SaveLog("在处理位于" + ip + "的获取文件信息请求时发生了异常:" + ex.ToString());
                return "{\"status\":\"Error\",\"msg\":\"无法获取该文件信息.请检查路径名称是否正确,是否以正确的用户账户运行服务端及是否具有该路径的访问权限.\"}"; ;
            }
        }
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> ReadFile(string Path, string Password)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password))
            {
                Log.SaveLog(ip + " 尝试读取该路径的文件: \"" + Path + "\" ,但是他/她输入了错误的密码");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            try
            {
                Log.SaveLog(ip + " 读取了以下路径的文件:" + Path);
                return System.IO.File.ReadAllText(Path);
            }
            catch (Exception ex)
            {
                Log.SaveLog("在处理位于" + ip + "的读取文件请求时发生了异常:" + ex.ToString());
                return "{\"status\":\"Error\",\"msg\":\"无法读取该文件.请检查路径名称是否正确,是否以正确的用户账户运行服务端及是否具有该路径的访问权限.\"}"; ;
            }
        }
        /// <summary>
        /// 写入并覆盖文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="Password">密码</param>
        /// <param name="Text">文本</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> WriteFile(string Path, string Password, string Text)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password))
            {
                Log.SaveLog(ip + " 尝试覆盖该路径的文件: \"" + Path + "\" ,但是他/她输入了错误的密码");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            try
            {
                Log.SaveLog(ip + " 覆盖了以下路径的文件:" + Path);
                System.IO.File.WriteAllText(Path, Text);
                return "{\"status\":\"Success.\"}";
            }
            catch (Exception ex)
            {
                Log.SaveLog("在处理位于" + ip + "的覆盖文件请求时发生了异常:" + ex.ToString());
                return "{\"status\":\"Error\",\"msg\":\"无法覆盖该文件.请检查路径名称是否正确,是否以正确的用户账户运行服务端及是否具有该路径的访问权限.\"}"; ;
            }
        }
        /// <summary>
        /// 追加到文件末尾
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="Password">密码</param>
        /// <param name="Text">文本</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> WriteToFile(string Path, string Password, string Text)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password))
            {
                Log.SaveLog(ip + " 尝试写入到该路径的文件: \"" + Path + "\" ,但是他/她输入了错误的密码");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            try
            {
                Log.SaveLog(ip + " 写入了以下路径的文件:" + Path);
                System.IO.File.AppendAllText(Path, Text);
                return "{\"status\":\"Success.\"}";
            }
            catch (Exception ex)
            {
                Log.SaveLog("在处理位于" + ip + "的写入文件请求时发生了异常:" + ex.ToString());
                return "{\"status\":\"Error\",\"msg\":\"无法写入该文件.请检查路径名称是否正确,是否以正确的用户账户运行服务端及是否具有该路径的访问权限.\"}"; ;
            }
        }
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="FromPath">起始路径</param>
        /// <param name="ToPath">目的路径</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> ZipFile(string FromPath, string ToPath, string Password, string Type)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password))
            {
                Log.SaveLog(ip + " 尝试将该路径的文件: \"" + FromPath + "\"以" + Type + "压缩到\"" + ToPath + "\" ,但是他/她输入了错误的密码");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            try
            {
                SevenZipApi.ZipFiles(FromPath, ToPath, Type);
                Log.SaveLog(ip + " 尝试将该路径的文件: \"" + FromPath + "\"以" + Type + "压缩到\"" + ToPath + "\".");
                return "{\"status\":\"Success.\"}";
            }
            catch (Exception ex)
            {
                Log.SaveLog("在处理位于" + ip + "的压缩文件请求时发生了异常:" + ex.ToString());
                return "{\"status\":\"Error\",\"msg\":\"无法压缩该文件.请检查路径名称是否正确,是否以正确的用户账户运行服务端及是否具有该路径的访问权限.\"}"; ;
            }
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DownloadFile(string Path, string Password)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password))
            {
                Log.SaveLog(ip + " 尝试下载该路径的文件: \"" + Path + "\" ,但是他/她输入了错误的密码");
                return Content("{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}");
            }
            try
            {
                Log.SaveLog(ip + " 下载了以下路径的文件:" + Path);
                var stream = System.IO.File.OpenRead(Path);  //创建文件流
                return File(stream, "multipart/form-data",Path.Split("\\")[^1].Split("/")[^1]);
            }
            catch (Exception ex)
            {
                Log.SaveLog("在处理位于" + ip + "的下载文件请求时发生了异常:" + ex.ToString());
                return Content("{\"status\":\"Error\",\"msg\":\"无法下载该文件.请检查路径名称是否正确,是否以正确的用户账户运行服务端及是否具有该路径的访问权限.\"}");
            }
        }
        #endregion
        #region 目录模块
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="Path">文件夹路径</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> Mkdir(string Path, string Password)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password))
            {
                Log.SaveLog(ip + " 尝试创建该路径的文件夹: \"" + Path + "\" ,但是他/她输入了错误的密码");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            try
            {
                Directory.CreateDirectory(Path);
                Log.SaveLog(ip + " 创建了以下路径的文件夹:" + Path);
                return "{\"status\":\"Success.\"}";
            }
            catch (Exception ex)
            {
                Log.SaveLog("在处理位于" + ip + "的创建文件夹请求时发生了异常:" + ex.ToString());
                return "{\"status\":\"Error\",\"msg\":\"无法创建该文件夹.请检查路径名称是否正确,是否以正确的用户账户运行服务端及是否具有该路径的访问权限.\"}"; ;
            }
        }
        /// <summary>
        /// 移动目录
        /// </summary>
        /// <param name="FromPath">起始路径</param>
        /// <param name="ToPath">目的路径</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> MoveDir(string FromPath, string ToPath, string Password)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password))
            {
                Log.SaveLog(ip + " 尝试将该路径的文件夹: \"" + FromPath + "\"移动到\"" + ToPath + "\" ,但是他/她输入了错误的密码");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            try
            {
                Directory.Move(FromPath, ToPath);
                Log.SaveLog(ip + " 尝试将该路径的文件夹: \"" + FromPath + "\"移动到\"" + ToPath + "\".");
                return "{\"status\":\"Success.\"}";
            }
            catch (Exception ex)
            {
                Log.SaveLog("在处理位于" + ip + "的移动目录请求时发生了异常:" + ex.ToString());
                return "{\"status\":\"Error\",\"msg\":\"无法移动该文件夹.请检查路径名称是否正确,是否以正确的用户账户运行服务端及是否具有该路径的访问权限.\"}"; ;
            }
        }
        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="Path">文件夹路径</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> DeleteDir(string Path, string Password)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password))
            {
                Log.SaveLog(ip + " 尝试删除该路径的文件夹: \"" + Path + "\" ,但是他/她输入了错误的密码");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            try
            {
                Directory.Delete(Path);
                Log.SaveLog(ip + " 删除了以下路径的文件夹:" + Path);
                return "{\"status\":\"Success.\"}";
            }
            catch (Exception ex)
            {
                Log.SaveLog("在处理位于" + ip + "的删除文件夹请求时发生了异常:" + ex.ToString());
                return "{\"status\":\"Error\",\"msg\":\"无法删除该文件夹.请检查路径名称是否正确,是否以正确的用户账户运行服务端及是否具有该路径的访问权限.\"}"; ;
            }
        }
        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="FromPath">起始路径</param>
        /// <param name="ToPath">目的路径</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> CopyDir(string FromPath, string ToPath, string Password)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password))
            {
                Log.SaveLog(ip + " 尝试将该路径的文件夹: \"" + FromPath + "\"复制到\"" + ToPath + "\" ,但是他/她输入了错误的密码");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            try
            {
                CopyFolder(FromPath, ToPath);
                Log.SaveLog(ip + " 尝试将该路径的文件夹: \"" + FromPath + "\"复制到\"" + ToPath + "\".");
                return "{\"status\":\"Success.\"}";
            }
            catch (Exception ex)
            {
                Log.SaveLog("在处理位于" + ip + "的复制文件夹请求时发生了异常:" + ex.ToString());
                return "{\"status\":\"Error\",\"msg\":\"无法复制该文件夹.请检查路径名称是否正确,是否以正确的用户账户运行服务端及是否具有该路径的访问权限.\"}"; ;
            }
        }
        /// <summary>
        /// 获取文件夹信息
        /// </summary>
        /// <param name="Path">文件夹路径</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> GetDirInfo(string Path, string Password)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password))
            {
                Log.SaveLog(ip + " 尝试获取该路径的文件夹信息: \"" + Path + "\" ,但是他/她输入了错误的密码");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            try
            {
                Log.SaveLog(ip + " 获取了以下路径的文件夹信息:" + Path);
                string CreationTime = Directory.GetCreationTime(Path).ToString("yyyy-MM-dd-HH:mm:ss");
                string Parent = Directory.GetParent(Path).ToString().Replace("\\", "/");
                string LastAccessTime = Directory.GetLastAccessTime(Path).ToString("yyyy-MM-dd-HH:mm:ss");
                string LastWriteTime = Directory.GetLastWriteTime(Path).ToString("yyyy-MM-dd-HH:mm:ss");
                string root = Directory.GetDirectoryRoot(Path).Replace("\\", "/");
                return "{\"CreationTime\":\"" + CreationTime + "\",\"Parent\":\"" + Parent + "\",\"LastAccess\":\"" + LastAccessTime + "\",\"LastWrite\":\"" + LastWriteTime + "\",\"Root\":\"" + root + "\"}";
            }
            catch (Exception ex)
            {
                Log.SaveLog("在处理位于" + ip + "的获取文件夹信息请求时发生了异常:" + ex.ToString());
                return "{\"status\":\"Error\",\"msg\":\"无法获取该文件夹信息.请检查路径名称是否正确,是否以正确的用户账户运行服务端及是否具有该路径的访问权限.\"}"; ;
            }
        }
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="FromPath">起始路径</param>
        /// <param name="ToPath">目的路径</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> ZipDir(string FromPath, string ToPath, string Password, string Type)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password))
            {
                Log.SaveLog(ip + " 尝试将该路径的文件: \"" + FromPath + "\"以" + Type + "压缩到\"" + ToPath + "\" ,但是他/她输入了错误的密码");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            try
            {
                SevenZipApi.ZipDir(FromPath, ToPath, Type);
                Log.SaveLog(ip + " 尝试将该路径的文件: \"" + FromPath + "\"以" + Type + "压缩到\"" + ToPath + "\".");
                return "{\"status\":\"Success.\"}";
            }
            catch (Exception ex)
            {
                Log.SaveLog("在处理位于" + ip + "的压缩文件请求时发生了异常:" + ex.ToString());
                return "{\"status\":\"Error\",\"msg\":\"无法压缩该文件.请检查路径名称是否正确,是否以正确的用户账户运行服务端及是否具有该路径的访问权限.\"}"; ;
            }
        }
        #endregion
        /// <summary>
        /// 复制文件夹及文件
        /// </summary>
        /// <param name="sourceFolder">原文件路径</param>
        /// <param name="destFolder">目标文件路径</param>
        /// <returns></returns>
        public void CopyFolder(string sourceFolder, string destFolder)
        {
            //如果目标路径不存在,则创建目标路径
            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }
            //得到原文件根目录下的所有文件
            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);
                System.IO.File.Copy(file, dest);//复制文件
            }
            //得到原文件根目录下的所有文件夹
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                CopyFolder(folder, dest);//构建目标路径,递归复制文件
            }
        }
    }
    #endregion
}
