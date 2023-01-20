namespace Game.Message.Response
{
	/// <summary>
	/// Une r�ponse indiquant le succ�s d'une op�ration.
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