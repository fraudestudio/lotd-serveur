using System;
using System.Data;

using MySql.Data;
using MySql.Data.MySqlClient;

namespace Server.Database
{
    /// <summary>
    /// Class that represent the database connection
    /// </summary>
    static public class DatabaseConnection
    {
        
        static private String _USERNAME = Environment.GetEnvironmentVariable("DATABASE_USERNAME") ?? "";
        static private String _PASSWORD = Environment.GetEnvironmentVariable("DATABASE_PASSWORD") ?? "";

        /// <summary>
        /// Method that create the connection to the database
        /// </summary>
        /// <returns></returns>
        static public MySqlConnection NewConnection()
        {
            return new MySqlConnection($"Server=127.0.0.1;User ID={_USERNAME};Password={_PASSWORD};Port=3306;Database=bdd_lotd");
        }
    }
}