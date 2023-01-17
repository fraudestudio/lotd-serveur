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
		private Socket socket;
		private String stringBuffer = "";

		public int Id => this.id;

		public Stream(Socket socket)
		{
			// assigne puis incrémente
			this.id = nextId++;

			this.socket = socket;
		}

		public IRequest? Receive()
		{
			byte[] buffer = new byte[1024]; 
			int bytesRead;

			while (!this.stringBuffer.Contains('\n'))
			{
				bytesRead = this.socket.Receive(buffer);
				this.stringBuffer += Encoding.ASCII.GetString(buffer, 0, bytesRead);
			}

			String message = this.stringBuffer.Split('\n', 2)[0];
			return serialiser.Deserialise(message);
		}

		public void Send(IResponse message)
		{
			String payload = serialiser.Serialise(message) + '\n';

			this.socket.Send(Encoding.ASCII.GetBytes(payload));
		}

		public void Close()
		{
			this.socket.Close();
		}
	}
}