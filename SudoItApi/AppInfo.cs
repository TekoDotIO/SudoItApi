namespace SudoItApi
{
    /// <summary>
    /// App信息类
    /// </summary>
    public class AppInfo
    {
        public static string AppName = "$udo!T Api Server";
        public static string Version = "v.1.0.0.3";
        public static string InsideVersion = "Beta 0005";
        public static string Copyright = "EachOther Tech. 2022";
        public static string UpdateLog = "-修复status里面CPU占用始终为0的问题;\n-添加注释;\n-现在,非致命的异常将被标为Exception并返回异常信息;\n-系统状态新增总内存与已用内存.";
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
