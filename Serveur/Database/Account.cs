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
	}
}