using System.IO;

namespace SudoItApi
{
    public class SetAndAuth
    {
        public static bool Auth(string Password)
        {
            if( File.ReadAllText(@"./Password.txt")==Password)
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
