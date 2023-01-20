namespace Game.Message.Response
{
	/// <summary>
	/// Une réponse indiquant le succès d'une opération.
	/// </summary>
	public class NotOk : IResponse
	{
		public ResponseType Type => ResponseType.NOK;

		public String[] ToData()
		{
			return new String[0];
		}	
	}
}