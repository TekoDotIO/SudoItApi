using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace SudoItApi
{
    public class Program
    {
        /// <summary>
        /// Ӧ�ó��������ڵ�
        /// </summary>
        /// <param name="args">����ִ̨�в���</param>
        public static void Main(string[] args)
        {
            Console.WriteLine("���ڳ�ʼ��Ӧ�ó���...");
            Initializater.Initializate();
            Console.WriteLine("�ӱ��ض�ȡ�˿��ļ�...");
            int Port = Convert.ToInt32(File.ReadAllText(@"./Port.txt"));
            Console.WriteLine("Ӧ�ó���������: *:" + Port);
            Log.SaveLog("Ӧ�ó���˿ںű��趨Ϊ" + Port);
            string[] PortArg = new string[] { "--urls", "http://*:" + Port };
            Console.WriteLine("��������������ģ��...");
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
