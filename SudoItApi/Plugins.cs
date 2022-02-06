using System;
using System.Diagnostics;
using System.IO;

namespace SudoItApi
{
    /// <summary>
    /// 插件类
    /// </summary>
    public class Plugins
    {
        /// <summary>
        /// 初始化所有插件
        /// </summary>
        public static void InitializatePlugins()
        {
            Directory.CreateDirectory("./Plugins/");
            Directory.CreateDirectory("./Plugins/POST-Methods/");//需要POST数据的自定义方法
            Directory.CreateDirectory("./Plugins/GET-Methods/");//只需GET数据的自定义方法
            Directory.CreateDirectory("./Plugins/InsideProcessor/");//对内部方法结果进行修改的方法
            Directory.CreateDirectory("./Plugins/CommandProcessor/");//命令处理器方法
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
        /// <summary>
        /// 输出插件列表
        /// </summary>
        public static void ReportPlugins()
        {
            Directory.CreateDirectory("./Plugins/");
            Directory.CreateDirectory("./Plugins/POST-Methods/");//需要POST数据的自定义方法
            Directory.CreateDirectory("./Plugins/GET-Methods/");//只需GET数据的自定义方法
            Directory.CreateDirectory("./Plugins/InsideProcessor/");//对内部方法结果进行修改的方法
            Directory.CreateDirectory("./Plugins/CommandProcessor/");//命令处理器方法
            string[] POSTs = Directory.GetDirectories("./Plugins/POST-Methods/");
            string[] GETs = Directory.GetDirectories("./Plugins/GET-Methods/");
            string[] INSIDEs = Directory.GetDirectories("./Plugins/InsideProcessor/");
            string[] CMDs = Directory.GetDirectories("./Plugins/CommandProcessor/");
            string POST_Methods = "", GET_Methods = "", Inside_Methods = "", Command_Methods = "";
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
            foreach (string Method in CMDs)
            {
                Command_Methods += Method + "\n";
            }
            Log.SaveLog("\n已加载的POST类插件所提供的方法有:\n" + POST_Methods);
            Log.SaveLog("\n已加载的GET类插件所提供的方法有:\n" + GET_Methods);
            Log.SaveLog("\n已加载的修改器插件所属目录有:\n" + Inside_Methods);
            Log.SaveLog("\n已加载的命令行插件方法有:\n" + Command_Methods);
        }
        /// <summary>
        /// 获取所有GET插件
        /// </summary>
        /// <returns></returns>
        public static string[] GetApis()
        {
            Directory.CreateDirectory("./Plugins/");
            Directory.CreateDirectory("./Plugins/GET-Methods/");//只需GET数据的自定义方法
            string[] PluginList = Directory.GetFiles("./Plugins/GET-Methods/");
            string[] NameList = new string[PluginList.Length];
            int num = 0;
            foreach (string Path in PluginList)
            {
                NameList[num] = Path.Split("\\")[^1].Split("/")[^1].Replace(".txt", "");
                num++;
            }
            return NameList;
        }
        /// <summary>
        /// 获取所有POST插件
        /// </summary>
        /// <returns></returns>
        public static string[] PostApis()
        {
            Directory.CreateDirectory("./Plugins/");
            Directory.CreateDirectory("./Plugins/POST-Methods/");//POST数据的自定义方法
            string[] PluginList = Directory.GetFiles("./Plugins/POST-Methods/");
            string[] NameList = new string[PluginList.Length];
            int num = 0;
            foreach (string Path in PluginList)
            {
                NameList[num] = Path.Split("\\")[^1].Split("/")[^1].Replace(".txt", "");
                num++;
            }
            return NameList;
        }
        /// <summary>
        /// 获取所有内部修改器插件
        /// </summary>
        /// <returns></returns>
        public static string[] InsideApis()
        {
            Directory.CreateDirectory("./Plugins/");
            Directory.CreateDirectory("./Plugins/InsideProcessor/");//修改数据的自定义方法
            string[] PluginList = Directory.GetFiles("./Plugins/InsideProcessor/");
            string[] NameList = new string[PluginList.Length];
            int num = 0;
            foreach (string Path in PluginList)
            {
                NameList[num] = Path.Split("\\")[^1].Split("/")[^1].Replace(".txt", "");
                num++;
            }
            return NameList;
        }
        /// <summary>
        /// 获取所有命令行插件
        /// </summary>
        /// <returns></returns>
        public static string[] CommandApis()
        {
            Directory.CreateDirectory("./Plugins/");
            Directory.CreateDirectory("./Plugins/CommandProcessor/");//自定义方法
            string[] PluginList = Directory.GetFiles("./Plugins/CommandProcessor/");
            string[] NameList = new string[PluginList.Length];
            int num = 0;
            foreach (string Path in PluginList)
            {
                NameList[num] = Path.Split("\\")[^1].Split("/")[^1].Replace(".txt", "");
                num++;
            }
            return NameList;
        }
        /// <summary>
        /// 处理结果
        /// </summary>
        /// <param name="Method">方法</param>
        /// <param name="Result">原结果</param>
        /// <returns></returns>
        public static string ProcessResult(string Method, string Result)
        {
            string[] Apis = InsideApis(); 
            if (Apis.Length == 0)
            {
                return Result;
            }
            foreach (string Api in Apis)
            {
                Process PluginProcess = new Process();
                string Processor = File.ReadAllText("./Plugins/InsideProcessor/" + Api + ".txt");
                PluginProcess.StartInfo.FileName = "./Plugins/" + Processor;
                PluginProcess.StartInfo.CreateNoWindow = true;
                PluginProcess.StartInfo.Arguments = "--Method " + Method + " --Data \"" + Result + "\"";
                string Output;
                string[] Outputs;
                PluginProcess.StartInfo.RedirectStandardInput = true; //接受来自调用程序的输入信息
                PluginProcess.StartInfo.RedirectStandardOutput = true; //由调用程序获取输出信息
                PluginProcess.StartInfo.RedirectStandardError = true; //重定向标准错误输出
                PluginProcess.Start();
                PluginProcess.WaitForExit();
                Output = PluginProcess.StandardOutput.ReadToEnd();
                Outputs = Output.Split("\n");
                Log.SaveLog("插件修改方法" + Method + "成功被调用,所属插件:" + Api);
                Log.SaveLog("插件返回给用户的信息:" + Outputs[0]);
                Log.SaveLog("插件返回给控制台的信息:" + Outputs[1]);
                Result = Output;
            }
            return Result;
        }
        /// <summary>
        /// 处理插件命令
        /// </summary>
        /// <param name="Method">方法</param>
        /// <param name="Args">参数</param>
        public static void ProcessCommand(string Method,string Args)
        {
            if (!System.IO.File.Exists("./Plugins/GET-Methods/" + Method + ".txt"))
            {
                Log.SaveLog("无效命令且不存在的插件方法:" + Method);
                return;
            }
            try
            {
                Process PluginProcess = new Process();
                string Processor = System.IO.File.ReadAllText("./Plugins/CommandProcessor/" + Method + ".txt");
                PluginProcess.StartInfo.FileName = "./Plugins/" + Processor;
                PluginProcess.StartInfo.CreateNoWindow = true;
                PluginProcess.StartInfo.Arguments = "--Method " + Method + " --HttpMethod None --Args \"" + Args + "\"";
                string Output;
                PluginProcess.StartInfo.RedirectStandardInput = true; //接受来自调用程序的输入信息
                PluginProcess.StartInfo.RedirectStandardOutput = true; //由调用程序获取输出信息
                PluginProcess.StartInfo.RedirectStandardError = true; //重定向标准错误输出
                PluginProcess.Start();
                PluginProcess.WaitForExit();
                Output = PluginProcess.StandardOutput.ReadToEnd();
                Log.SaveLog("插件方法" + Method + "成功被调用,参数:" + Args);
                Log.SaveLog("插件返回给控制台的信息:" + Output);
            }
            catch (Exception ex)
            {
                Log.SaveLog("插件方法" + Method + "调用异常,参数:" + Args);
                Log.SaveLog("触发异常:\n" + ex.ToString());
            }
        }
    }
}
