namespace Game.Message.Response
{
	/// <summary>
	/// Un message indiquant à un joueur qu'il doit attendre des ordres du serveur.
	/// </summary>
	public class Wait : IResponse
	{
		public ResponseType Type => ResponseType.WAIT;

		public String[] ToData()
		{
			return new String[0];
		}	
	}
}