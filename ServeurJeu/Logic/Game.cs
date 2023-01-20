using Game.Networking;
using Game.Message;
using Game.Message.Request;

namespace Game.Logic
{
	/// <summary>
	/// Une exp?ition.
	/// </summary>
	public class Game
	{
		/// <summary>
		/// Nombre de joueurs.
		/// </summary>
		public const int PLAYER_COUNT = 2;

		private int seed;
		private Clients clients;

		/// <summary>
		/// Cr? une nouvelle partie.
		/// </summary>
		/// <param name="clients">La connexion avec les joueurs.</param>
		public Game(Clients clients)
		{
			this.clients = clients;
		}

		/// <summary>
		/// Initialise la partie.
		/// </summary>
		public void Init()
		{
			this.seed = new Random().Next();
			this.clients.Notify(new Message.Response.Seed(this.seed));

			// indique l'ordre des joueurs
			for (int i = 0; i < PLAYER_COUNT; i++)
			{
				this.clients.SendTo(i, new Message.Response.Order(i));
			}
		}

		/// <summary>
		/// Joue chaque tour en boucle jusqu'à la fin de la partie.
		/// </summary>
		public void Loop()
		{
			while (true)
			{
				// tours des joueurs
				for (int i = 0; i < PLAYER_COUNT; i++)
				{
					this.clients.Notify(new Message.Response.Turn(true, i));
					this.PlayerTurn(i);
				}

				// tour des ennemis
				this.clients.Notify(new Message.Response.Turn(false, 0));
				this.EnemyTurn();
 			}
		}

		private void PlayerTurn(int player)
		{
			// début du tour
			for (int i = 0; i < PLAYER_COUNT; i++)
			{
				if (i == player)
				{
					// le joueur dont c'est le tour
					this.clients.SendTo(i, new Message.Response.Play());
				}
				else
				{
					// les autres joueurs
					this.clients.SendTo(i, new Message.Response.Wait());
				}
			}

			bool actionOk = false;

			while (!actionOk)
			{
				IRequest? action = this.clients.ReceiveFrom(player, TimeSpan.FromSeconds(30));

				if (action == null)
				{
					// timed out, passe son tour
					actionOk = true;
				}
				else if (action is Message.Request.Move move)
				{
					// déplacement d'un personnage
					// TODO : vérification de la validité de l'action ...

					this.clients.Notify(new Message.Response.Move(true, move.Id, move.X, move.Y));
					actionOk = true;
				}
				else
				{
					this.clients.SendTo(player, new Message.Response.NotOk());					
				}
			}
		}

		private void EnemyTurn()
		{
			this.clients.Notify(new Message.Response.Wait());
			// TODO
		}
	}
}