using System;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Server.Database;

namespace Serveur.Database
{
    public static class Universe
    {
        /// <summary>
        /// creer un univers 
        /// </summary>
        /// <param name="mdp">mot de passe de l'univers</param>
        /// <param name="nom_univers">nom de l'univers</param>
        static public async Task<bool> InsertUniverse(Model.Universe universe, int owner)
        {
            bool result = false;
            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();

                string query = "INSERT INTO UNIVERS (NOM_UNIVERS,MDP_SERVEUR,OWNER) VALUES (@nom,@mdp,@owner);";

                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nom", universe.Name);
                    cmd.Parameters.AddWithValue("@mdp", universe.Password);
                    cmd.Parameters.AddWithValue("@owner", owner);
                    await cmd.ExecuteNonQueryAsync();
                    result = true;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return result;
        }


        /// <summary>
        /// retourne la liste des univers
        /// </summary>
        /// <returns>une liste a deux dimensions se composant de la façons suivant [univers,0(id_univers) 1(nom univers)]</returns>
       static public async Task<List<Model.Universe>> ReturnUniverse()
       {
            int i = 0;
            List<Model.Universe> res =  new List<Model.Universe>();

            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();
                try
                { 
                    string query = "select ID_UNIVERS from UNIVERS;";
                    MySqlCommand cmd2 = new MySqlCommand(query, conn);
                    MySqlDataReader dataReader = cmd2.ExecuteReader();
                    query = "select NOM_UNIVERS from UNIVERS;";
                    MySqlCommand cmd3 = new MySqlCommand(query, conn);
                    MySqlDataReader dataReader2 = cmd3.ExecuteReader();
                    while (dataReader.Read())
                    {
                        res.Add(dataReader.GetInt32(i), dataReader2.GetString(i));
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return res;
       }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        static public async Task<int[]?> UniversPlayer(int playerId)
        {
            int[] result = new int[5];

            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();
                try
                {
                    string query = "select ID_UNIVERS from JOUE WHERE ID_JOUEUR = @id;";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", playerId);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    for (int i = 0; i < 4 ; i++)
                    {
                        result[i] = dataReader.GetInt32(i);
                    }

                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return result;
        }
    }
}
