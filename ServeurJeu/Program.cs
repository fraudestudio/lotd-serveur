using Game.Networking;

namespace Game
{
	public class Program
	{
		public static void Main(string[] args)
		{
			int port = Int32.Parse(args[0]);
			String parentToken = args[1];

			Listener listener = new Listener(port, 2);
			listener.Start();
			listener.Connect(parentToken);
		}
	}
}