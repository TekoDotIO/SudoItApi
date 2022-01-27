using System.IO;

namespace SudoItApi
{
    public class CommandReader
    {
        public static string Execute(string Command)
        {
            string[] Cmds = Command.Split(' ');
            switch(Cmds[0])
            {
                case "help":
                    return "Keep";
                case "stop":
                case "shutdown":
                    return "shutdown";
                case "reboot":
                case "restart":
                    return "reboot";
                case "unban":
                    if (File.Exists("./banips/" + Cmds[1] + ".txt")) File.Delete("./banips/" + Cmds[1] + ".txt");
                    Log.SaveLog("用户使用命令行解除了以下地址的IP屏蔽" + Cmds[1]);
                    return "Keep";
                default:
                    return "Keep";
            }
        }
    }
}
