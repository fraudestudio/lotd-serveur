using System;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Server.Database;

namespace Serveur.Database
{
    public class Universe
    {
        /// <summary>
        /// creer un univers 
        /// </summary>
        /// <param name="mdp">mot de passe de l'univers</param>
        /// <param name="nom_univers">nom de l'univers</param>
        static public async Task<bool> InsertUnivers(string nom_univers, string? mdp, int owner)
        {
            bool result = false;
            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();

                string query = "INSERT INTO UNIVERS (NOM_UNIVERS,MDP_SERVEUR,OWNER) VALUES (@nom,@mdp,@owner);";

                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nom", nom_univers);
                    cmd.Parameters.AddWithValue("@mdp", mdp);
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
       static public async Task<List<(int, string)>> ReturnUniverse()
       {
            List<(int, string)> res = new List<(int, string)>();


            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();
                try
                { 
                    string query = "select ID_UNIVERS , NOM_UNIVERS from UNIVERS;";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    
                    while (dataReader.Read())
                    {
                        res.Add((dataReader.GetInt32(0), dataReader.GetString(1)));
                        
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
                        result[i] = dataReader.GetInt32(0);
                    }

                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return result;
        }

        /// <summary>
        /// renvoie l'id du village du joueur renvoie null si aucun 
        /// </summary>
        /// <param name="idJ"></param>
        /// <param name="idU"></param>
        /// <returns></returns>
        static public async Task<int?> PlayerHaveVillageInUnivers(int idJ, int idU)
        {
            int? res = null;
            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();
                try
                {
                    string query = "select ID_VILLAGE from VILLAGE WHERE ID_JOUEUR = @idJ and ID_UNIVERS = @idU;";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idJ", idJ);
                    cmd.Parameters.AddWithValue("@idU", idU);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    res = dataReader.GetInt32(0);  
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return res;
        }

        /// <summary>
        /// renvoie le nom du village du joueur 
        /// </summary>
        /// <param name="idJ"></param>
        /// <param name="idU"></param>
        /// <returns></returns>
        static public async Task<string?> PlayerVillageName(int idJ, int idV)
        {
            string? res = null;
            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();
                try
                {
                    string query = "select NOM_VILLAGE from VILLAGE WHERE ID_JOUEUR = @idJ and ID_VILLAGE = @idV;";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idJ", idJ);
                    cmd.Parameters.AddWithValue("@idV", idV);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    res = dataReader.GetString(0);
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return res;
        }

        static public async Task<string> MajoritaryFaction(int idU)
        {
            string? res = null;
            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();
                try
                {
                    string query = "select FACTION from VILLAGE WHERE ID_UNIVERS = @idU and COUNT(FACTION) = MAX(COUNT(FACTION));";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idU", idU);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    res = dataReader.GetString(0);
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return res;
        } 
    }
}
