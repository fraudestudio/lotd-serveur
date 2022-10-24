using Microsoft.AspNetCore.Hosting.Server;
using MySqlConnector;
using System.Collections.Concurrent;


namespace Server.Utils
{
	public class RequestCollection
	{
		static private String _SCHEMA = Environment.GetEnvironmentVariable("DB_SCHEMA");
		static private String _USERNAME = Environment.GetEnvironmentVariable("DB_USERNAME");
		static private String _PASSWORD = Environment.GetEnvironmentVariable("DB_PASSWORD");
		static private BlockingCollection<DatabaseMessage> _messageQueue = new BlockingCollection<DatabaseMessage>();

		static public async void HandleRequests()
		{
			MySqlConnection mySqlConnection = Server.Database.DatabaseConnection.NewConnection();
            
			await mySqlConnection.OpenAsync();

            while (true) 
            { 
                DatabaseMessage message = _messageQueue.Take();
                MySqlCommand command = new MySqlCommand(message.Query, mySqlConnection);

            }

            
		}

        public static void AddMessage(DatabaseMessage message)
        {
            _messageQueue.Add(message);
        }
    }
}