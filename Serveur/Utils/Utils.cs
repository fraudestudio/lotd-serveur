using System;

namespace Server.Utils
{
    static class Utils
    {
        private const String ALPHANUM = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        static public String RandomPassword(int size)
        {
            string result = "";
            Random random = new Random();

            for (int i = 0; i < size; i++)
            {
                result += ALPHANUM[random.Next(ALPHANUM.Length)];
            }

            return result;
        }
    }
}