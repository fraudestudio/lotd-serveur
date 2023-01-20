using System.Net.Sockets;
using System.Text;
using Game.Message;
using Game.Message.Request;
using Game.Message.Response;

namespace Game.Networking
{
	/// <summary>
	/// Une connexion, ecapsulant un <see cref="TcpClient"/>.
	/// </summary>
	public class Stream
	{
		private static int nextId = 0;
		private static ISerialiser serialiser = new SpaceSeparatedSerialiser();

		private int id;
		private TcpClient socket;
		private StreamReader input;
		private StreamWriter output;

		public int Id => this.id;

		/// <summary>
		/// Crée un nouveau <see cref="Stream"/>. 
		/// </summary>
		/// <param name="socket">Le socket utilisé pour la connexion.</param>
		public Stream(TcpClient socket)
		{
			// assigne puis incrémente
			this.id = nextId++;

			this.socket = socket;
			this.socket.NoDelay = true;

			this.input = new StreamReader(socket.GetStream(), Encoding.ASCII);
			this.output = new StreamWriter(socket.GetStream(), Encoding.ASCII);
		}

		/// <summary>
		/// Reçois un message.
		/// </summary>
		/// <returns>La requête reçue.</returns>
		public IRequest? Receive()
		{
			if (this.input.ReadLine() is String msg)
			{
				Console.WriteLine("#{0} <<< {1}", this.id, msg);
				return serialiser.Deserialise(msg);
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Reçois un message.
		/// </summary>
		/// <param name="timeout">La durée maximale à attendre avant d'arrêter d'attendre une réponse.</param>
		/// <returns>La requête reçue.</returns>
		public IRequest? Receive(TimeSpan timeout)
		{
			Task<String?> task = this.input.ReadLineAsync();
			if (task.Wait(timeout))
			{
				if (task.Result is String msg)
				{
					Console.WriteLine("#{0} <<< {1}", this.id, msg);
					return serialiser.Deserialise(msg);
				}
			}
			
			return null;
		}

		/// <summary>
		/// Envoie un message.
		/// </summary>
		/// <param name="message">La réponse à envoyer.</param>
		public void Send(IResponse message)
		{
			String msg = serialiser.Serialise(message);
			Console.WriteLine("#{0} >>> {1}", this.id, msg);
			this.output.WriteLine(msg);
			this.output.Flush();
		}

		/// <summary>
		/// Ferme la connexion.
		/// </summary>
		public void Close()
		{
			this.input.Close();
			this.output.Close();
			this.socket.Close();
		}
	}
}