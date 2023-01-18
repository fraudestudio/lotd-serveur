using Game.Message.Response;
using Game.Message.Request;

namespace Game.Networking
{
	public class Clients
	{
		private struct Client {
			public Stream stream;
			public int playerId;
		}

		private List<Client> clients;
		private int requiredClients;

		public bool ClientsNeeded => this.clients.Count < requiredClients;

		public Clients(int requiredClients)
		{
			this.clients = new List<Client>();
			this.requiredClients = requiredClients;
		}

		public void Add(Stream stream, int playerId)
		{
			this.clients.Add(new Client { stream = stream, playerId = playerId });
		}

		public void Notify(IResponse response)
		{
			foreach (var client in this.clients)
			{
				client.stream.Send(response);
			}
		}

		public void SendTo(IResponse response, int index)
		{
			this.clients[index].stream.Send(response);
		}

		public IRequest? ReceiveFrom(int index)
		{
			return this.clients[index].stream.Receive();
		}
	}
}