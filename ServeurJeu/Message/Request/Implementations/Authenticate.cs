namespace Game.Message.Request
{
	/// <summary>
	/// Une requ�te d'authentification.
	/// </summary>
	public class Authenticate : IRequest
	{
		private String token;

		/// <summary>
		/// Le jeton de connexion.
		/// </summary>
		public String Token => this.token;

		public Authenticate(String token)
		{
			this.token = token;
		}

		public RequestType Type => RequestType.AUTH;

		public void FromData(String[] data)
		{
			this.token = data[0];
		}
	}
}