namespace Game.Message.Response
{
	public class Started : IResponse
	{
		public ResponseType Type => ResponseType.STARTED;

		public String[] ToData()
		{
			return new String[0];
		}	
	}
}