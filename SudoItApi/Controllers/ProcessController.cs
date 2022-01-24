using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace SudoItApi.Controllers
{
    [ApiController]
    [Route("SudoIt/[controller]/[action]")]
    public class ProcessController : Controller
    {
        #region GET模块
        /// <summary>
        /// 获取所有进程
        /// </summary>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> GetProcesses(string Password)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password))
            {
                Log.SaveLog(ip + "尝试获取所有进程,但他/她输入了错误的密码");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            else
            {
                Log.SaveLog(ip + "尝试获取所有进程");
                Process[] processes = Process.GetProcesses();
                string Dictionary = "{";
                foreach (Process process in processes)
                {
                    string Name = process.ProcessName.ToString();
                    string Pid = process.Id.ToString();
                    Dictionary = Dictionary + "\n\"" + Name + "\":\"" + Pid + "\",";
                }
                Dictionary = Dictionary[0..^1] + "\n}";
                return Dictionary;
            }
        }
        /// <summary>
        /// 通过Pid杀死进程
        /// </summary>
        /// <param name="Password">密码</param>
        /// <param name="Pid">进程Pid</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> KillProcessByPid(string Password, string Pid)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password))
            {
                Log.SaveLog(ip + "尝试通过Pid杀死进程" + Pid + ",但他/她输入了错误的密码");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            else
            {
                Log.SaveLog(ip + "尝试通过Pid杀死进程" + Pid);
                try
                {
                    Process process = Process.GetProcessById(Convert.ToInt32(Pid));
                    process.Kill();
                    return "{\"status\":\"OK\",\"msg\":\"Done.\"}";
                }
                catch(Exception ex)
                {
                    Log.SaveLog("触发异常:\n" + ex.ToString());
                    return "{\"status\":\"Error\",\"msg\":\"查找或杀死进程时出错.可能原因:1.Pid格式或输入有误,导致找不到进程;2.权限不够,请尝试提权后运行.\"}";
                }
            }
        }
        /// <summary>
        /// 通过名称杀死进程
        /// </summary>
        /// <param name="Password">密码</param>
        /// <param name="Name">进程名称</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> KillProcessByName(string Password, string Name)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password))
            {
                Log.SaveLog(ip + "尝试通过名称杀死进程" + Name + ",但他/她输入了错误的密码");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            else
            {
                Log.SaveLog(ip + "尝试通过名称杀死进程" + Name);
                try
                {
                    Process[] processes = Process.GetProcessesByName(Name);
                    foreach(Process process in processes)
                    {
                        process.Kill();
                        Log.SaveLog("成功杀死了位于Pid" + process.Id + "的进程" + Name);
                    }
                    return "{\"status\":\"OK\",\"msg\":\"Done.\"}";
                }
                catch (Exception ex)
                {
                    Log.SaveLog("触发异常:\n" + ex.ToString());
                    return "{\"status\":\"Error\",\"msg\":\"查找或杀死进程时出错.可能原因:1.名称格式或输入有误,导致找不到进程;2.权限不够,请尝试提权后运行.\"}";
                }
            }
        }
        /// <summary>
        /// 启动进程
        /// </summary>
        /// <param name="Password">密码</param>
        /// <param name="Path">文件路径</param>
        /// <param name="CreateWindow">是否创建窗口(可选,默认否)</param>
        /// <param name="args">启动参数(可选)</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> StartProcess(string Password, string Path, string CreateWindow = "false", string Args = "")
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password))
            {
                Log.SaveLog(ip + "尝试启动进程" + Path + ",但他/她输入了错误的密码");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            else
            {
                Log.SaveLog(ip + "尝试启动进程" + Path);
                try
                {
                    Process process = new Process();
                    process.StartInfo.FileName = Path;
                    switch (CreateWindow)
                    {
                        case "True":
                        case "true":
                        case "TRUE":
                        case "1":
                            process.StartInfo.CreateNoWindow = false;
                            break;
                        case "False":
                        case "FALSE":
                        case "false":
                        case "0":
                            process.StartInfo.CreateNoWindow = true;
                            break;
                        default:
                            return "{\"status\":\"Error\",\"msg\":\"未指定是否打开窗口\"}";
                    }
                    if (Args != "") 
                    {
                        process.StartInfo.Arguments = Args;
                    }
                    process.Start();
                    return "{\"status\":\"OK\",\"msg\":\"Done.\"}";
                }
                catch (Exception ex)
                {
                    Log.SaveLog("触发异常:\n" + ex.ToString());
                    return "{\"status\":\"Error\",\"msg\":\"启动进程出错.可能原因:1.名称格式或输入有误,导致找不到进程;2.权限不够,请尝试提权后运行.\"}";
                }
            }
        }
        /// <summary>
        /// 通过Pid获取进程名称
        /// </summary>
        /// <param name="Password">密码</param>
        /// <param name="Pid">进程Pid</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> GetName(string Password, string Pid)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password))
            {
                Log.SaveLog(ip + "尝试通过Pid查找进程" + Pid + ",但他/她输入了错误的密码");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            else
            {
                Log.SaveLog(ip + "尝试通过Pid查找进程" + Pid);
                try
                {
                    string Name = Process.GetProcessById(Convert.ToInt32(Pid)).ProcessName;
                    return "{\"status\":\"OK\",\"info\":\"" + Name + "\"}";
                }
                catch (Exception ex)
                {
                    Log.SaveLog("触发异常:\n" + ex.ToString());
                    return "{\"status\":\"Error\",\"msg\":\"查找进程时出错.可能原因:1.Pid格式或输入有误,导致找不到进程;2.权限不够,请尝试提权后运行.\"}";
                }
            }
        }
        /// <summary>
        /// 通过名称获取Pid
        /// </summary>
        /// <param name="Password">密码</param>
        /// <param name="Name">进程名称</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> GetPid(string Password, string Name)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password))
            {
                Log.SaveLog(ip + "尝试通过名称查找进程" + Name + ",但他/她输入了错误的密码");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            else
            {
                Log.SaveLog(ip + "尝试通过名称查找进程" + Name);
                try
                {
                    Process[] Pids = Process.GetProcessesByName(Name);
                    string Dictionary = "[";
                    foreach (Process process in Pids)
                    {
                        string Pid = process.Id.ToString();
                        Dictionary = Dictionary + "\n\"" + Pid + "\",";
                    }
                    Dictionary = Dictionary[0..^1] + "\n]";
                    return Dictionary;
                }
                catch (Exception ex)
                {
                    Log.SaveLog("触发异常:\n" + ex.ToString());
                    return "{\"status\":\"Error\",\"msg\":\"查找进程时出错.可能原因:1.名称格式或输入有误,导致找不到进程;2.权限不够,请尝试提权后运行.\"}";
                }
            }
        }

        //以下代码已废弃
        //原因:无法实现所需需求,无法跨进程获取StartInfo.


        ///// <summary>
        ///// 通过Pid查询进程信息
        ///// </summary>
        ///// <param name="Password">密码</param>
        ///// <param name="Pid">进程Pid</param>
        ///// <returns></returns>
        //[HttpGet]
        //public ActionResult<string> GetInfoByPid(string Password, string Pid)
        //{
        //    string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        //    if (!SetAndAuth.Auth(Password))
        //    {
        //        Log.SaveLog(ip + "尝试通过Pid查询进程" + Pid + ",但他/她输入了错误的密码");
        //        return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
        //    }
        //    else
        //    {
        //        Log.SaveLog(ip + "尝试通过Pid查询进程" + Pid);
        //        try
        //        {
        //            Process process = Process.GetProcessById(Convert.ToInt32(Pid));
        //            string Name = process.ProcessName;
        //            string Path = process.StartInfo.FileName;
        //            string UserInfo = process.StartInfo.UserName;
        //            string Args = process.StartInfo.Arguments;
        //            string WorkDir = process.StartInfo.WorkingDirectory;
        //            string StartTime = process.StartTime.ToString("yyyy-MM-dd,HH:mm");
        //            return "{\n\"Name\":\"" + Name + "\",\n\"Pid\":\"" + Pid + "\",\n\"Path\":\"" + Path + "\",\n\"User\":\"" + UserInfo + "\",\n\"Args\":\"" + Args + "\",\n\"WorkDir\":\"" + WorkDir + "\",\n\"StartTime\":\"" + StartTime + "\"\n}";
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.SaveLog("触发异常:\n" + ex.ToString());
        //            return "{\"status\":\"Error\",\"msg\":\"查询进程时出错.可能原因:1.Pid格式或输入有误,导致找不到进程;2.权限不够,请尝试提权后运行.\"}";
        //        }
        //    }
        //}
        ///// <summary>
        ///// 通过Pid查询进程信息
        ///// </summary>
        ///// <param name="Password">密码</param>
        ///// <param name="Name">进程名称</param>
        ///// <returns></returns>
        //[HttpGet]
        //public ActionResult<string> GetInfoByName(string Password, string Name)
        //{
        //    string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        //    if (!SetAndAuth.Auth(Password))
        //    {
        //        Log.SaveLog(ip + "尝试通过名称查询进程" + Name + ",但他/她输入了错误的密码");
        //        return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
        //    }
        //    else
        //    {
        //        Log.SaveLog(ip + "尝试通过名称查询进程" + Name);
        //        try
        //        {
        //            Process[] processes = Process.GetProcessesByName(Name);
        //            string Dictionary = "{";
        //            foreach (Process process in processes)
        //            {
        //                string Pid = process.Id.ToString();
        //                string Path = process.StartInfo.FileName;
        //                string UserInfo = process.StartInfo.UserName;
        //                string Args = process.StartInfo.Arguments;
        //                string WorkDir = process.StartInfo.WorkingDirectory;
        //                string StartTime = process.StartTime.ToString("yyyy-MM-dd,HH:mm");
        //                Dictionary = Dictionary + "\"" + Pid + "\":\"{\n\"Name\":\"" + Name + "\",\n\"Pid\":\"" + Pid + "\",\n\"Path\":\"" + Path + "\",\n\"User\":\"" + UserInfo + "\",\n\"Args\":\"" + Args + "\",\n\"WorkDir\":\"" + WorkDir + "\",\n\"StartTime\":\"" + StartTime + "\"\n}\",";
        //            }
        //            Dictionary = Dictionary[0..^1] + "\n}";
        //            return Dictionary;
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.SaveLog("触发异常:\n" + ex.ToString());
        //            return "{\"status\":\"Error\",\"msg\":\"查询进程时出错.可能原因:1.名称格式或输入有误,导致找不到进程;2.权限不够,请尝试提权后运行.\"}";
        //        }
        //    }
        //}

        //以上代码已废弃
        //原因:无法实现所需需求,无法跨进程获取StartInfo.
        #endregion
        #region POST模块
        /// <summary>
        /// POST方式进程接口
        /// </summary>
        /// <param name="obj">JSON对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<string> PostApi([FromBody] Json obj)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            Log.SaveLog(ip + "使用POST方式访问了进程模块");
            switch (obj.Operation)
            {
                case "GetProcesses":
                    return GetProcesses(obj.Password);
                case "KillProcessByPid":
                    return KillProcessByPid(obj.Password, obj.Pid);
                case "KillProcessByName":
                    return KillProcessByName(obj.Password, obj.Name);
                case "Start":
                    return StartProcess(obj.Password, obj.Path, obj.CreateWindow, obj.Args);
                case "GetName":
                    return GetName(obj.Password, obj.Pid);
                case "GetPid":
                    return GetPid(obj.Password, obj.Name);
                default:
                    return "{\"status\":\"Error\",\"msg\":\"未指定的操作\"}";
            }
        }
        public class Json
        {
            public string Operation { get; set; }
            public string Password { get; set; }
            public string Pid { get; set; }
            public string Name { get; set; }
            public string Path { get; set; }
            public string Args { get; set; }
            public string CreateWindow { get; set; }
        }
        #endregion
    }
}
