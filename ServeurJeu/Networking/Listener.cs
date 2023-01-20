using System.Net;
using System.Net.Sockets;
using Game.Message;
using Game.Message.Request;

namespace Game.Networking
{
	/// <summary>
	/// Gère les connexions.
	/// </summary>
	public class Listener
	{
		private TcpListener listener;

		private Stream? parentProcess;
		private Clients clients;

		/// <summary>
		/// Les clients connectés.
		/// </summary>
		public Clients Clients => this.clients;

		/// <summary>
		/// Crée un nouveau <see cref="Listener"/>.
		/// </summary>
		/// <param name="port">Le port sur lequel écouter.</param>
		public Listener(int port)
		{
      		IPAddress localAddr = IPAddress.Parse("0.0.0.0");
			this.listener = new TcpListener(localAddr, port);

			this.parentProcess = null;
			this.clients = new Clients();
		}

		/// <summary>
		/// Commence à écouter pour des connexions entrantes.
		/// </summary>
		public void Start()
		{
			Console.WriteLine("[LISTENER] Starting up...");
			this.listener.Start();
			Console.WriteLine("[LISTENER] Listening on {0}", this.listener.LocalEndpoint);
		}

		/// <summary>
		/// Accepte la connexion du processus parent et des clients.
		/// 
		/// Cette méthode bloque jusqu'à ce que tous les clients soient connectés.
		/// </summary>
		/// <param name="parentToken"></param>
		public void Accept(String parentToken)
		{
			while (this.parentProcess == null)
			{
				Console.WriteLine("[LISTENER] Waiting for the parent process...");
				TcpClient client = listener.AcceptTcpClient();

				Console.WriteLine("[LISTENER] Connecting with {0}...", client.Client.RemoteEndPoint);
				Stream stream  = new Stream(client);
				if (this.AcceptConnection(stream, parentToken))
				{
					this.parentProcess = stream; 
				}
			}

			while (this.clients.ClientsNeeded)
			{
				IRequest? req = this.parentProcess!.Receive();
				if (req is Message.Request.Join joinRequest)
				{
					Console.WriteLine("[LISTENER] Waiting for a client...");
					TcpClient client = listener.AcceptTcpClient();

					Console.WriteLine("[LISTENER] Connecting with {0}...", client.Client.RemoteEndPoint);
					Stream stream  = new Stream(client);
					if (this.AcceptConnection(stream, joinRequest.Token))
					{
						this.clients.Add(stream, joinRequest.Id); 

						if (this.clients.ClientsNeeded)
						{
							this.parentProcess.Send(new Message.Response.Ok());
						}
						else
						{
							this.parentProcess.Send(new Message.Response.Started());
						}
					}
				}
				else
				{
					Console.WriteLine("[LISTENER] Expected JOIN request");
				}
			}
		}

		private bool AcceptConnection(Stream client, String token)
		{
			client.Send(new Message.Response.Authenticate());
			IRequest? req = client.Receive();
			
			if (req is Message.Request.Authenticate auth)
			{
				if (auth.Token == token)
				{
					client.Send(new Message.Response.Ok());
					Console.WriteLine("[LISTENER] Successfully connected");
					return true;
				}
				else
				{
					Console.WriteLine("[LISTENER] Invalid token, refusing connection");
					client.Close();
					return false;
				}
			}
			else if (req != null)
			{
				Console.WriteLine("[LISTENER] Invalid response, refusing connection");
				client.Close();
				return false;
			}
			else
			{
				Console.WriteLine("[LISTENER] Timed out, refusing connection");
				client.Close();
				return false;
			}
		}


	}
}