using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Server.Utils
{
    /// <summary>
    /// Class that represent the utils
    /// </summary>
    public class Util
    {
        public static JsonSerializerOptions DefaultJsonOptions => new JsonSerializerOptions {
            AllowTrailingCommas = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true,
        };

        private const String ALPHANUM = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        /// <summary>
        /// Method that generate a random alphanum random
        /// </summary>
        /// <param name="size"> the size of the password</param>
        /// <returns></returns>
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

        /// <summary>
        /// Compute the salted password 
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        static public byte[] ComputeHash(string password, string salt)
        {
            Byte[] res = new Byte[32];
            using (SHA256 sha256Hash = SHA256.Create())
            {
                res = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
            }
            return res;
        }

        /// <summary>
        /// Compare the two salted password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        static public bool CompareHash(string password, string salt, Byte[] hash)
        {
            Byte[] temp = ComputeHash(password, salt);
            return Enumerable.SequenceEqual(temp, hash);
        }

        /// <summary>
        /// Method that generate the characters name randomly 
        /// </summary>
        /// <returns></returns>
        public static string GenerationNom()
        {
            Random rand = new Random();
            string res = "";
            string[] BaseNom = { "ae", "gn", "or", "ran", "ir", "am", "rie", "ir", "rod", "ael", "is", "el", "na", "ro", "chi" };
            for (int i = 0; i < rand.Next(2, 3); i++)
            {
                res += BaseNom[rand.Next(BaseNom.Length)];
            }
            res += " ";
            for (int i = 0; i < rand.Next(2, 3); i++)
            {
                res += BaseNom[rand.Next(BaseNom.Length)];
            }
            return res;
        }
    }
}