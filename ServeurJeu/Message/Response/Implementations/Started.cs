namespace Game.Message.Response
{
	/// <summary>
	/// Une notification indiquant le début de la partie.
	/// </summary>
	public class Started : IResponse
	{
		public ResponseType Type => ResponseType.STARTED;

		public String[] ToData()
		{
			return new String[0];
		}	
	}
}