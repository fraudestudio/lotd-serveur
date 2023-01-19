namespace Game.Message.Response
{
	/// <summary>
	/// Une r�ponse indiquant le succ�s d'une op�ration.
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