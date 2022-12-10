using System;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Server.Database;
using Server.Model;

namespace Serveur.Database
{
    public class VillageDB
    {


        static public async Task<bool> InsertVillage(int idU, int idJ, string faction, string nomV)
        {
            bool res = false;
            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();

                string query = "INSERT INTO VILLAGE (FACTION,BOIS,PIERRE,ORS,ID_JOUEUR,ID_UNIVERS,NOM_VILLAGE) VALUES (@faction,250,250,250,@idJ,@idU,@nomV);";
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@faction", faction);
                    cmd.Parameters.AddWithValue("@idJ", idJ);
                    cmd.Parameters.AddWithValue("@idU", idU);
                    cmd.Parameters.AddWithValue("@nomV", nomV);
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
                    if(dataReader.Read())
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            res[i] = dataReader.GetInt32(i);
                        }
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
                    }
                    res = true;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return res;
        }

        static public async Task<List<(int,int, string)>> GetBatimentInfo(int idV)
        {
            List<(int,int, string)> res = new List<(int,int, string)>();

            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();

                string query = "SELECT ID_BATIMENT,NIVEAU_BATIMENT,TYPE_BATIMENT FROM BATIMENT WHERE ID_VILLAGE = @idV;";

                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idV", idV);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        res.Add((dataReader.GetInt32(0), dataReader.GetInt32(1), dataReader.GetString(2)));
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
        /// retourne plusieurs liste contenant l'id l'heure d'arriver et le slot des personnage contenu dans un batiment
        /// </summary>
        /// <param name="idB"></param>
        /// <returns></returns>
        static public async Task<List<(int,int,int)>> GetPersoInBatiment(Server.Model.Batiment batiment)
        {
            List<(int,int,int)> res = new List<(int,int,int)>();

            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();

                string query = "SELECT ID_PERSO,ENTREE,SLOT FROM BATIMENT WHERE ID_BATIMENT = @idB;";

                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idB", batiment.Id);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        TimeSpan sec = (DateTime.Now - dataReader.GetDateTime(1));
                        res.Add((dataReader.GetInt32(0),(int)sec.TotalSeconds,dataReader.GetInt32(2)));
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return res;
        }


        static public async Task<bool> InsertPersoInBatiment(Server.Model.Perso Perso, Server.Model.Batiment batiment,int slot)
        {
            bool result = false;
            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();

                try
                {
                    string queryVerif = "SELECT ID_PERSONNAGE, ID_BATIMENT FROM STOCK_PERSONNAGE WHERE ID_PERSONNAGE = @idP;";
                    MySqlCommand cmd = new MySqlCommand(queryVerif, conn);
                    cmd.Parameters.AddWithValue("@idP", Perso.id);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    if (dataReader.Read() && dataReader.GetInt32(1) != batiment.Id)
                    {
                        string queryDelete = "DELETE FROM STOCK_PERSONNAGE WHERE ID_PERSONNAGE = @idP;";
                        cmd = new MySqlCommand(queryDelete, conn);
                        await cmd.ExecuteNonQueryAsync();
                        string query = "INSERT INTO STOCK_PERSONNAGE (ID_PERSONNAGE,ID_BATIMENT,ENTREE,SLOT) VALUES (@idP,@idB,@hEntree,@slot);";
                        cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@idP", Perso.Id);
                        cmd.Parameters.AddWithValue("@mdp", batiment.Id);
                        cmd.Parameters.AddWithValue("@hEntree", DateTime.Now);
                        cmd.Parameters.AddWithValue("@slot", slot);
                        await cmd.ExecuteNonQueryAsync();
                        result = true;
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