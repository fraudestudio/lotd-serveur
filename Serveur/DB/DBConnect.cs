using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using MySqlConnector;

namespace DB_LOTD
{
     public class DBConnect
    {

        static private MySqlConnection connection;
        private const string server = "127.0.0.1";
        private const string database = "bdd_lotd";
        private const string uid = "u";
        private const string password = "m";

        //Constructor
        public DBConnect()
        {
            if (connection == null)
            {
                Initialize();
            }
            
        }

        #region connexion
        //Initialize values
        private void Initialize()
        {
            string connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            Console.WriteLine(connectionString);

            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        private bool OpenConnection()
        {
            if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Connecting)
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                try
                {
                    connection.Close();
                    return true;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            else
            {
                return true;
            } 
        }
        #endregion

        #region utilitaire
        private string RandomKey(int i)
        {
            var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string result = "";
            var random = new Random();

            for (int j = 0; j < i; j++)
            {
                result += characters[random.Next(characters.Length)];
            }
            return result;
        }
        #endregion

        #region insert 

        /// <summary>
        /// créer un client temporaire avec les informations forunie 
        /// </summary>
        /// <param name="adresse_mail">adresse mail du client</param>
        /// <param name="nom_compte">nom du compte</param>
        /// <returns>renvoie le mot de passe temporaire</returns>
        public string InsertJoueurTemp(string adresse_mail, string nom_compte)
        {
            string mdp = RandomKey(10);
            string query = "INSERT INTO JOUEUR (ADRESS_MAIL,NOM_COMPTE,MDP,TOKEN_CONNEXION,VALIDE,DATE_CREATION) VALUES(@adresse_mail,@nom_compte,@mdp,@TOKEN_CONNEXION,@VALIDITE,@DATE_CREATION);";
            Console.WriteLine("connexion...");
            OpenConnection();
            //open connection
            if (OpenConnection())
            {
                Console.WriteLine("connecté");
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@adresse_mail",adresse_mail );
                cmd.Parameters.AddWithValue("@nom_compte", nom_compte);
                cmd.Parameters.AddWithValue("@mdp", mdp);
                cmd.Parameters.AddWithValue("@TOKEN_CONNEXION", RandomKey(10));
                cmd.Parameters.AddWithValue("@VALIDITE", false);
                cmd.Parameters.AddWithValue("@DATE_CREATION", DateTime.Now);
                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
                Console.WriteLine("donner ajouté");
            }
            else
            {
                    Console.WriteLine("erreur");
            }
            return mdp;
        }

        /// <summary>
        /// valide un utilisateur 
        /// </summary>
        /// <param name="compte">compte de l'utilisateur a validé </param>
        /// <param name="mdp">mot de passe a set sur le compte</param>
        public void ValidationUser(string compte, string mdp)
        {
            string query = "UPDATE JOUEUR SET mdp=@mdp, VALIDE = true WHERE NOM_COMPTE=@nomcompte";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@nomcompte", compte);
                cmd.Parameters.AddWithValue("@mdp", mdp);
                //Execute query
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// creer un univers 
        /// </summary>
        /// <param name="mdp">mot de passe de l'univers</param>
        /// <param name="nom_univers">nom de l'univers</param>
        public void InsertUnivers(string nom_uinvers,string mdp,string owner)
        {
            string query = "INSERT INTO univers (MDP_SERVEUR,NOM_UNIVERS,owner) VALUES(@mdp,@nom,@owner);";
            Console.WriteLine("connexion...");
            OpenConnection();
            //open connection
            if (OpenConnection())
            {
                Console.WriteLine("connecté");
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@mdp", mdp);
                cmd.Parameters.AddWithValue("@nom", nom_uinvers);
                cmd.Parameters.AddWithValue("@owner", owner);
                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
                this.InsertJoue(owner,nom_uinvers);
                Console.WriteLine("donner ajouté");
            }
            else
            {
                Console.WriteLine("erreur");
            }
        }

        public void InsertJoue(string user, string univers)
        {
            string query = "INSERT INTO JOUE (Id_joueur,id_univers) VALUES((SELECT ID_JOUEUR FROM JOUEUR where nom_compte = @user) , (SELECT ID_univers FROM univers where NOM_UNIVERS = @univers));";
            Console.WriteLine("connexion...");
            OpenConnection();
            //open connection
            if (OpenConnection())
            {
                Console.WriteLine("connecté");
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@user", user);
                cmd.Parameters.AddWithValue("@univers", univers);
                
                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
                Console.WriteLine("donner ajouté");
            }
            else
            {
                Console.WriteLine("erreur");
            }
        }


        #endregion

        #region verif
        /// <summary>
        /// verirife si l'utilisateur existe et si il a le bon mot de passe
        /// </summary>
        /// <param name="nom_compte"></param>
        /// <param name="mdp"></param>
        /// <returns></returns>
        public Boolean VerifJoueurConnexion(string nom_compte, string mdp)
        {
            string query = "select ID_JOUEUR from client where NOM_COMPTE = @nomcompte and MDP = @mdp ;";
            Console.WriteLine("connexion...");
            OpenConnection();
            bool result;
            try
            {
                Console.WriteLine("caca");
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@nomcompte", nom_compte);
                cmd.Parameters.AddWithValue("@mdp", mdp);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                result = dataReader.Read();
                Console.WriteLine(result);
                if (result)
                {
                    Console.WriteLine("l'utilisateur existe ");
                    result = true;
                } 
                else
                {
                    Console.WriteLine("l'utilisateur n'existe pas ");
                    result = false;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                result = false;
            }
            return result;
        }

        public Boolean VerifAdresseMailExiste(string adresseMail)
        {
            string query = "select ID_JOUEUR from JOUEUR where ADRESS_MAIL = @mail ;";
            Console.WriteLine("connexion...");
            OpenConnection();
            bool result;
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@mail", adresseMail);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                result = dataReader.Read();
                Console.WriteLine(result);
                if (result)
                {
                    Console.WriteLine("l'utilisateur existe ");
                    result = true;
                } 
                else
                {
                    Console.WriteLine("l'utilisateur n'existe pas ");
                    result = false;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                result = false;
            }
            return result;
        }

        public Boolean VerifLoginExiste(string login)
        {
            string query = "select ID_JOUEUR from JOUEUR where NOM_COMPTE = @compte ;";
            Console.WriteLine("connexion...");
            OpenConnection();
            bool result;
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@compte", login);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                result = dataReader.Read();
                Console.WriteLine(result);
                if (result)
                {
                    Console.WriteLine("l'utilisateur existe ");
                    result = true;
                }
                else
                {
                    Console.WriteLine("l'utilisateur n'existe pas ");
                    result = false;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                result = false;
            }
            return result;
        }
        #endregion

        #region get

        public string GetAdresseMail(string username)
        {
            string query = "select ADRESS_MAIL from JOUEUR where NOM_COMPTE = @compte ;";
            Console.WriteLine("connexion...");
            OpenConnection();
            string result = "";
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@compte", username);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                dataReader.Read();
                result += dataReader[0];
                Console.WriteLine(result);
                if (result == "")
                {
                    Console.WriteLine("l'utilisateur n'existe pas");
                    
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        #endregion



        //Update statement
        public void Update()
        {
            string query = "UPDATE tableinfo SET name='Joe', age='22' WHERE name='John Smith'";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Delete statement
        public void Delete()
        {
        }

        //Select statement
        public List<string>[] Select()
        {
            string query = "SELECT * FROM tableinfo";

            //Create a list to store the result
            List<string>[] list = new List<string>[3];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["id"] + "");
                    list[1].Add(dataReader["name"] + "");
                    list[2].Add(dataReader["age"] + "");
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        //Count statement
        public int Count()
        {
            return 0;
        }

        //Backup
        public void Backup()
        {
        }

        //Restore
        public void Restore()
        {
        }
    }
}
