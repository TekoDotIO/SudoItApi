using Microsoft.AspNetCore.Mvc;
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
        /// 调用GET方法插件
        /// </summary>
        /// <param name="Method">方法</param>
        /// <param name="Password">密码</param>
        /// <param name="Args">参数</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> Get(string Method, string Password, string Args = "")
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
                PluginProcess.StartInfo.RedirectStandardInput = true; //接受来自调用程序的输入信息
                PluginProcess.StartInfo.RedirectStandardOutput = true; //由调用程序获取输出信息
                PluginProcess.StartInfo.RedirectStandardError = true; //重定向标准错误输出
                PluginProcess.Start();
                PluginProcess.WaitForExit();
                string Output = PluginProcess.StandardOutput.ReadToEnd();
                string[] Outputs = Output.Split("\n");
                Log.SaveLog("插件方法" + Method + "成功被调用,参数:" + Args);
                Log.SaveLog("插件返回给用户的信息:" + Outputs[0]);
                Log.SaveLog("插件返回给控制台的信息:" + Outputs[1]);
                return "{\"status\":\"OK\",\"msg\":\"" + Output + "\"}";
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
                PluginProcess.StartInfo.RedirectStandardInput = true; //接受来自调用程序的输入信息
                PluginProcess.StartInfo.RedirectStandardOutput = true; //由调用程序获取输出信息
                PluginProcess.StartInfo.RedirectStandardError = true; //重定向标准错误输出
                PluginProcess.Start();
                PluginProcess.WaitForExit();
                string Output = PluginProcess.StandardOutput.ReadToEnd();
                string[] Outputs = Output.Split("\n");
                Log.SaveLog("插件方法" + obj.Method + "成功被调用,参数:" + obj.Args);
                Log.SaveLog("插件返回给用户的信息:" + Outputs[0]);
                Log.SaveLog("插件返回给控制台的信息:" + Outputs[1]);
                return "{\"status\":\"OK\",\"msg\":\"" + Output + "\"}";
            }
            catch (Exception ex)
            {
                Log.SaveLog("插件方法" + obj.Method + "调用异常,参数:" + obj.Args);
                Log.SaveLog("触发异常:\n" + ex.ToString());
                return "{\"status\":\"Exception\",\"msg\":\"运行插件时出错.可能原因:1.名称输入有误,导致调用错插件;2.权限不够,请尝试提权后运行.\",\"exception\":\"" + ex.Message + "\"}";
            }
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
        }
    }
}
