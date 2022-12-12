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

                string query = "INSERT INTO VILLAGE (FACTION,BOIS,PIERRE,ORS,ID_JOUEUR,ID_UNIVERS,NOM_VILLAGE) VALUES (@faction,250,250,250,@idJ,@idU,@nomV);";
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

        /// <summary>
        /// renvoie les ressources disponible sous formes de tableau 
        /// </summary>
        /// <param name="idV"></param>
        /// <returns></returns>
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

        /// <summary>
        /// initialise tout les batiment d'un village
        /// </summary>
        /// <param name="idV"></param>
        /// <returns></returns>
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

        /// <summary>
        /// augemente le niveau d'un batiment de plus 1 et retire les ressource spécifié
        /// </summary>
        /// <param name="idB"></param>
        /// <param name="coutB"></param>
        /// <param name="coutP"></param>
        /// <param name="coutO"></param>
        /// <returns></returns>
        static public async Task<bool> UpBatiment(int idB, int coutB, int coutP, int coutO)
        {
            bool res = false;
            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                int[] temp = new int[4];
                await conn.OpenAsync();
                try
                {
                    string query = "SELECT BOIS,PIERRE,ORS,ID_VILLAGE FROM VILLAGE WHERE ID_VILLAGE = (SELECT ID_VILLAGE FROM BATIMENT WHERE ID_BATIMENT = @idB);";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idB", idB);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    if (dataReader.Read())
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            dataReader.GetInt32(i);
                        }
                        if (temp[0] <= coutB &&
                        temp[1] <= coutP &&
                        temp[2] <= coutO)
                        {
                            string query2 = "UPDATE VILLAGE SET " +
                                          "BOIS = @bois - @coutbois, PIERRE = @pierre - @coutpierre, ORS = @ors - @coutors " +
                                          "WHRE ID_VILLAGE = @idVillage";
                            cmd = new MySqlCommand(query2, conn);
                            cmd.Parameters.AddWithValue("@bois", temp[0]);
                            cmd.Parameters.AddWithValue("@coutbois", coutB);
                            cmd.Parameters.AddWithValue("@pierre", temp[1]);
                            cmd.Parameters.AddWithValue("@coutpierre", coutP);
                            cmd.Parameters.AddWithValue("@ors", temp[2]);
                            cmd.Parameters.AddWithValue("@coutors", coutO);
                            cmd.Parameters.AddWithValue("@idVillage", temp[3]);
                            await cmd.ExecuteNonQueryAsync();
                            string query3 = "UPDATE BATIMENT SET " +
                                            "LEVEL = LEVEL + 1 " +
                                            "WHERE ID_BATIMENT = @idB";
                            cmd = new MySqlCommand(query3, conn);
                            cmd.Parameters.AddWithValue("@idB", idB);
                            await cmd.ExecuteNonQueryAsync();
                        }

                    }
                }
                catch(MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return res;
        }

        /// <summary>
        /// renvoie le niveau de batiment d'un village
        /// </summary>
        /// <param name="idV"></param>
        /// <returns></returns>
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

        /// <summary>
        /// retourne plusieurs liste contenant l'id l'heure d'arriver et le slot des personnage contenu dans un batiment
        /// </summary>
        /// <param name="idB"></param>
        /// <returns></returns>
        static public async Task<List<(int, int, int)>> GetPersoInBatiment(int idB)
        {
            List<(int, int, int)> res = new List<(int, int, int)>();

            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();

                string query = "SELECT ID_PERSO,ENTREE,SLOT FROM BATIMENT WHERE ID_BATIMENT = @idB;";

                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idB", idB);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        TimeSpan sec = (DateTime.Now - dataReader.GetDateTime(1));
                        res.Add((dataReader.GetInt32(0), (int)sec.TotalSeconds, dataReader.GetInt32(2)));
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
        /// ajoute un perso dans un batiement au slot spécifié, si le personnage est déjà dedans ne fait rien 
        /// </summary>
        /// <param name="idP"></param>
        /// <param name="idB"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        static public async Task<bool> InsertPersoInBatiment(int idP, int idB, int slot)
        {
            bool result = false;
            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();

                try
                {
                    string queryVerif = "SELECT ID_BATIMENT FROM STOCK_PERSONNAGE WHERE ID_PERSONNAGE = @idP;";
                    MySqlCommand cmd = new MySqlCommand(queryVerif, conn);
                    cmd.Parameters.AddWithValue("@idP", idB);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    if (dataReader.Read() && dataReader.GetInt32(0) != idB)
                    {
                        string queryDelete = "DELETE FROM STOCK_PERSONNAGE WHERE ID_PERSONNAGE = @idP;";
                        cmd = new MySqlCommand(queryDelete, conn);
                        await cmd.ExecuteNonQueryAsync();
                        string query = "INSERT INTO STOCK_PERSONNAGE (ID_PERSONNAGE,ID_BATIMENT,ENTREE,SLOT) VALUES (@idP,@idB,@hEntree,@slot);";
                        cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@idP", idP);
                        cmd.Parameters.AddWithValue("@mdp",idB);
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