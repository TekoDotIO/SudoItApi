using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace SudoItApi.Controllers
{
    /// <summary>
    /// 进程类
    /// </summary>
    [ApiController]
    [Route("SudoIt/[controller]/[action]")]
    public class ProcessController : Controller
    {
        #region GET模块
        /// <summary>
        /// 获取所有进程
        /// </summary>
        /// <param name="Password">密码</param>
        /// <param name="Num">每页个数 all就是不分页 可选</param>
        /// <param name="Page">页数</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> GetProcesses(string Password, string Num = "all", string Page = "1")
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password, ip))
            {
                Log.SaveLog(ip + "尝试获取所有进程,但他/她输入了错误的密码");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            else
            {
                Log.SaveLog(ip + "尝试获取所有进程");
                Process[] processes = Process.GetProcesses();
                string Dictionary = "{";//构造词典
                if (Num == "all")//如果是all,不需要处理页数
                {
                    foreach (Process process in processes)//处理每个Process
                    {
                        string Name = process.ProcessName.ToString();
                        string Pid = process.Id.ToString();
                        Dictionary = Dictionary + "\n\"" + Name + "\":\"" + Pid + "\",";
                    }
                    Dictionary = Dictionary[0..^1] + "\n}";
                    return Plugins.ProcessResult("GetProcesses", Dictionary);//返回词典
                }
                int ExecutedNum = 0;//已执行次数
                int ToNum = Convert.ToInt32(Num);//每页个数
                int PageNum = Convert.ToInt32(Page) - 1;//页数
                //一定要减1,因为默认页数从0开始
                //以上两个变量要使用Convert转换为Int
                int StartNum = PageNum * ToNum;
                //这是PageNum页的起始项目
                int EndNum = StartNum + ToNum;
                //这是PageNum页的最后一个项目
                foreach (Process process in processes)//对每个Process对象进行遍历
                {
                    if (ExecutedNum >= StartNum && ExecutedNum < EndNum)//植树问题,如果全部用大于等于或小于等于会多出一个
                    {
                        string Name = process.ProcessName.ToString();
                        string Pid = process.Id.ToString();
                        Dictionary = Dictionary + "\n\"" + Name + "\":\"" + Pid + "\",";//构造词典
                    }
                    ExecutedNum++;//等同于ExecutedNum+1;
                    //使已执行次数+1,带入下次遍历
                }
                Dictionary = Dictionary[0..^1] + "\n}";//去除末尾","并加上终止符
                return Plugins.ProcessResult("GetProcesses", Dictionary);//返回词典
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
            if (!SetAndAuth.Auth(Password, ip))
            {
                Log.SaveLog(ip + "尝试通过Pid杀死进程" + Pid + ",但他/她输入了错误的密码");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            else
            {
                Log.SaveLog(ip + "尝试通过Pid杀死进程" + Pid);
                try
                {
                    Process process = Process.GetProcessById(Convert.ToInt32(Pid));//通过Pid查找进程
                    process.Kill();//杀死指定进程
                    string Result = "{\"status\":\"OK\",\"msg\":\"Done.\"}";
                    return Plugins.ProcessResult("KillProcessByPid", Result);
                }
                catch(Exception ex)
                {
                    Log.SaveLog("触发异常:\n" + ex.ToString());
                    return "{\"status\":\"Exception\",\"msg\":\"查找或杀死进程时出错.可能原因:1.Pid格式或输入有误,导致找不到进程;2.权限不够,请尝试提权后运行.\",\"exception\":\"" + ex.Message + "\"}";
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
            if (!SetAndAuth.Auth(Password, ip))
            {
                Log.SaveLog(ip + "尝试通过名称杀死进程" + Name + ",但他/她输入了错误的密码");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            else
            {
                Log.SaveLog(ip + "尝试通过名称杀死进程" + Name);
                try
                {
                    Process[] processes = Process.GetProcessesByName(Name);//通过名称获取进程
                    //这里需要用Process[]来接收,因为可能存在进程同名
                    foreach(Process process in processes)//逐个杀死进程
                    {
                        process.Kill();
                        Log.SaveLog("成功杀死了位于Pid" + process.Id + "的进程" + Name);
                    }
                    string Result = "{\"status\":\"OK\",\"msg\":\"Done.\"}";
                    return Plugins.ProcessResult("KillProcessByName", Result);
                }
                catch (Exception ex)
                {
                    Log.SaveLog("触发异常:\n" + ex.ToString());
                    return "{\"status\":\"Exception\",\"msg\":\"查找或杀死进程时出错.可能原因:1.名称格式或输入有误,导致找不到进程;2.权限不够,请尝试提权后运行.\",\"exception\":\"" + ex.Message + "\"}";
                }
            }
        }
        /// <summary>
        /// 启动进程
        /// </summary>
        /// <param name="Password">密码</param>
        /// <param name="Path">文件路径</param>
        /// <param name="CreateWindow">是否创建窗口(可选,默认否)</param>
        /// <param name="Args">启动参数(可选)</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> StartProcess(string Password, string Path, string CreateWindow = "false", string Args = "")
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password, ip))
            {
                Log.SaveLog(ip + "尝试启动进程" + Path + ",但他/她输入了错误的密码");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            else
            {
                Log.SaveLog(ip + "尝试启动进程" + Path);
                try
                {
                    Process process = new Process();//构造进程
                    process.StartInfo.FileName = Path;//设定路径
                    switch (CreateWindow)//判断是否启动窗口
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
                    process.Start();//启动进程
                    string Result = "{\"status\":\"OK\",\"msg\":\"Done.\"}";
                    return Plugins.ProcessResult("StartProcess", Result);
                }
                catch (Exception ex)
                {
                    Log.SaveLog("触发异常:\n" + ex.ToString());
                    return "{\"status\":\"Exception\",\"msg\":\"启动进程出错.可能原因:1.名称格式或输入有误,导致找不到进程;2.权限不够,请尝试提权后运行.\",\"exception\":\"" + ex.Message + "\"}";
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
            if (!SetAndAuth.Auth(Password, ip))
            {
                Log.SaveLog(ip + "尝试通过Pid查找进程" + Pid + ",但他/她输入了错误的密码");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            else
            {
                Log.SaveLog(ip + "尝试通过Pid查找进程" + Pid);
                try
                {
                    string Name = Process.GetProcessById(Convert.ToInt32(Pid)).ProcessName;//获取指定Pid的进程
                    string Result = "{\"status\":\"OK\",\"info\":\"" + Name + "\"}";
                    return Plugins.ProcessResult("GetName", Result);
                }
                catch (Exception ex)
                {
                    Log.SaveLog("触发异常:\n" + ex.ToString());
                    return "{\"status\":\"Exception\",\"msg\":\"查找进程时出错.可能原因:1.Pid格式或输入有误,导致找不到进程;2.权限不够,请尝试提权后运行.\",\"exception\":\"" + ex.Message + "\"}";
                }
            }
        }
        /// <summary>
        /// 通过名称获取Pid
        /// </summary>
        /// <param name="Password">密码</param>
        /// <param name="Name">进程名称</param>
        /// <param name="Num">每页项目</param>
        /// <param name="Page">页码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> GetPid(string Password, string Name, string Num = "all", string Page = "1")
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password, ip))
            {
                Log.SaveLog(ip + "尝试通过名称查找进程" + Name + ",但他/她输入了错误的密码");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            else
            {
                Log.SaveLog(ip + "尝试通过名称查找进程" + Name);
                try
                {
                    Process[] Pids = Process.GetProcessesByName(Name);//通过名称获取进程集
                    //可能有多个进程,使用Process[]
                    string Dictionary = "[";
                    //foreach (Process process in Pids)
                    //{
                    //    string Pid = process.Id.ToString();
                    //    Dictionary = Dictionary + "\n\"" + Pid + "\",";
                    //}
                    //Dictionary = Dictionary[0..^1] + "\n]";
                    //return Dictionary;
                    //以上是原本的写法,仅供参考
                    if (Num == "all")//此时不需要处理Json
                    {
                        foreach (Process process in Pids)//遍历每个Process对象
                        {
                            string Pid = process.Id.ToString();
                            Dictionary = Dictionary + "\n\"" + Pid + "\",";
                        }
                        Dictionary = Dictionary[0..^1] + "\n]";
                        return Plugins.ProcessResult("GetPid", Dictionary);//返回词典
                    }
                    int ExecutedNum = 0;//已执行次数
                    int ToNum = Convert.ToInt32(Num);//每页个数
                    int PageNum = Convert.ToInt32(Page) - 1;//页数
                    int StartNum = PageNum * ToNum;//本页起始项目编号
                    int EndNum = StartNum + ToNum;//本页结束项目编号
                    foreach (Process process in Pids)//遍历Processes
                    {
                        if (ExecutedNum >= StartNum && ExecutedNum < EndNum)//植树问题,如果全部用大于等于或小于等于会多出一个
                        {
                            string Pid = process.Id.ToString();
                            Dictionary = Dictionary + "\n\"" + Pid + "\",";
                        }
                        ExecutedNum++;//次数+1
                    }
                    Dictionary = Dictionary[0..^1] + "\n]";//去除末尾","并添加终止符
                    return Plugins.ProcessResult("GetPid", Dictionary);//返回词典
                }
                catch (Exception ex)
                {
                    Log.SaveLog("触发异常:\n" + ex.ToString());
                    return "{\"status\":\"Exception\",\"msg\":\"查找进程时出错.可能原因:1.名称格式或输入有误,导致找不到进程;2.权限不够,请尝试提权后运行.\",\"exception\":\"" + ex.Message + "\"}";
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
        //    if (!SetAndAuth.Auth(Password, ip))
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
        //            return "{\"status\":\"Error\",\"msg\":\"查询进程时出错.可能原因:1.Pid格式或输入有误,导致找不到进程;2.权限不够,请尝试提权后运行.\",\"exception\":\"" + ex.Message + "\"}";
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
        //    if (!SetAndAuth.Auth(Password, ip))
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
        //            return "{\"status\":\"Error\",\"msg\":\"查询进程时出错.可能原因:1.名称格式或输入有误,导致找不到进程;2.权限不够,请尝试提权后运行.\",\"exception\":\"" + ex.Message + "\"}";
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
            //这里需要额外记录IP
            //因为下面调用的是GET的方法,无法获取当前IP
            return obj.Operation switch
            {
                "GetProcesses" => GetProcesses(obj.Password, obj.Num, obj.Page),
                "KillProcessByPid" => KillProcessByPid(obj.Password, obj.Pid),
                "KillProcessByName" => KillProcessByName(obj.Password, obj.Name),
                "Start" => StartProcess(obj.Password, obj.Path, obj.CreateWindow, obj.Args),
                "GetName" => GetName(obj.Password, obj.Pid),
                "GetPid" => GetPid(obj.Password, obj.Name, obj.Num, obj.Page),
                _ => "{\"status\":\"Error\",\"msg\":\"未指定的操作\"}",
            };
        }
        /// <summary>
        /// JSON类
        /// </summary>
        public class Json
        {
            /// <summary>
            /// 操作命令
            /// </summary>
            public string Operation { get; set; }
            /// <summary>
            /// 密码
            /// </summary>
            public string Password { get; set; }
            /// <summary>
            /// 进程PID
            /// </summary>
            public string Pid { get; set; }
            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 路径
            /// </summary>
            public string Path { get; set; }
            /// <summary>
            /// 参数
            /// </summary>
            public string Args { get; set; }
            /// <summary>
            /// 是否创建窗口
            /// </summary>
            public string CreateWindow { get; set; }
            /// <summary>
            /// 每页个数
            /// </summary>
            public string Num { get; set; }
            /// <summary>
            /// 页码
            /// </summary>
            public string Page { get; set; }
        }
        #endregion
    }
}
