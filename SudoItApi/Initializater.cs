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
            Console.WriteLine("��ʼ���ɹ�");
            Log.SaveLog("Initializate succeed.");
        }
    }
}
