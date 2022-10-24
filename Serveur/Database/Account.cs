using MySqlConnector;
using Server.Utils;
using Server.Database;

namespace Server.Database
{
	static public class Account
	{
        static public async Task<bool> UserExists(string username)
        {
            bool result = true;
            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();
                
                string query = "select ID_JOUEUR from JOUEUR where NOM_COMPTE = @compte ;";
                
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@compte", username);
                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync();
                    result = await dataReader.ReadAsync();
                    Console.WriteLine(result);
                    if (result)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return result;
        }

		static public async Task<bool> UserExistsEmail(string emailAddress)
        {
            bool result = true;
            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();
                
                string query = "SELECT ID_JOUEUR FROM JOUEUR WHERE ADRESS_MAIL = @mail;";

                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@mail", emailAddress);

                    MySqlDataReader dataReader = await cmd.ExecuteReaderAsync();
                    result = await dataReader.ReadAsync();

                    if (result)
                    {
                        result = true;
                    } 
                    else
                    {
                        result = false;
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return result;
        }

        static public async Task<string> CreateTemp(string email, string username)
        { 
            string password = Utils.Utils.RandomPassword(10);
            
            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();
               
                string query = "INSERT INTO JOUEUR (ADRESS_MAIL,NOM_COMPTE,MDP,VALIDE,DATE_CREATION) VALUES (@adresse_mail,@nom_compte,@password,false,@date_creation);";
            
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@adresse_mail",email );
                    cmd.Parameters.AddWithValue("@nom_compte", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@date_creation", DateTime.Now);

                    await cmd.ExecuteNonQueryAsync();
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return password;
        }

        static public async Task<int?> VerifJoueurConnexion(string nom_compte, string mdp)
        {
            int? result = null;
            bool exist;
            
            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();

                string query = "select ID_JOUEUR from client where NOM_COMPTE = @nomcompte and MDP = @mdp ;";
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nomcompte", nom_compte);
                    cmd.Parameters.AddWithValue("@mdp", mdp);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    exist = dataReader.Read();
                    if (exist)
                    {
                        result = dataReader.GetInt32(1);
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return result;
        }

        static public async Task<bool> VerifCompteExist(string nom_compte)
        {
            bool exist = false;
            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();

                string query = "select ID_JOUEUR from client where NOM_COMPTE = @nomcompte;";
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nomcompte", nom_compte);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    exist = dataReader.Read();
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return exist;

        }

        static public async Task<bool> InsertJoueurSession(int id_compte)
        {
            string token = Utils.Utils.RandomPassword(10);
            bool execute = false;
            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();

                string query = "INSERT INTO SESSION (ID_JOUEUR,TOKEN) VALUES(@id_j,@token);";
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id_j", id_compte);
                    cmd.Parameters.AddWithValue("@token", token);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    execute = true;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return execute;
        }

        static public async Task<bool> DeleteJoueurSession(int id_compte)
        {
            bool execute = false;
            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();

                string query = "DELETE FROM SESSION WHERE ID_JOUEUR = @id_j ;";
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id_j", id_compte);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    execute = true;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return execute;
        }
    }
}