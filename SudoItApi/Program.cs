using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

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
            Console.WriteLine("正在初始化应用程序...");
            Initializater.Initializate();
            Console.WriteLine("尝试运行主程序模块...\n\n");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
