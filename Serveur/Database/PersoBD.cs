using System;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Server.Database;

namespace Serveur.Database
{
    public class PersoDB
    {
        
        public static string GenerationNom()
        {
            Random rand = new Random();
            string res = "";
            string[] BaseNom = { "ae", "gn", "or", "ran","ir","am","rie","ir","rod","ael","is","el","n","r" };
            for (int i = 0; i < rand.Next(2,3)  ; i++)
            {
                res += BaseNom[rand.Next(BaseNom.Length)];
            }
            return res;
        }


    }
}
