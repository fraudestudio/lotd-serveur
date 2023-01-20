using Game.Networking;

namespace Game.Logic
{
	/// <summary>
	/// Une expédition.
	/// </summary>
	public class Game
	{
		/// <summary>
		/// Nombre de joueurs.
		/// </summary>
		public const int PLAYER_COUNT = 2;

		private int turn;
		private int seed;
		private Clients clients;

		/// <summary>
		/// Crée une nouvelle partie.
		/// </summary>
		/// <param name="clients">La connexion avec les joueurs.</param>
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

		/// <summary>
		/// Initialise la partie.
		/// </summary>
		public void Init()
		{
			this.seed = new Random().Next();
			this.clients.Notify(new Message.Response.Seed(this.seed));
		}
	}
}