using System.Net.Sockets;
using System.Text;
using Game.Message;
using Game.Message.Request;
using Game.Message.Response;

namespace Game.Networking
{
	public class Stream
	{
		private static int nextId = 0;
		private static ISerialiser serialiser = new SpaceSeparatedSerialiser();

		private int id;
		private TcpClient socket;
		private StreamReader input;
		private StreamWriter output;


		public int Id => this.id;

		public Stream(TcpClient socket)
		{
			// assigne puis incrémente
			this.id = nextId++;

			this.socket = socket;
			this.socket.NoDelay = true;

			this.input = new StreamReader(socket.GetStream(), Encoding.ASCII);
			this.output = new StreamWriter(socket.GetStream(), Encoding.ASCII);
		}

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

		public void Send(IResponse message)
		{
			String msg = serialiser.Serialise(message);
			Console.WriteLine("#{0} >>> {1}", this.id, msg);
			this.output.WriteLine(msg);
			this.output.Flush();
		}

		public void Close()
		{
			this.socket.Close();
		}
	}
}