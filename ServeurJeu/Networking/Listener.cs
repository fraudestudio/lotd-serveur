using System.Net;
using System.Net.Sockets;
using Game.Message;
using Game.Message.Request;

namespace Game.Networking
{
	public class Listener
	{
		private TcpListener listener;

		private Stream? parentProcess;
		private List<Stream> clients;

		public bool AllConnected {
			get => parentProcess != null && clients.Count == 2;
		}

		public Listener(int port, int nbClients)
		{
      		IPAddress localAddr = IPAddress.Parse("127.0.0.1");
			this.listener = new TcpListener(localAddr, port);

			this.parentProcess = null;
			this.clients = new List<Stream>();
		}

		public void Start()
		{
			Console.WriteLine("[LISTENER] Starting up...");
			this.listener.Start();
			Console.WriteLine("[LISTENER] Listening on {0}", this.listener.LocalEndpoint);
		}

		public void Connect(String parentToken)
		{
			while (!this.AllConnected)
			{
				Console.WriteLine("[LISTENER] Waiting for a connection...");
				Socket client = listener.AcceptSocket();

				Console.WriteLine("[LISTENER] Connecting with {0}...", client.RemoteEndPoint);
				this.AcceptConnection(new Stream(client), parentToken);
			}
		}

		private void AcceptConnection(Stream client, String parentToken)
		{
			client.Send(new Message.Response.Authenticate());
			IRequest? req = client.Receive();
			
			if (req is Message.Request.Authenticate auth)
			{
				if (auth.Token == parentToken)
				{
					if (this.parentProcess == null)
					{
						this.parentProcess = client;
						client.Send(new Message.Response.Ok());
						Console.WriteLine("[LISTENER] Parent process connected");
					}
					else
					{
						Console.WriteLine("[LISTENER] Duplicate parent process, refusing connection");
						client.Close();
					}
				}
				else
				{
					this.clients.Add(client);
					client.Send(new Message.Response.Ok());
					Console.WriteLine("[LISTENER] Client #{0} connected", client.Id);
				}
			}
			else if (req != null)
			{
				Console.WriteLine("[LISTENER] Invalid response, refusing connection");
				client.Close();
			}
			else
			{
				Console.WriteLine("[LISTENER] Timed out, refusing connection");
				client.Close();
			}
		}
	}
}