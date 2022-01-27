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
        public static bool Auth(string Password, string ip)
        {
            Directory.CreateDirectory("./banips/");
            if(!File.Exists("./banips/" + ip + ".txt")) File.Create("./banips/" + ip + ".txt").Close();
            if (File.ReadAllText("./banips/" + ip + ".txt")=="EEEEE")
            {
                Log.SaveLog(ip + "因为输入错误密码次数过多已被屏蔽.如需解除,请执行unban " + ip);
                return false;
            }
            if (File.ReadAllText(@"./Password.txt") == Password) //读取本地密码
            {
                if (File.Exists("./banips/" + ip + ".txt"))
                {
                    File.Delete("./banips/" + ip + ".txt");
                }
                return true;
            }
            else
            {
                if (!File.Exists("./banips/" + ip + ".txt"))
                {
                    File.AppendAllText("./banips/" + ip + ".txt", "E");
                }
                else
                {
                    File.AppendAllText("./banips/" + ip + ".txt", "E");
                }
                return false;
            }
        }
    }
}
