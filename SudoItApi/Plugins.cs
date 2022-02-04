using System.Diagnostics;
using System.IO;

namespace SudoItApi
{
    public class Plugins
    {
        public static void InitializatePlugins()
        {
            Directory.CreateDirectory("./Plugins/");
            Directory.CreateDirectory("./Plugins/POST-Methods/");//需要POST数据的自定义方法
            Directory.CreateDirectory("./Plugins/GET-Methods/");//只需GET数据的自定义方法
            Directory.CreateDirectory("./Plugins/InsideProcessor/");//对内部方法结果进行修改的方法
            string[] PluginList = Directory.GetFiles("./Plugins/");
            string Args = "--Initializate --Version " + AppInfo.Version + " --AppName " + AppInfo.AppName;
            //如:"Plugin.exe --Initializate --Version v.1.0.0.4 --AppName $udo!T-Api-Server"
            if (PersonalInfo.InsidePermission)
            {
                Args += " --Dev";
            }
            if (PersonalInfo.Name != null)
            {
                Args += " --User " + PersonalInfo.Name;
            }
            if (PersonalInfo.Company != null)
            {
                Args += " --Organization " + PersonalInfo.Company;
            }
            foreach (string PluginName in PluginList)
            {
                string Name = PluginName.Split("\\")[^1].Split("/")[^1];
                Log.SaveLog("开始加载插件" + Name);
                Process pluginInitializater = new Process();
                pluginInitializater.StartInfo.FileName = PluginName;
                pluginInitializater.StartInfo.CreateNoWindow = true;
                pluginInitializater.StartInfo.Arguments = Args;
                pluginInitializater.StartInfo.RedirectStandardInput = true; //接受来自调用程序的输入信息
                pluginInitializater.StartInfo.RedirectStandardOutput = true; //由调用程序获取输出信息
                pluginInitializater.StartInfo.RedirectStandardError = true; //重定向标准错误输出
                pluginInitializater.Start();
                pluginInitializater.WaitForExit();
                string Output = pluginInitializater.StandardOutput.ReadToEnd();
                Log.SaveLog("插件" + Name + "初始化完成.来自插件的信息:");
                Log.SaveLog(Output);
            }
            Log.SaveLog("插件已全部加载完成.");
        }
        public static void ReportPlugins()
        {
            Directory.CreateDirectory("./Plugins/");
            Directory.CreateDirectory("./Plugins/POST-Methods/");//需要POST数据的自定义方法
            Directory.CreateDirectory("./Plugins/GET-Methods/");//只需GET数据的自定义方法
            Directory.CreateDirectory("./Plugins/InsideProcessor/");//对内部方法结果进行修改的方法
            string[] POSTs = Directory.GetDirectories("./Plugins/POST-Methods/");
            string[] GETs = Directory.GetDirectories("./Plugins/GET-Methods/");
            string[] INSIDEs = Directory.GetDirectories("./Plugins/InsideProcessor/");
            string POST_Methods = "", GET_Methods = "", Inside_Methods = "";
            foreach (string Method in POSTs)
            {
                POST_Methods += Method + "\n";
            }
            foreach (string Method in GETs)
            {
                GET_Methods += Method + "\n";
            }
            foreach (string Method in INSIDEs)
            {
                Inside_Methods += Method + "\n";
            }
            Log.SaveLog("\n已加载的POST类插件所提供的方法有:\n" + POST_Methods);
            Log.SaveLog("\n已加载的GET类插件所提供的方法有:\n" + GET_Methods);
            Log.SaveLog("\n已加载的修改器插件所属目录有:\n" + Inside_Methods);
        }
        public static string[] GetApis()
        {
            Directory.CreateDirectory("./Plugins/");
            Directory.CreateDirectory("./Plugins/GET-Methods/");//只需GET数据的自定义方法
            string[] PluginList = Directory.GetFiles("./Plugins/GET-Methods/");
            string[] NameList = new string[PluginList.Length];
            int num = 0;
            foreach (string Path in PluginList)
            {
                NameList[num] = Path.Split("\\")[^1].Split("/")[^1];
                num++;
            }
            return NameList;
        }
    }
}
