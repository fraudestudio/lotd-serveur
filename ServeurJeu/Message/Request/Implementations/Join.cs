namespace Game.Message.Request
{
	public class Join : IRequest
	{
		private int id;
		private String token;

		public int Id => this.id;

		public String Token => this.token;

		public Join(int id, String token)
		{
			this.id = id;
			this.token = token;
		}

		public RequestType Type => RequestType.JOIN;

		public void FromData(String[] data)
		{
			this.id = Int32.Parse(data[0]);
			this.token = data[1];
		}
	}
}