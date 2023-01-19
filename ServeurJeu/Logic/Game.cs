using Game.Networking;

namespace Game.Logic
{
	public class Game
	{
		public const int PLAYER_COUNT = 2;

		private int turn;
		private int seed;
		private Clients clients;

		public int Turn => this.turn;

		public Game(Clients clients)
		{
			this.turn = 0;
			this.clients = clients;
		}

		private void NextTurn()
		{
			this.turn++;
			this.turn %= PLAYER_COUNT;
		}

		public void Init()
		{
			this.seed = new Random().Next();
			this.clients.Notify(new Message.Response.Seed(this.seed));
		}
	}
}