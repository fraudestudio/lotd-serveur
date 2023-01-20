using Game.Message.Response;
using Game.Message.Request;

namespace Game.Networking
{	
	/// <summary>
	/// Une collection de clients
	/// </summary>
	public class Clients
	{
		private struct Client {
			public Stream stream;
			public int playerId;
		}

		private List<Client> clients;

		/// <summary>
		/// <see langword="true"/> s'il manque des clients pour commencer la partie.
		/// </summary>
		public bool ClientsNeeded => this.clients.Count < Logic.Game.PLAYER_COUNT;

		/// <summary>
		/// Crée un nouveau <see cref="Clients"/>
		/// </summary>
		public Clients()
		{
			this.clients = new List<Client>();
		}

		/// <summary>
		/// Ajoute un client.
		/// </summary>
		/// <param name="stream">La connexion au client.</param>
		/// <param name="playerId">L'id du joueur.</param>
		public void Add(Stream stream, int playerId)
		{
			this.clients.Add(new Client { stream = stream, playerId = playerId });
		}

		/// <summary>
		/// Envoie une message à tous les clients.
		/// </summary>
		/// <param name="response">Le message à envoyer.</param>
		public void Notify(IResponse response)
		{
			foreach (var client in this.clients)
			{
				client.stream.Send(response);
			}
		}

		/// <summary>
		/// Envoie un message à un client.
		/// </summary>
		/// <param name="response">Le message à envoyer.</param>
		/// <param name="index">Le numéro du client à qui envoyer le message.</param>
		public void SendTo(int index, IResponse response)
		{
			this.clients[index].stream.Send(response);
		}

		/// <summary>
		/// Reçois un message d'un client.
		/// </summary>
		/// <param name="index">Le numéro du client depuis lequel recevoir le message.</param>
		/// <returns>Le message reçu, si il y en a un, sinon <see langword="null"/>.</returns>
		public IRequest? ReceiveFrom(int index)
		{
			return this.clients[index].stream.Receive();
		}

		/// <summary>
		/// Reçois un message d'un client.
		/// </summary>
		/// <param name="index">Le numéro du client depuis lequel recevoir le message.</param>
		/// <param name="timeout">La durée maximale à attendre avant d'arrêter d'attendre une réponse.</param>
		/// <returns>Le message reçu, si il y en a un, sinon <see langword="null"/>.</returns>
		public IRequest? ReceiveFrom(int index, TimeSpan timeout)
		{
			return this.clients[index].stream.Receive(timeout);
		}
	}
}