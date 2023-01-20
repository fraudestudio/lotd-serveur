namespace Game.Message.Response
{
	/// <summary>
	/// Un message indiquant � un joueur que c'est � son tour de jouer.
	/// </summary>
	public class Play : IResponse
	{
		public ResponseType Type => ResponseType.PLAY;

		public String[] ToData()
		{
			return new String[0];
		}	
	}
}