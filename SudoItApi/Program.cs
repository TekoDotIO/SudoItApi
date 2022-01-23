using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
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
            Console.WriteLine("欢迎使用" + AppInfo.AppName);
            Console.WriteLine(AppInfo.Copyright + " All right reserved.");
            Console.WriteLine("版本号:" + AppInfo.Version);
            Console.WriteLine("正在检测用户信息...");
            if (PersonalInfo.Name != null)
            {
                Console.WriteLine("欢迎来自" + PersonalInfo.Company + "的用户" + PersonalInfo.Name + "使用本应用程序!");
                if (PersonalInfo.InsidePermission)
                {
                    Console.WriteLine("开发者模式已激活!");
                    Console.WriteLine("App内部版本号:" + AppInfo.InsideVersion+"\n\n更新日志:\n"+AppInfo.UpdateLog);
                }
            }
            Console.WriteLine("正在初始化应用程序...");
            Initializater.Initializate();
            Console.WriteLine("从本地读取端口文件...");
            int Port = Convert.ToInt32(File.ReadAllText(@"./Port.txt"));
            Console.WriteLine("应用程序将运行在: *:" + Port);
            Log.SaveLog("应用程序端口号被设定为" + Port);
            string[] PortArg = new string[] { "--urls", "http://*:" + Port };
            Console.WriteLine("尝试运行主程序模块...");
            CreateHostBuilder(PortArg).Build().Run();
            //CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
