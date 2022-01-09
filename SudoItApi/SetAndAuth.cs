using System.IO;

namespace SudoItApi
{
    public class SetAndAuth
    {
        public static bool Auth(string Password)
        {
            string RealPassword = File.ReadAllText(@"./Password.txt");
            if (Password == RealPassword)
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
