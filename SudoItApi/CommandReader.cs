using System;
using System.Diagnostics;
using System.IO;

namespace SudoItApi
{
    /// <summary>
    /// Shell指令类
    /// </summary>
    public class CommandReader
    {
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="Command">命令文本</param>
        /// <returns></returns>
        public static string Execute(string Command)
        {
            string[] Cmds = Command.Split(' ');
            string[] HelpTexts = new string[8];
            HelpTexts[0] = "help - 输出帮助文档";
            HelpTexts[1] = "stop或shutdown - 停止MVC控制器的服务,并退出程序";
            HelpTexts[2] = "reboot或restart - 已废弃";
            HelpTexts[3] = "unban [已被屏蔽的IP地址] - 对某个被屏蔽的用户解除屏蔽";
            HelpTexts[4] = "ban [需要屏蔽的IP地址] - 屏蔽来自指定用户的连接";
            HelpTexts[5] = "load [插件名称] - 初始化指定插件";
            HelpTexts[6] = "reload - 重新加载所有插件";
            HelpTexts[7] = "[自定义方法名称] [自定义参数] - 调用插件执行命令";
            try
            {
                string Cmd0 = Cmds[0];//如果不测试,Ctrl+C会抛异常.
            }
            catch
            {
                return "";
            }
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
                case "load":
                    try
                    {
                        string Name = Cmds[1];
                        Log.SaveLog("开始加载插件" + Name);
                        string Args = "--Initializate --Version " + AppInfo.Version + " --AppName " + AppInfo.AppName;
                        Process pluginInitializater = new Process();
                        pluginInitializater.StartInfo.FileName = "./Plugins/" + Name;
                        pluginInitializater.StartInfo.CreateNoWindow = true;
                        pluginInitializater.StartInfo.Arguments = Args;
                        pluginInitializater.StartInfo.RedirectStandardInput = true; //接受来自调用程序的输入信息
                        pluginInitializater.StartInfo.RedirectStandardOutput = true; //由调用程序获取输出信息
                        pluginInitializater.StartInfo.RedirectStandardError = true; //重定向标准错误输出
                        pluginInitializater.Start();
                        pluginInitializater.WaitForExit();
                        string Output = pluginInitializater.StandardOutput.ReadToEnd();
                        Log.SaveLog("插件" + Name + "初始化完成.来自插件的信息:");
                        Log.SaveLog(Output);
                        return "Keep";
                    }
                    catch
                    {
                        Log.SaveLog("插件无法加载,原因可能是没有找到插件或权限不够");
                        return "Keep";
                    }
                case "reload":
                    Plugins.InitializatePlugins();
                    return "Keep";
                default:
                    Plugins.ProcessCommand(Cmds[0], Command);
                    return "Keep";
            }
        }
    }
}
