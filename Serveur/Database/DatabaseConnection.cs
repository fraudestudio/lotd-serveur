using MySqlConnector;
using System.Data;

namespace Server.Database
{
    static public class DatabaseConnection
    {
        static public MySqlConnection NewConnection()
        {
            return new MySqlConnection("Server=127.0.0.1;User ID=fraude;Password=password;Port=3306;Database=bdd_lotd");
        }
    }
}