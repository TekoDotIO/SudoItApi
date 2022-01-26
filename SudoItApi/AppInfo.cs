namespace SudoItApi
{
    /// <summary>
    /// App信息类
    /// </summary>
    public class AppInfo
    {
        public static string AppName = "$udo!T Api Server";
        public static string Version = "v.1.0.0.4";
        public static string InsideVersion = "Beta 0006";
        public static string Copyright = "EachOther Tech. 2022";
        public static string UpdateLog = "-增加进程模块翻页\n-增加注释";
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
