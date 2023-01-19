using Game.Networking;
using Game.Message;

namespace Game
{
	public class Program
    {
		public static void Main(string[] args)
		{
			int port = Int32.Parse(args[0]);
			String parentToken = args[1];

			Listener listener = new Listener(port, Logic.Game.PLAYER_COUNT);
			listener.Start();
			listener.Accept(parentToken);

			// indique que la partie commence
			listener.Clients.Notify(new Message.Response.Started());

			Logic.Game game = new Logic.Game(listener.Clients);

			game.Init();
		}
	}
}