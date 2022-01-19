using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;

namespace SudoItApi
{
    public class Initializater
    {
        /// <summary>
        /// ��ʼ��
        /// </summary>
        public static void Initializate()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            if (!File.Exists(@"./deletemetoreset.each"))
            {
                Console.Clear();
                Console.WriteLine("��ӭʹ��SudoIt!ϵͳ��⵽���ǵ�һ������SudoIt(������ѳ���ļ����ļ��Ե���hh),���,����Ҫ��ɼ����򵥵Ĳ���,����֮����ſ��Ծ�������SudoIt!");
                Console.WriteLine("���س�������");
                Console.ReadKey();
                Console.Clear();
                Console.WriteLine("��һ��");
                Console.WriteLine("�û���˽����");
                Console.WriteLine("\n\n�໥�Ƽ��߶����Ӷ��û���Ϣ�ı��ܹ���,���ĸ�����Ϣ������ļ�����ʹ�ñ�������κβ��ֶ�û�б��ϴ����κεط�.����Ŀ��GitHub��AGPLЭ�鿪Դ,���������ɲ鿴��������κδ���.����Ϊ��ά�������ȶ���,���Ǳ����ȡ������־��Ϣ�������ϴ����໥�Ƽ�������.\n���������ʹ�ñ����,����ʾͬ��������Ϣ���䲢��ͬ���໥�Ƽ����·������û�Э��.");
                Console.WriteLine("\n�������Ⲣͬ��,�밴�س�������.\n�������ͬ��,���˳�����.");
                Console.ReadKey();
                Console.Clear();
                Console.WriteLine("�ڶ���");
                Console.WriteLine("���ö�������");
                Console.WriteLine("\n\nSudoIt��Ҫ������һ������������������������.\n��ע��:��������Ӧ�ð�����д�ַ�,Сд�ַ�,��?&/\\=��֮��������ַ�");
                Console.WriteLine("������������벢���»س�:");
                string Password = Console.ReadLine();
                File.Create(@"./Password.txt").Close();
                File.WriteAllText(@"./Password.txt", Password);
                Console.Clear();
                Console.WriteLine("������");
                Console.WriteLine("�������ж˿�");
                Console.WriteLine("\n\nSudoIt��Ҫ������һ�����ж˿����������з���.\n��ע��:���Ķ˿ں�Ӧ����һ��0~65535֮���������,����\"5000\"");
                Console.WriteLine("����ǿ�ҽ�����ʹ��5000��Ϊ���ж˿ں�,��ΪSudoIt��Ĭ�����ý�5000��Ϊ�����˿�");
                Console.WriteLine("������������ж˿ڲ����»س�:");
                string Port = Console.ReadLine();
                PortHelper portHelper = new PortHelper();
                if (portHelper.portInUse(Convert.ToInt32(Port), PortType.TCP))
                {
                    Console.WriteLine("��ʼ��ʧ��:�ö˿��ѱ�ռ��\n��ر�ռ�ö˿ڵĳ��������˿ں�����");
                    Console.ReadKey();
                    return;
                }
                if(Convert.ToInt32(Port)>65535||Convert.ToInt32(Port)<=0)
                {
                    Console.WriteLine("��ʼ��ʧ��:�˿ڸ�ʽ����ȷ\n���Ķ˿ں�Ӧ����һ��0~65535֮���������,����\"5000\"\n�����³�ʼ��Ӧ�ó���");
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
                Console.WriteLine("���ö�������");
                Console.WriteLine("\n\nSudoIt��Ҫ������һ������������������������.\n��ע��:��������Ӧ�ð�����д�ַ�,Сд�ַ�,��?&/\\=��֮��������ַ�");
                Console.WriteLine("�������������:");
                string Password = Console.ReadLine();
                File.Create(@"./Password.txt").Close();
                File.WriteAllText(@"./Password.txt", Password);
                Console.Clear();
            }
            if (!File.Exists(@"./Port.txt"))
            {
                Console.WriteLine("�������ж˿�");
                Console.WriteLine("\n\nSudoIt��Ҫ������һ�����ж˿����������з���.\n��ע��:���Ķ˿ں�Ӧ����һ��0~65535֮���������,����\"5000\"");
                Console.WriteLine("����ǿ�ҽ�����ʹ��5000��Ϊ���ж˿ں�,��ΪSudoIt��Ĭ�����ý�5000��Ϊ�����˿�");
                Console.WriteLine("������������ж˿ڲ����»س�:");
                string Port = Console.ReadLine();
                PortHelper portHelper = new PortHelper();
                if (portHelper.portInUse(Convert.ToInt32(Port), PortType.TCP))
                {
                    Console.WriteLine("��ʼ��ʧ��:�ö˿��ѱ�ռ��\n��ر�ռ�ö˿ڵĳ��������˿ں�����");
                    Console.ReadKey();
                    return;
                }
                if (Convert.ToInt32(Port) > 65535 || Convert.ToInt32(Port) <= 0)
                {
                    Console.WriteLine("��ʼ��ʧ��:�˿ڸ�ʽ����ȷ\n���Ķ˿ں�Ӧ����һ��0~65535֮���������,����\"5000\"\n�����³�ʼ��Ӧ�ó���");
                    Console.ReadKey();
                    return;
                }
                File.Create(@"./Port.txt").Close();
                File.WriteAllText(@"./Port.txt", Port);
                Console.Clear();
            }
            Console.WriteLine("��ʼ���ɹ�");
            Log.SaveLog("��ʼ�������");
        }
    }
    class PortHelper
    {

        #region ָ�����͵Ķ˿��Ƿ��Ѿ���ʹ����
        /// <summary>
        /// ָ�����͵Ķ˿��Ƿ��Ѿ���ʹ����
        /// </summary>
        /// <param name="port">�˿ں�</param>
        /// <param name="type">�˿�����</param>
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

    #region �˿�ö������
    /// <summary>
    /// �˿�����
    /// </summary>
    enum PortType
    {
        /// <summary>
        /// TCP����
        /// </summary>
        TCP,
        /// <summary>
        /// UDP����
        /// </summary>
        UDP
    }
    #endregion
}
