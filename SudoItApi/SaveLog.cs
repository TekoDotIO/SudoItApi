using System;
using System.IO;

namespace SudoItApi
{
    public class Log
    {
        /// <summary>
        /// 存储日志
        /// </summary>
        /// <param name="message">日志信息</param>
        public static void SaveLog(string message)
        {
            message = DateTime.Now.ToString() + " " + message;
            Directory.CreateDirectory("./Log/");
            System.IO.File.AppendAllText(@"./Log/Console" + DateTime.Now.ToString("yyyy-MM-dd") + ".log", "\r\n" + message);
            Console.WriteLine(message);
        }
    }
}
