using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SudoItApi.Controllers
{
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
            return "";
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
        public ActionResult<string> TimeDelayExecute(string Command, string Password,string DelayTime)
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
            catch(Exception ex)
            {
                Log.SaveLog("在处理位于" + ip + "的创建文件夹请求时发生了异常:" + ex.ToString());
                return "{\"status\":\"Error\",\"msg\":\"无法创建该文件夹.请检查路径名称是否正确,是否以正确的用户账户运行服务端及是否具有该路径的访问权限.\"}";;
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
                Directory.CreateDirectory(Path);
                Log.SaveLog(ip + " 获取了以下路径的文件夹信息:" + Path);
                string CreationTime = Directory.GetCreationTime(Path).ToString("yyyy-MM-dd-HH:mm:ss");
                string Parent = Directory.GetParent(Path).ToString().Replace("\\","/");
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
        #endregion
    }
    #endregion
}
