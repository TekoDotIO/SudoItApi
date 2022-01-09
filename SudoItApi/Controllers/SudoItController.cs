using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudoItApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SudoItController : ControllerBase
    {
        /// <summary>
        /// 获取服务端状态
        /// </summary>
        /// <returns>词典</returns>
        [HttpGet]
        public ActionResult<string> Status()
        {
            string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            Log.SaveLog(ip + " got your server status.");
            return "{\"status\":\"OK\"}";
        }
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="Command">命令文本</param>
        /// <param name="Password">密码</param>
        /// <returns>词典(结果或错误)</returns>
        [HttpGet]
        public ActionResult<string> ExecuteCommand(string Command,string Password)
        {
            if (SetAndAuth.Auth(Password))
            {
                string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                Log.SaveLog(ip + " execated \""+Command+"\"");
                string result = Cmd.RunCmd(Command);
                return "{\"status\":\"Done\",\"msg\":\"" + result + "\"}";
            }
            else
            {
                string ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                Log.SaveLog(ip + " tried to execate \"" + Command + "\" , but he/she entered a wrong password.");
                HttpContext.Response.StatusCode = 403;
                return "{\"status\":\"Error\",\"msg\":\"Password is not correct.\"}";
            }
        }
    }
}
