namespace SudoItApi
{
    /// <summary>
    /// App信息类
    /// </summary>
    public class AppInfo
    {
        /// <summary>
        /// 应用名称
        /// </summary>
        public static string AppName = "$udo!T-Api-Server";
        /// <summary>
        /// 版本号
        /// </summary>
        public static string Version = "v.1.0.1.2";
        /// <summary>
        /// 内部版本
        /// </summary>
        public static string InsideVersion = "Release I";
        /// <summary>
        /// 版权信息
        /// </summary>
        public static string Copyright = "EachOther Tech. 2022";
        /// <summary>
        /// 更新日志
        /// </summary>
        public static string UpdateLog = "-修复了插件列表不显示的问题\n-修复了命令插件却进入GET插件目录查找的问题\n-修正了Help命令的大小写争议\n-日志间距重新缩小为一行\n-修复了GET与POST插件返回值为两行的问题";
    }
    /// <summary>
    /// 用户信息类(定制)
    /// </summary>
    public class PersonalInfo
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public static string Name;
        /// <summary>
        /// 所属单位
        /// </summary>
        public static string Company;
        /// <summary>
        /// 是否具有内部开发者权限
        /// </summary>
        public static bool InsidePermission;
    }
}
