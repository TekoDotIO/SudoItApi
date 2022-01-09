using System;
using System.IO;

namespace SudoItApi
{
    public class Log
    {
        public static void SaveLog(string message)
        {
            message = DateTime.Now.ToString() + " " + message;
            System.IO.File.AppendAllText(@".\Console" + DateTime.Now.ToString("yyyy-MM-dd") + ".log", "\r\n" + DateTime.Now.ToString() + "\r\n" + message);
            Console.WriteLine(message);
        }
    }
}
