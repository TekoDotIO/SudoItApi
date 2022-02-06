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
        public static string Version = "v.1.0.1.0";
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
        public static string UpdateLog = "-修复了密码错误引发的异常\n-添加插件支持\n-改进初始化模块\n-支持自定义密码错误容忍次数\n-添加应用图标\n-删除不必要的包\n-将日志整合到Log目录下\n-新增命令行线程\n\n\n注意:7z压缩模块因跨平台兼容性低已被删除,届时所有使用压缩文件模块的方法无法使用,请知悉.";
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
