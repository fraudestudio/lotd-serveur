namespace Game.Message.Response
{
	/// <summary>
	/// Une réponse indiquant le succès d'une opération.
	/// </summary>
	public class Ok : IResponse
	{
		public ResponseType Type => ResponseType.OK;

		public String[] ToData()
		{
			return new String[0];
		}	
	}
}