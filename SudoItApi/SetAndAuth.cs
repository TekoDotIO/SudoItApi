using System.IO;

namespace SudoItApi
{
    public class SetAndAuth
    {
        /// <summary>
        /// 校验密码
        /// </summary>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        public static bool Auth(string Password)
        {
            if (File.ReadAllText(@"./Password.txt") == Password) //读取本地密码
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
