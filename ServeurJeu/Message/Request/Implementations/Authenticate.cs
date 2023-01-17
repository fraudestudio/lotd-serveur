namespace Game.Message.Request
{
	public class Authenticate : IRequest
	{
		private String token;

		public String Token => this.token;

		public Authenticate(String token)
		{
			this.token = token;
		}

		public RequestType Type => RequestType.AUTH;

		public void FromData(String[] data)
		{
			this.token = data[0];
		}
	}
}