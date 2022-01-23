using Microsoft.AspNetCore.Mvc;
using System;

namespace SudoItApi.Controllers
{
    #region 命令系统
    /// <summary>
    /// 服务器命令控制器
    /// </summary>
    [ApiController]
    [Route("SudoIt/[controller]/[action]")]
    public class CommandController : ControllerBase
    {
        #region GET部分
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
                Log.SaveLog(ip + " 执行了命令 \"" + Command + "\"");
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
                Log.SaveLog(ip + " 安全执行了 \"" + Command + "\"");
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
        #endregion
        #region POST部分
        /// <summary>
        /// POST方式执行命令
        /// </summary>
        /// <param name="obj">JSON对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<string> PostApi([FromBody] Json obj)
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            Log.SaveLog(ip + "使用POST方式访问了命令模块");
            switch (obj.WaitForExit)
            {
                case "True":
                case "true":
                case "TRUE":
                case "1":
                    return ExecuteCommand(obj.Command, obj.Password);
                case "False":
                case "FALSE":
                case "false":
                case "0":
                    return SafeExecute(obj.Command, obj.Password);
                default:
                    return "{\"status\":\"Error\",\"msg\":\"未指定是否等待返回值(WaitForExit)\"}";
            }
        }
        public class Json
        {
            public string WaitForExit { get; set; }
            public string Command { get; set; }
            public string Password { get; set; }
        }
        #endregion
    }
    #endregion
}
