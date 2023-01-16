namespace Game.Network
{
	public class Listener
	{
		private TcpListener listener;

		public Listener(int port)
		{
      		IPAddress localAddr = IPAddress.Parse("127.0.0.1");
			this.listener = new TcpListener(localAddr, port);
		}

		public void StartLoop()
		{
			
		}
	}
}