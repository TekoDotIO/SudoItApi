using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace SudoItApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("���ڳ�ʼ��Ӧ�ó���...");
            Initializater.Initializate();
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
