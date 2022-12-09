using System;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Server.Database;

namespace Server.Database
{
    public class VillageDB
    {


        /// <summary>
        /// Create the village of the player in a universe 
        /// </summary>
        /// <param name="v">Village</param>
        /// <param name="idJ">ID of the player</param>
        /// <returns></returns>
        static public async Task<bool> InsertVillage(Model.Village v,  int idJ)
        {
            bool res = false;
            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();

                string query = "INSERT INTO VILLAGE (FACTION,BOIS,PIERRE,ORS,ID_JOUEUR,ID_UNIVERS?NOM_VILLAGE) VALUES (@faction,250,250,250,@idJ,@idU,@nomV);";
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@faction", v.Faction);
                    cmd.Parameters.AddWithValue("@idJ", idJ);
                    cmd.Parameters.AddWithValue("@idU", v.IdUnivers);
                    cmd.Parameters.AddWithValue("@nomV", v.Name);
                    await cmd.ExecuteNonQueryAsync();
                    res = true;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return res;
        }

        static public async Task<int[]> GetRessource(int idV)
        {
            int[] res = new int[3];

            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();

                string query = "SELECT BOIS,PIERRE,ORS FROM VILLAGE WHERE ID_VILLAGE = @idV;";
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idV", idV);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    for (int i = 0; i < 3; i++)
                    {
                        res[i] = dataReader.GetInt32(i);
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return res;
        }

        static public async Task<bool> initBatiment(int idV)
        {
            bool res = false;
            string[] bat = { "TAVERNE", "FORGERON", "ENTREPOT", "CAMP_DENTRAINEMENT", "INFIRMERIE" };

            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();

                string query = "INSERT INTO BATIMENT (TYPE_BATIMENT,NIVEAU,ID_VILLAGE) VALUES (@TB,1,@idV);";
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    for (int i = 0; i < 5; i++)
                    {
                        cmd.Parameters.AddWithValue("@idV", idV);
                        cmd.Parameters.AddWithValue("@TB", bat[i]);
                        await cmd.ExecuteNonQueryAsync();
                        res = true;
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return res;
        }

        static public async Task<List<(int, string)>> GetLevelBatiment(int idV)
        {
            List<(int, string)> res = new List<(int, string)>();

            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();

                string query = "SELECT NIVEAU_BATIMENT,TYPE_BATIMENT FROM BATIMENT WHERE ID_VILLAGE = @idV;";

                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idV", idV);
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

    }
}