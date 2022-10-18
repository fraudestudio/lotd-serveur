using System.Collections.Concurrent;

namespace Server.Utils
{
	public class DatabaseConnection
	{
		static private String _SCHEMA = Environment.GetEnvironmentVariable("DB_SCHEMA");
		static private String _USERNAME = Environment.GetEnvironmentVariable("DB_USERNAME");
		static private String _PASSWORD = Environment.GetEnvironmentVariable("DB_PASSWORD");
		static private BlockingCollection<DatabaseMessage> _messageQueue = new BlockingCollection<DatabaseMessage>();

		static public void HandleRequests()
		{

			while (true)
			{
				
			}
		}
	}
}