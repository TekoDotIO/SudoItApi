﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace SudoItApi.Controllers
{
    /// <summary>
    /// 插件控制器
    /// </summary>
    [ApiController]
    [Route("SudoIt/[controller]/[action]")]
    public class PluginsController : Controller
    {
        /// <summary>
        /// 获取所有GET方法
        /// </summary>
        /// <param name="Password">密码</param>
        /// <param name="Num">每页个数</param>
        /// <param name="Page">页码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> GetMethods(string Password, string Num = "all", string Page = "1")
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password, ip))
            {
                Log.SaveLog(ip + " 尝试获取所有GET插件 ,但是他/她输入了错误的密码.");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            try
            {
                string Dictionary = "{";
                string[] Apis = Plugins.GetApis();
                if (Num == "all")//此时不需要处理Json
                {
                    foreach (string Api in Apis)//遍历每个Process对象
                    {
                        Dictionary = Dictionary + "\n\"" + Api + "\",";
                    }
                    Dictionary = Dictionary[0..^1] + "\n]";
                    return Plugins.ProcessResult("GetMethods", Dictionary);//返回词典
                }
                int ExecutedNum = 0;//已执行次数
                int ToNum = Convert.ToInt32(Num);//每页个数
                int PageNum = Convert.ToInt32(Page) - 1;//页数
                int StartNum = PageNum * ToNum;//本页起始项目编号
                int EndNum = StartNum + ToNum;//本页结束项目编号
                foreach (string Api in Apis)//遍历Processes
                {
                    if (ExecutedNum >= StartNum && ExecutedNum < EndNum)//植树问题,如果全部用大于等于或小于等于会多出一个
                    {
                        Dictionary = Dictionary + "\n\"" + Api + "\",";
                    }
                    ExecutedNum++;//次数+1
                }
                Dictionary = Dictionary[0..^1] + "\n]";//去除末尾","并添加终止符
                return Plugins.ProcessResult("GetMethods", Dictionary);//返回词典
            }
            catch(Exception ex)
            {
                return "{\"status\":\"Exception\",\"msg\":\"运行插件时出错.可能原因:1.名称输入有误,导致调用错插件;2.权限不够,请尝试提权后运行.\",\"exception\":\"" + ex.Message + "\"}";
            }
        }
        /// <summary>
        /// 获取所有POST方法
        /// </summary>
        /// <param name="Password">密码</param>
        /// <param name="Num">每页个数</param>
        /// <param name="Page">页码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> PostMethods(string Password, string Num = "all", string Page = "1")
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password, ip))
            {
                Log.SaveLog(ip + " 尝试获取所有POST插件 ,但是他/她输入了错误的密码.");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            try
            {
                string Dictionary = "{";
                string[] Apis = Plugins.PostApis();
                if (Num == "all")//此时不需要处理Json
                {
                    foreach (string Api in Apis)//遍历每个Process对象
                    {
                        Dictionary = Dictionary + "\n\"" + Api + "\",";
                    }
                    Dictionary = Dictionary[0..^1] + "\n]";
                    return Plugins.ProcessResult("PostMethods", Dictionary);//返回词典
                }
                int ExecutedNum = 0;//已执行次数
                int ToNum = Convert.ToInt32(Num);//每页个数
                int PageNum = Convert.ToInt32(Page) - 1;//页数
                int StartNum = PageNum * ToNum;//本页起始项目编号
                int EndNum = StartNum + ToNum;//本页结束项目编号
                foreach (string Api in Apis)//遍历Processes
                {
                    if (ExecutedNum >= StartNum && ExecutedNum < EndNum)//植树问题,如果全部用大于等于或小于等于会多出一个
                    {
                        Dictionary = Dictionary + "\n\"" + Api + "\",";
                    }
                    ExecutedNum++;//次数+1
                }
                Dictionary = Dictionary[0..^1] + "\n]";//去除末尾","并添加终止符
                return Plugins.ProcessResult("PostMethods", Dictionary);//返回词典
            }
            catch (Exception ex)
            {
                return "{\"status\":\"Exception\",\"msg\":\"运行插件时出错.可能原因:1.名称输入有误,导致调用错插件;2.权限不够,请尝试提权后运行.\",\"exception\":\"" + ex.Message + "\"}";
            }
        }
        /// <summary>
        /// 获取所有修改器方法
        /// </summary>
        /// <param name="Password">密码</param>
        /// <param name="Num">每页个数</param>
        /// <param name="Page">页码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> InsideProcessorMethods(string Password, string Num = "all", string Page = "1")
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password, ip))
            {
                Log.SaveLog(ip + " 尝试获取所有修改器插件 ,但是他/她输入了错误的密码.");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            try
            {
                string Dictionary = "{";
                string[] Apis = Plugins.InsideApis();
                if (Num == "all")//此时不需要处理Json
                {
                    foreach (string Api in Apis)//遍历每个Process对象
                    {
                        Dictionary = Dictionary + "\n\"" + Api + "\",";
                    }
                    Dictionary = Dictionary[0..^1] + "\n]";
                    return Plugins.ProcessResult("InsideProcessorMethods", Dictionary);//返回词典
                }
                int ExecutedNum = 0;//已执行次数
                int ToNum = Convert.ToInt32(Num);//每页个数
                int PageNum = Convert.ToInt32(Page) - 1;//页数
                int StartNum = PageNum * ToNum;//本页起始项目编号
                int EndNum = StartNum + ToNum;//本页结束项目编号
                foreach (string Api in Apis)//遍历Processes
                {
                    if (ExecutedNum >= StartNum && ExecutedNum < EndNum)//植树问题,如果全部用大于等于或小于等于会多出一个
                    {
                        Dictionary = Dictionary + "\n\"" + Api + "\",";
                    }
                    ExecutedNum++;//次数+1
                }
                Dictionary = Dictionary[0..^1] + "\n]";//去除末尾","并添加终止符
                return Plugins.ProcessResult("InsideProcessorMethods", Dictionary);//返回词典
            }
            catch (Exception ex)
            {
                return "{\"status\":\"Exception\",\"msg\":\"运行插件时出错.可能原因:1.名称输入有误,导致调用错插件;2.权限不够,请尝试提权后运行.\",\"exception\":\"" + ex.Message + "\"}";
            }
        }
        /// <summary>
        /// 获取所有命令行方法
        /// </summary>
        /// <param name="Password">密码</param>
        /// <param name="Num">每页个数</param>
        /// <param name="Page">页码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> CommandProcessorMethods(string Password, string Num = "all", string Page = "1")
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password, ip))
            {
                Log.SaveLog(ip + " 尝试获取所有命令行插件 ,但是他/她输入了错误的密码.");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            try
            {
                string Dictionary = "{";
                string[] Apis = Plugins.CommandApis();
                if (Num == "all")//此时不需要处理Json
                {
                    foreach (string Api in Apis)//遍历每个Process对象
                    {
                        Dictionary = Dictionary + "\n\"" + Api + "\",";
                    }
                    Dictionary = Dictionary[0..^1] + "\n]";
                    return Plugins.ProcessResult("CommandProcessorMethods", Dictionary);//返回词典
                }
                int ExecutedNum = 0;//已执行次数
                int ToNum = Convert.ToInt32(Num);//每页个数
                int PageNum = Convert.ToInt32(Page) - 1;//页数
                int StartNum = PageNum * ToNum;//本页起始项目编号
                int EndNum = StartNum + ToNum;//本页结束项目编号
                foreach (string Api in Apis)//遍历Processes
                {
                    if (ExecutedNum >= StartNum && ExecutedNum < EndNum)//植树问题,如果全部用大于等于或小于等于会多出一个
                    {
                        Dictionary = Dictionary + "\n\"" + Api + "\",";
                    }
                    ExecutedNum++;//次数+1
                }
                Dictionary = Dictionary[0..^1] + "\n]";//去除末尾","并添加终止符
                return Plugins.ProcessResult("CommandProcessorMethods", Dictionary);//返回词典
            }
            catch (Exception ex)
            {
                return "{\"status\":\"Exception\",\"msg\":\"运行插件时出错.可能原因:1.名称输入有误,导致调用错插件;2.权限不够,请尝试提权后运行.\",\"exception\":\"" + ex.Message + "\"}";
            }
        }
        /// <summary>
        /// 获取所有可执行插件
        /// </summary>
        /// <param name="Password">密码</param>
        /// <param name="Num">每页个数</param>
        /// <param name="Page">页码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> GetAllPlugins(string Password, string Num = "all", string Page = "1")
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password, ip))
            {
                Log.SaveLog(ip + " 尝试获取所有插件 ,但是他/她输入了错误的密码.");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            try
            {
                string Dictionary = "{";
                string[] Apis = System.IO.Directory.GetFiles("./Plugins/");
                if (Num == "all")//此时不需要处理Json
                {
                    foreach (string Api in Apis)//遍历每个Process对象
                    {
                        Dictionary = Dictionary + "\n\"" + Api + "\",";
                    }
                    Dictionary = Dictionary[0..^1] + "\n]";
                    return Plugins.ProcessResult("GetAllPlugins", Dictionary);//返回词典
                }
                int ExecutedNum = 0;//已执行次数
                int ToNum = Convert.ToInt32(Num);//每页个数
                int PageNum = Convert.ToInt32(Page) - 1;//页数
                int StartNum = PageNum * ToNum;//本页起始项目编号
                int EndNum = StartNum + ToNum;//本页结束项目编号
                foreach (string Api in Apis)//遍历Processes
                {
                    if (ExecutedNum >= StartNum && ExecutedNum < EndNum)//植树问题,如果全部用大于等于或小于等于会多出一个
                    {
                        Dictionary = Dictionary + "\n\"" + Api + "\",";
                    }
                    ExecutedNum++;//次数+1
                }
                Dictionary = Dictionary[0..^1] + "\n]";//去除末尾","并添加终止符
                return Plugins.ProcessResult("GetAllPlugins", Dictionary);//返回词典
            }
            catch (Exception ex)
            {
                return "{\"status\":\"Exception\",\"msg\":\"运行插件时出错.可能原因:1.名称输入有误,导致调用错插件;2.权限不够,请尝试提权后运行.\",\"exception\":\"" + ex.Message + "\"}";
            }
        }
        /// <summary>
        /// 获取所有可执行插件
        /// </summary>
        /// <param name="Password">密码</param>
        /// <param name="Method">需要查找的方法</param>
        /// <param name="Path">目标插件路径</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> GetPointedPlugins(string Password, string Method, string Path)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password, ip))
            {
                Log.SaveLog(ip + " 尝试获取指定插件:\"" + Method + "\",\"" + Path + "\" ,但是他/她输入了错误的密码.");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            try
            {
                return Path switch
                {
                    "GET-Methods" => Plugins.ProcessResult("GetPointedPlugins", "{\"status\":\"OK\",\"info\":\"" + System.IO.File.ReadAllText("./Plugins/GET-Methods/" + Method + ".txt") + "\"}"),
                    "POST-Methods" => Plugins.ProcessResult("GetPointedPlugins", "{\"status\":\"OK\",\"info\":\"" + System.IO.File.ReadAllText("./Plugins/POST-Methods/" + Method + ".txt") + "\"}"),
                    "InsideProcessor" => Plugins.ProcessResult("GetPointedPlugins", "{\"status\":\"OK\",\"info\":\"" + System.IO.File.ReadAllText("./Plugins/InsideProcessor/" + Method + ".txt") + "\"}"),
                    "CommandProcessor" => Plugins.ProcessResult("GetPointedPlugins", "{\"status\":\"OK\",\"info\":\"" + System.IO.File.ReadAllText("./Plugins/CommandProcessor/" + Method + ".txt") + "\"}"),
                    _ => "{\"status\":\"Exception\",\"msg\":\"请指定Path,它应为GET-Methods,POST-Methods,InsideProcessor或CommandProcessor\"}",
                };
            }
            catch (Exception ex)
            {
                return "{\"status\":\"Exception\",\"msg\":\"运行插件时出错.可能原因:1.名称输入有误,导致调用错插件;2.权限不够,请尝试提权后运行.\",\"exception\":\"" + ex.Message + "\"}";
            }
        }
        /// <summary>
        /// 初始化所有可执行插件
        /// </summary>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> Initializate(string Password)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password, ip))
            {
                Log.SaveLog(ip + " 尝试初始化所有插件 ,但是他/她输入了错误的密码.");
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            try
            {
                Plugins.InitializatePlugins();
                return "{\"status\":\"OK\"}";
            }
            catch (Exception ex)
            {
                return "{\"status\":\"Exception\",\"msg\":\"运行插件时出错.可能原因:1.名称输入有误,导致调用错插件;2.权限不够,请尝试提权后运行.\",\"exception\":\"" + ex.Message + "\"}";
            }
        }
        /// <summary>
        /// 调用GET方法插件
        /// </summary>
        /// <param name="Method">方法</param>
        /// <param name="Password">密码</param>
        /// <param name="Args">参数</param>
        /// <param name="WaitForResult">是否等待返回</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> Get(string Method, string Password, string Args = "", string WaitForResult = "False")
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(Password, ip))
            {
                Log.SaveLog(ip + " 尝试调用GET插件 ,但是他/她输入了错误的密码,方法:" + Method + ",参数:" + Args);
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            if (!System.IO.File.Exists("./Plugins/GET-Methods/" + Method + ".txt"))
            {
                Log.SaveLog(ip + " 尝试调用不存在的插件方法:" + Method);
                return "{\"status\":\"Error\",\"msg\":\"插件或方法不存在.请检查拼写与方法是否有误.\"}";
            }
            try
            {
                Process PluginProcess = new Process();
                string Processor = System.IO.File.ReadAllText("./Plugins/GET-Methods/" + Method + ".txt");
                PluginProcess.StartInfo.FileName = "./Plugins/" + Processor;
                PluginProcess.StartInfo.CreateNoWindow = true;
                PluginProcess.StartInfo.Arguments = "--Method " + Method + " --HttpMethod GET --Args \"" + Args + "\"";
                string Output;
                string[] Outputs;
                switch (WaitForResult)
                {
                    case "False":
                    case "0":
                    case "false":
                    case "FALSE":
                        PluginProcess.Start();
                        Log.SaveLog("插件方法" + Method + "成功被调用,参数:" + Args);
                        return Plugins.ProcessResult("Plugin.Get." + Method, "{\"status\":\"OK\",\"msg\":\"成功调用\"}");
                    case "True":
                    case "1":
                    case "true":
                    case "TRUE":
                        PluginProcess.StartInfo.RedirectStandardInput = true; //接受来自调用程序的输入信息
                        PluginProcess.StartInfo.RedirectStandardOutput = true; //由调用程序获取输出信息
                        PluginProcess.StartInfo.RedirectStandardError = true; //重定向标准错误输出
                        PluginProcess.Start();
                        PluginProcess.WaitForExit();
                        Output = PluginProcess.StandardOutput.ReadToEnd();
                        Outputs = Output.Split("\n");
                        Log.SaveLog("插件方法" + Method + "成功被调用,参数:" + Args);
                        Log.SaveLog("插件返回给用户的信息:" + Outputs[0]);
                        Log.SaveLog("插件返回给控制台的信息:" + Outputs[1]);
                        return Plugins.ProcessResult("Plugin.Get." + Method, Outputs[0]);
                    default:
                        return "{\"status\":\"Error\",\"msg\":\"未指定的是否获取返回值.\"}";
                }
            }
            catch (Exception ex)
            {
                Log.SaveLog("插件方法" + Method + "调用异常,参数:" + Args);
                Log.SaveLog("触发异常:\n" + ex.ToString());
                return "{\"status\":\"Exception\",\"msg\":\"运行插件时出错.可能原因:1.名称输入有误,导致调用错插件;2.权限不够,请尝试提权后运行.\",\"exception\":\"" + ex.Message + "\"}";
            }
        }

        /// <summary>
        /// 调用POST方法插件
        /// </summary>
        /// <param name="obj">Json对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<string> POST(Json obj)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (!SetAndAuth.Auth(obj.Password, ip))
            {
                Log.SaveLog(ip + " 尝试调用POST插件 ,但是他/她输入了错误的密码,方法:" + obj.Method + ",参数:" + obj.Args);
                return "{\"status\":\"Error\",\"msg\":\"密码不正确.Password is not correct.\"}";
            }
            if (!System.IO.File.Exists("./Plugins/POST-Methods/" + obj.Method + ".txt"))
            {
                Log.SaveLog(ip + " 尝试调用不存在的插件方法:" + obj.Method);
                return "{\"status\":\"Error\",\"msg\":\"插件或方法不存在.请检查拼写与方法是否有误.\"}";
            }
            try
            {
                Process PluginProcess = new Process();
                string Processor = System.IO.File.ReadAllText("./Plugins/POST-Methods/" + obj.Method + ".txt");
                PluginProcess.StartInfo.FileName = "./Plugins/" + Processor;
                PluginProcess.StartInfo.CreateNoWindow = true;
                PluginProcess.StartInfo.Arguments = "--Method " + obj.Method + " --HttpMethod POST --Args \"" + obj.Args + "\"";
                string Output;
                string[] Outputs;
                switch (obj.WaitForResult)
                {
                    case "False":
                    case "0":
                    case "false":
                    case "FALSE":
                        PluginProcess.Start();
                        Log.SaveLog("插件方法" + obj.Method + "成功被调用,参数:" + obj.Args);
                        return Plugins.ProcessResult("Plugin.Post." + obj.Method, "{\"status\":\"OK\",\"msg\":\"成功调用\"}");
                    case "True":
                    case "1":
                    case "true":
                    case "TRUE":
                        PluginProcess.StartInfo.RedirectStandardInput = true; //接受来自调用程序的输入信息
                        PluginProcess.StartInfo.RedirectStandardOutput = true; //由调用程序获取输出信息
                        PluginProcess.StartInfo.RedirectStandardError = true; //重定向标准错误输出
                        PluginProcess.Start();
                        PluginProcess.WaitForExit();
                        Output = PluginProcess.StandardOutput.ReadToEnd();
                        Outputs = Output.Split("\n");
                        Log.SaveLog("插件方法" + obj.Method + "成功被调用,参数:" + obj.Args);
                        Log.SaveLog("插件返回给用户的信息:" + Outputs[0]);
                        Log.SaveLog("插件返回给控制台的信息:" + Outputs[1]);
                        return Plugins.ProcessResult("Plugin.Post." + obj.Method, Outputs[0]);
                    default:
                        return "{\"status\":\"Error\",\"msg\":\"未指定的是否获取返回值.\"}";
                }
            }
            catch (Exception ex)
            {
                Log.SaveLog("插件方法" + obj.Method + "调用异常,参数:" + obj.Args);
                Log.SaveLog("触发异常:\n" + ex.ToString());
                return "{\"status\":\"Exception\",\"msg\":\"运行插件时出错.可能原因:1.名称输入有误,导致调用错插件;2.权限不够,请尝试提权后运行.\",\"exception\":\"" + ex.Message + "\"}";
            }
        }
        /// <summary>
        /// 以POST方法调用插件模块
        /// </summary>
        /// <param name="obj">JSON对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<string> PostApi([FromBody] Json obj)
        {
            return obj.Operation switch
            {
                "GetMethods" => GetMethods(obj.Password, obj.Num, obj.Page),
                "PostMethods" => PostMethods(obj.Password, obj.Num, obj.Page),
                "InsideProcessorMethods" => InsideProcessorMethods(obj.Password, obj.Num, obj.Page),
                "CommandProcessorMethods" => CommandProcessorMethods(obj.Password, obj.Num, obj.Page),
                "GetAllPlugins" => GetAllPlugins(obj.Password, obj.Num, obj.Page),
                "GetPointedPlugins" => GetPointedPlugins(obj.Password, obj.Method, obj.Path),
                _ => "{\"status\":\"Error\",\"msg\":\"Operation或方法不存在.请检查拼写与方法是否有误.\"}",
            };
        }
        /// <summary>
        /// Json对象
        /// </summary>
        public class Json
        {
            /// <summary>
            /// 方法名称
            /// </summary>
            public string Method { get; set; }
            /// <summary>
            /// 密码
            /// </summary>
            public string Password { get; set; }
            /// <summary>
            /// 参数
            /// </summary>
            public string Args { get; set; }
            /// <summary>
            /// 是否等待返回值
            /// </summary>
            public string WaitForResult { get; set; }
            /// <summary>
            /// 操作
            /// </summary>
            public string Operation { get; set; }
            /// <summary>
            /// 每页项目个数
            /// </summary>
            public string Num { get; set; }
            /// <summary>
            /// 页码
            /// </summary>
            public string Page { get; set; }
            /// <summary>
            /// 操作路径
            /// </summary>
            public string Path { get; set; }
        }
    }
}
