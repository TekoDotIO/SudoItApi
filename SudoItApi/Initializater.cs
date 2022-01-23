using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;

namespace SudoItApi
{
    public class Initializater
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initializate()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            if (!File.Exists(@"./deletemetoreset.each"))
            {
                Console.WriteLine("\n\n\n欢迎使用SudoIt!系统检测到你是第一次运行SudoIt(或者你把程序的记忆文件吃掉了hh),因此,你需要完成几个简单的步骤,在这之后你才可以尽情享受SudoIt!");
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
                Console.WriteLine("第三步");
                Console.WriteLine("设置运行端口");
                Console.WriteLine("\n\nSudoIt需要您设置一个运行端口以在网络中访问.\n请注意:您的端口号应该是一个0~65535之间的正整数,例如\"5000\"");
                Console.WriteLine("我们强烈建议您使用5000作为运行端口号,因为SudoIt的默认设置将5000作为正常端口");
                Console.WriteLine("请设置你的运行端口并按下回车:");
                string Port = Console.ReadLine();
                PortHelper portHelper = new PortHelper();
                if (portHelper.portInUse(Convert.ToInt32(Port), PortType.TCP))
                {
                    Console.WriteLine("初始化失败:该端口已被占用\n请关闭占用端口的程序或更换端口后重试");
                    Console.ReadKey();
                    return;
                }
                if(Convert.ToInt32(Port)>65535||Convert.ToInt32(Port)<=0)
                {
                    Console.WriteLine("初始化失败:端口格式不正确\n您的端口号应该是一个0~65535之间的正整数,例如\"5000\"\n请重新初始化应用程序");
                    Console.ReadKey();
                    return;
                }
                File.Create(@"./Port.txt").Close();
                File.WriteAllText(@"./Port.txt", Port);
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
            if (!File.Exists(@"./Port.txt"))
            {
                Console.WriteLine("设置运行端口");
                Console.WriteLine("\n\nSudoIt需要您设置一个运行端口以在网络中访问.\n请注意:您的端口号应该是一个0~65535之间的正整数,例如\"5000\"");
                Console.WriteLine("我们强烈建议您使用5000作为运行端口号,因为SudoIt的默认设置将5000作为正常端口");
                Console.WriteLine("请设置你的运行端口并按下回车:");
                string Port = Console.ReadLine();
                PortHelper portHelper = new PortHelper();
                if (portHelper.portInUse(Convert.ToInt32(Port), PortType.TCP))
                {
                    Console.WriteLine("初始化失败:该端口已被占用\n请关闭占用端口的程序或更换端口后重试");
                    Console.ReadKey();
                    return;
                }
                if (Convert.ToInt32(Port) > 65535 || Convert.ToInt32(Port) <= 0)
                {
                    Console.WriteLine("初始化失败:端口格式不正确\n您的端口号应该是一个0~65535之间的正整数,例如\"5000\"\n请重新初始化应用程序");
                    Console.ReadKey();
                    return;
                }
                File.Create(@"./Port.txt").Close();
                File.WriteAllText(@"./Port.txt", Port);
                Console.Clear();
            }
            Console.WriteLine("初始化成功");
            Log.SaveLog("初始化已完成");
        }
    }
    class PortHelper
    {

        #region 指定类型的端口是否已经被使用了
        /// <summary>
        /// 指定类型的端口是否已经被使用了
        /// </summary>
        /// <param name="port">端口号</param>
        /// <param name="type">端口类型</param>
        /// <returns></returns>
        public bool portInUse(int port, PortType type)
        {
            bool flag = false;
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipendpoints = null;
            if (type == PortType.TCP)
            {
                ipendpoints = properties.GetActiveTcpListeners();
            }
            else
            {
                ipendpoints = properties.GetActiveUdpListeners();
            }
            foreach (IPEndPoint ipendpoint in ipendpoints)
            {
                if (ipendpoint.Port == port)
                {
                    flag = true;
                    break;
                }
            }
            ipendpoints = null;
            properties = null;
            return flag;
        }
        #endregion

    }

    #region 端口枚举类型
    /// <summary>
    /// 端口类型
    /// </summary>
    enum PortType
    {
        /// <summary>
        /// TCP类型
        /// </summary>
        TCP,
        /// <summary>
        /// UDP类型
        /// </summary>
        UDP
    }
    #endregion
}
