using System;
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
            Directory.CreateDirectory("./Setting/banips/");
            if(!File.Exists("./Setting/banips/" + ip + ".txt")) File.Create("./Setting/banips/" + ip + ".txt").Close();
            if (File.ReadAllText("./Setting/banips/" + ip + ".txt") == File.ReadAllText("./Setting/ErrTimes.txt")) 
            {
                Log.SaveLog(ip + "因为输入错误密码次数过多已被屏蔽.如需解除,请执行unban " + ip);
                return false;
            }
            if (File.ReadAllText(@"./Setting/Password.txt") == Password) //读取本地密码
            {
                if (File.Exists("./Setting/banips/" + ip + ".txt"))
                {
                    File.Delete("./Setting/banips/" + ip + ".txt");
                }
                return true;
            }
            else
            {
                if (!File.Exists("./Setting/banips/" + ip + ".txt"))
                {
                    File.WriteAllText("./Setting/banips/" + ip + ".txt", "1");
                }
                else
                {
                    File.WriteAllText("./Setting/banips/" + ip + ".txt", (Convert.ToInt32(File.ReadAllText("./Setting/banips/" + ip + ".txt")) + 1).ToString());
                }
                return false;
            }
        }
    }
}
