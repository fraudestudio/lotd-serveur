namespace Game.Message.Response
{
	public class Ok : IResponse
	{
		public ResponseType Type => ResponseType.OK;

		public String[] ToData()
		{
			return new String[0];
		}	
	}
}