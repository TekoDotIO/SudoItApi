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
        public static string Version = "v.1.0.1.1";
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
        public static string UpdateLog = "-修复了进程类GetPids时词典格式不正确的问题\n-增加了插件类GetMethods,PostMethods等方法,可获取所有额外方法.";
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
