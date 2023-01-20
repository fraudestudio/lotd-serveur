namespace Game.Message.Response
{
	/// <summary>
	/// Une notification d'autentification.
	/// </summary>
	public class Authenticate : IResponse
	{
		public ResponseType Type => ResponseType.AUTH;

		public String[] ToData()
		{
			return new String[0];
		}
	}
}