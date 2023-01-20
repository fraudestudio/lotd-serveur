using System;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Server.Utils;

namespace Server.Database
{
    /// <summary>
    /// Class that Contains for the village database méthods for the API
    /// /// </summary>
    public class Account
	{
        /// <summary>
        /// Method return if the user exists in the database.
        /// </summary>
        /// <param name="username" > The username of the user.</param>
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
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    result = await dataReader.ReadAsync();
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return result;
        }

        /// <summary>
        /// Method that return if the Email exists in the database.
        /// </summary>
        /// <param name="emailAddress"> The email address of the user.</param>
        /// <returns> return the id of the user if the email exists, null otherwise.</returns>
		static public async Task<int?> UserExistsEmail(string emailAddress)
        {
            int? result = null;

            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();
                
                string query = "SELECT ID_JOUEUR FROM JOUEUR WHERE ADRESS_MAIL = @mail;";

                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@mail", emailAddress);

                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    if (dataReader.Read())
                    {
                        result = dataReader.GetInt32(0);
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
        /// créer un utilisateur temporaire et renvoie le mot de passe temporaire
        /// </summary>
        /// <param name="email"> l'adresse mail de l'utilisateur</param>
        /// <param name="username"> le nom d'utilisateur de l'utilisateur</param>
        /// <returns> le mot de passe temporaire</returns>
        static public async Task<bool> CreateTemp(string email, string username, string password)
        {
            string sel = Util.RandomPassword(32);
            
            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();
               
                string query = "INSERT INTO JOUEUR (ADRESS_MAIL,NOM_COMPTE,MDP,VALIDE,DATE_CREATION,SEL) VALUES (@adresse_mail,@nom_compte,@password,false,@date_creation,@sel);";
            
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@adresse_mail",email );
                    cmd.Parameters.AddWithValue("@nom_compte", username);
                    cmd.Parameters.AddWithValue("@password", Util.ComputeHash(password, sel));
                    cmd.Parameters.AddWithValue("@date_creation", DateTime.Now);
                    cmd.Parameters.AddWithValue("@sel", sel);

                    await cmd.ExecuteNonQueryAsync();
                    return true;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return false;
        }

        /// <summary>
        /// check for the connexion return the id of the player 
        /// </summary>
        /// <param name="username"> the username of the player</param>
        /// <param name="password"> the password of the player</param>
        /// <returns> the id of the player if the connexion is correct, null otherwise</returns>
        static public async Task<(int?, bool)> CheckUsernamePassword(string username, string password)
        {
            int? result = null;
            bool validate = false;

            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();

                string query = "select SEL from JOUEUR where NOM_COMPTE = @nomcompte ;";
                string query2 = "select ID_JOUEUR , VALIDE from JOUEUR where NOM_COMPTE = @nomcompte and MDP = @mdp ;";
         
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nomcompte", username);
                    string sel;
                    using (MySqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        dataReader.Read();
                        sel = dataReader.GetString(0);
                    }
                    cmd = new MySqlCommand(query2, conn);
                    cmd.Parameters.AddWithValue("@nomcompte", username);
                    cmd.Parameters.AddWithValue("@mdp", Util.ComputeHash(password,sel));
                    using (MySqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.Read())
                        {
                            result = dataReader.GetInt32(0);
                            validate = dataReader.GetBoolean(1);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return (result,validate);
        }
        /// <summary>
        /// Method that create a session for the client
        /// </summary>
        /// <param name="userId"> The id of the user.</param>
        /// <param name="sessionToken"> The session token of the user.</param>
        /// <returns> return true if the session is created, false otherwise.</returns>
        static public async Task<bool> CreateSession(int userId, string sessionToken)
        {
            bool executed = false;
            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();

                string query = "INSERT INTO SESSION (ID_JOUEUR,TOKEN) VALUES(@id_j,@token);";
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id_j", userId);
                    cmd.Parameters.AddWithValue("@token", sessionToken);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    executed = true;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return executed;
        }

        /// <summary>
        /// Method that delete the session of the client
        /// </summary>
        /// <param name="id_compte"> The id of the user.</param>
        /// <returns> return true if the session is deleted, false otherwise.</returns>
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

        
        /// <summary>
        /// Method that check the session token 
        /// </summary>
        /// <param name="session"> the name of the Session </param>
        /// <returns></returns>
        static public async Task<int?> CheckTokenSession(string session)
        {
            int? result = null;
            bool exist;

            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();

                string query = "select ID_JOUEUR from SESSION where TOKEN = @token;";
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@token", session);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    exist = dataReader.Read();
                    if (exist)
                    {
                        result = dataReader.GetInt32(0);
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
        /// Method that Validate an User 
        /// </summary>
        /// <param name="id"> the id of the player</param>
        /// <returns> true if the player is validated </returns>
        static public async Task<bool> ValidateUser(int id)
        {
            bool result = false;

            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();

                string query = "UPDATE JOUEUR SET VALIDE = TRUE WHERE ID_JOUEUR = @id; ";
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
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
        /// Method that fetch the User Info in the Model account
        /// </summary>
        /// <param name="id"> the id of the player</param>
        /// <returns> return the model fetched or null otherwise</returns>
        static public async Task<Model.Account?> UserInfo(int id)
        {
            Model.Account? result = null;

            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();

                string query = "SELECT NOM_COMPTE, ADRESS_MAIL, VALIDE FROM JOUEUR WHERE ID_JOUEUR = @id; ";
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader reader = cmd.ExecuteReader()) 
                    {
                        if (reader.Read())
                        {
                            result = new Model.Account(
                                reader.GetString(0),
                                reader.GetString(1),
                                reader.GetBoolean(2)
                            );
                        }
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
        /// Method that Update the password of the user
        /// </summary>
        /// <param name="mdp"> the new password </param>
        /// <param name="id"> the id of the player </param>
        /// <returns> return true if the password has been updated, false otherwise</returns>
        static public async Task<bool> UpdateMDP(string mdp, int id)
        {
            bool result = false;
            string salt = Util.RandomPassword(32);

            using (MySqlConnection conn = DatabaseConnection.NewConnection())
            {
                await conn.OpenAsync();

                string query = "UPDATE JOUEUR SET MDP = @mdp, SEL = @salt WHERE ID_JOUEUR = @id;";
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@mdp", Util.ComputeHash(mdp, salt));
                    cmd.Parameters.AddWithValue("@salt", salt);
                    cmd.Parameters.AddWithValue("@id", id);
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
    }
}
