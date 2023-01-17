namespace Game.Message.Response
{
	public class Authenticate : IResponse
	{
		public ResponseType Type => ResponseType.AUTH;

		public String[] ToData()
		{
			return new String[0];
		}
	}
}