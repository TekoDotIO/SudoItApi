namespace SudoItApi
{
    /// <summary>
    /// App信息类
    /// </summary>
    public class AppInfo
    {
        public static string AppName = "$udo!T Api Server";
        public static string Version = "v.1.0.0.2";
        public static string InsideVersion = "Alpha 1";
        public static string Copyright = "EachOther Tech. 2022";
        public static string UpdateLog = "-更新了进程类部分功能\n-添加注释\n-现在,日志文件将被保存到Log文件夹下";
    }
    /// <summary>
    /// 用户信息类(定制)
    /// </summary>
    public class PersonalInfo
    {
        public static string Name;
        public static string Company;
        public static bool InsidePermission;
    }
}
