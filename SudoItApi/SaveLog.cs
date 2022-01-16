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
            System.IO.File.AppendAllText(@".\Console" + DateTime.Now.ToString("yyyy-MM-dd") + ".log", "\r\n" + message);
            Console.WriteLine(message);
        }
    }
}
