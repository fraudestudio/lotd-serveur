using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MySqlConnector;

namespace DB_LOTD
{
    public class DBConnect
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public DBConnect()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "127.0.0.1";
            database = "bdd_lotd";
            uid = "u";
            password = "m";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            Console.WriteLine(connectionString);

            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine(ex.Message);
                        break;

                    case 1045:
                        Console.WriteLine(ex.Message);
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
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

        public string RandomKey(int i)
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

        //Insert statement
        public void Insert_client(string adresse_mail, string nom_compte, string mdp)
        {
            string query = "INSERT INTO Client (ADRESS_MAIL,NOM_COMPTE,MDP,TOKEN_CONNEXION,VALIDE,DATE_CREATION) VALUES(@adresse_mail,@nom_compte,@mdp,@TOKEN_CONNEXION,@VALIDITE,@DATE_CREATION);";
            Console.WriteLine("connexion...");
            bool r = this.OpenConnection();
            //open connection
            if (r == true)
            {
                Console.WriteLine("connecté");
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@adresse_mail", adresse_mail);
                cmd.Parameters.AddWithValue("@nom_compte", nom_compte);
                cmd.Parameters.AddWithValue("@mdp", mdp);
                cmd.Parameters.AddWithValue("@TOKEN_CONNEXION", RandomKey(10));
                cmd.Parameters.AddWithValue("@VALIDITE", true);
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
        }

        public Boolean verif_USER(string nom_compte, string mdp)
        {
            string query = "select * from client where NOM_COMPTE = @nomcompte and MDP = @mdp ;";
            Console.WriteLine("connexion...");
            bool r = this.OpenConnection();
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@nomcompte", nom_compte);
                cmd.Parameters.AddWithValue("@mdp", mdp);
                cmd.ExecuteNonQuery();
                Console.WriteLine("utilisateur existant ");
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //Update statement
        public void Update()
        {
        }

        //Delete statement
        public void Delete()
        {
        }

        //Select statement
        public List<string>[] Select()
        {
            return null;
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
