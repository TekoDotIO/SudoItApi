using System;
using System.IO;

namespace SudoItApi
{
    public class CommandReader
    {
        public static string Execute(string Command)
        {
            string[] Cmds = Command.Split(' ');
            string[] HelpTexts = new string[6];
            HelpTexts[0] = "help - 输出帮助文档";
            HelpTexts[1] = "stop或shutdown - 停止MVC控制器的服务,并退出程序";
            HelpTexts[2] = "reboot或restart - 已废弃";
            HelpTexts[3] = "unban [已被屏蔽的IP地址] - 对某个被屏蔽的用户解除屏蔽";
            HelpTexts[4] = "ban [需要屏蔽的IP地址] - 屏蔽来自指定用户的连接";
            switch(Cmds[0])
            {
                case "help":
                    foreach(string HelpText in HelpTexts)
                    {
                        Console.WriteLine(HelpText);
                    }
                    return "Keep";
                case "stop":
                case "shutdown":
                    return "shutdown";
                case "reboot":
                case "restart":
                    return "reboot";
                case "unban":
                    try
                    {
                        if (File.Exists("./banips/" + Cmds[1] + ".txt")) File.Delete("./banips/" + Cmds[1] + ".txt");
                        Log.SaveLog("用户使用命令行解除了以下地址的IP屏蔽" + Cmds[1]);
                    }
                    catch
                    {
                        Log.SaveLog("无效参数");
                    }
                    return "Keep";
                case "ban":
                    try
                    {
                        string ip = Cmds[1];
                        File.WriteAllText("./banips/" + ip + ".txt", File.ReadAllText("./Setting/ErrTimes.txt"));
                        Log.SaveLog("配置文件已创建,已屏蔽用户" + ip);
                    }
                    catch
                    {
                        Log.SaveLog("无效参数");
                    }
                    return "Keep";
                default:
                    Log.SaveLog("无效命令.执行help以获取帮助");
                    return "Keep";
            }
        }
    }
}
