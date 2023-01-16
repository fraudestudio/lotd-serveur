namespace Game.Network
{
	public class Stream
	{
		private static int nextId = 0;

		private int id;

		public int Id => this.id;

		public Stream(socket: Socket)
		{
			// assigne puis incrémente
			this.id = nextId++;
		}

		public IRequest? Receive()
		{

		}

		public void Send(message: IResponse)
		{
			
		}
	}
}