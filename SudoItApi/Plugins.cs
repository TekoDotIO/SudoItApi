using System.Diagnostics;
using System.IO;

namespace SudoItApi
{
    public class Plugins
    {
        public static void IntializatePlugins()
        {
            Directory.CreateDirectory("./Plugins/");
            string[] PluginList = Directory.GetFiles("./Plugins/");
            foreach(string PluginName in PluginList)
            {
                Process pluginIntializater = new Process();
                pluginIntializater.StartInfo.FileName = PluginName;
                pluginIntializater.StartInfo.CreateNoWindow = true;
                pluginIntializater.StartInfo.Arguments = "--Intializate --Version " + AppInfo.AppName + " --User " + PersonalInfo.Name + " --AppName " + AppInfo.AppName;
                if (PersonalInfo.InsidePermission)
                {
                    pluginIntializater.StartInfo.Arguments += " --Dev";
                }
            }
        }
        public static void ScanPlugins()
        {
            Directory.CreateDirectory("./Plugins/");
            string[] Folders = Directory.GetDirectories("./Plugins/");
        }
    }
}
