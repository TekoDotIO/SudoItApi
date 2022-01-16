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
}
