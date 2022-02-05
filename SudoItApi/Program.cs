using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.IO;

namespace SudoItApi
{
    public class Program
    {
        /// <summary>
        /// 应用程序的主入口点
        /// </summary>
        /// <param name="args">控制台执行参数</param>
        public static void Main(string[] args)
        {
            //App信息可在AppInfo.cs里面设置
            Console.WriteLine("欢迎使用" + AppInfo.AppName);
            Console.WriteLine(AppInfo.Copyright + " All right reserved.");
            Console.WriteLine("版本号:" + AppInfo.Version);
            Console.WriteLine("正在检测用户信息...");
            if (PersonalInfo.Name != null)//定制选项
            {
                Console.WriteLine("欢迎来自" + PersonalInfo.Company + "的用户" + PersonalInfo.Name + "使用本应用程序!");
                if (PersonalInfo.InsidePermission)
                {
                    Console.WriteLine("开发者模式已激活!");
                    Console.WriteLine("App内部版本号:" + AppInfo.InsideVersion+"\n\n更新日志:\n"+AppInfo.UpdateLog);
                    //以后开发者会支持更多高级功能
                }
            }
            Console.WriteLine("正在初始化应用程序...");
            Initializater.Initializate();
            //调用初始化,间Initializater.cs
            Plugins.InitializatePlugins();
            Plugins.ReportPlugins();
            Console.WriteLine("尝试运行主程序模块...");
            Thread MvcThread = new Thread(StartMvc)
            {
                Name = "SudoItApi-MVC"
            };
            MvcThread.Start();
            string Result = ReadCommand(false);
            switch (Result)
            {
                case "reboot":
                    Console.WriteLine("重启应用程序的方式不再受支持,因为此方式会导致MVC模块崩溃.");
                    ReadCommand(true);
                    break;
                case "shutdown":
                    MvcThread.Interrupt();
                    break;
                default:
                    ReadCommand(true);
                    break;
            }
        }
        /// <summary>
        /// 带初始化参数的命令模式
        /// </summary>
        /// <param name="Initializated">初始化状态</param>
        public static string ReadCommand(bool Initializated)
        {
            if (Initializated)
            {
                return ReadCommand();
            }
            else
            {
                Thread.Sleep(3000);
                Log.SaveLog("MVC控制器进入守护状态,命令模式已启动");
                Log.SaveLog("输入Help以获取命令帮助!");
                return ReadCommand();
            }
        }
        /// <summary>
        /// 命令读取器
        /// </summary>
        /// <returns></returns>
        public static string ReadCommand()
        {
            Console.Write(">");
            string CommandText = Console.ReadLine();
            string Result = CommandReader.Execute(CommandText);
            if (Result != "Keep") return Result;
            ReadCommand();
            return "";
        }
        /// <summary>
        /// 启动Mvc
        /// </summary>
        public static void StartMvc()
        {
            Console.WriteLine("从本地读取端口文件...");
            int Port = Convert.ToInt32(File.ReadAllText(@"./Setting/Port.txt"));
            Console.WriteLine("应用程序将运行在: *:" + Port);
            Log.SaveLog("应用程序端口号被设定为" + Port);
            //指定Mvc加载时参数
            //示例脚本: dotnet xxx.dll --urls http://*:5000
            //URL需要指定为*,否则通过其他入口访问程序可能会拒绝连接请求
            string[] PortArg = new string[] { "--urls", "http://*:" + Port };
            try
            {
                CreateHostBuilder(PortArg).Build().Run();
            }
            catch
            {
                return;
            }
            //构建Mvc模块
            //CreateHostBuilder(args).Build().Run();
        }
        /// <summary>
        /// 由Visual Studio生成的模块-加载Mvc
        /// </summary>
        /// <param name="args">操作</param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
