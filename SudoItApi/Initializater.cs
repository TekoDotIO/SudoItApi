using System;
using System.IO;

namespace SudoItApi
{
    public class Initializater
    {
        public static void Initializate()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            if (!File.Exists(@"./deletemetoreset.each"))
            {
                Console.Clear();
                Console.WriteLine("欢迎使用SudoIt!系统检测到你是第一次运行SudoIt(或者你把程序的记忆文件吃掉了hh),因此,你需要完成几个简单的步骤,在这之后你才可以尽情享受SudoIt!");
                Console.WriteLine("按回车键继续");
                Console.ReadKey();
                Console.Clear();
                Console.WriteLine("第一步");
                Console.WriteLine("用户隐私声明");
                Console.WriteLine("\n\n相互科技高度重视对用户信息的保密工作,您的个人信息与个人文件在您使用本程序的任何部分都没有被上传到任何地方.本项目在GitHub以AGPL协议开源,您可以轻松查看本程序的任何代码.但是为了维护程序稳定性,我们必须获取您的日志信息并将其上传到相互科技服务器.\n如果您继续使用本软件,即表示同意上述信息传输并且同意相互科技最新发布的用户协议.");
                Console.WriteLine("\n如果您理解并同意,请按回车键继续.\n如果您不同意,请退出程序.");
                Console.ReadKey();
                Console.Clear();
                Console.WriteLine("第二步");
                Console.WriteLine("设置独立密码");
                Console.WriteLine("\n\nSudoIt需要您设置一个独立密码以区分您和他人.\n请注意:您的密码应该包含大写字符,小写字符,除?&/\\=等之外的特殊字符");
                Console.WriteLine("请设置你的密码并按下回车:");
                string Password = Console.ReadLine();
                File.Create(@"./Password.txt").Close();
                File.WriteAllText(@"./Password.txt", Password);
                Console.Clear();
                File.Create(@"./deletemetoreset.each").Close();
            }
            if (!File.Exists(@"./Password.txt"))
            {
                Console.WriteLine("设置独立密码");
                Console.WriteLine("\n\nSudoIt需要您设置一个独立密码以区分您和他人.\n请注意:您的密码应该包含大写字符,小写字符,除?&/\\=等之外的特殊字符");
                Console.WriteLine("请设置你的密码:");
                string Password = Console.ReadLine();
                File.Create(@"./Password.txt").Close();
                File.WriteAllText(@"./Password.txt", Password);
                Console.Clear();
            }
            Console.WriteLine("初始化成功");
            Log.SaveLog("Initializate succeed.");
        }
    }
}
