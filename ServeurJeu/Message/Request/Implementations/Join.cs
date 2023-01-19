namespace Game.Message.Request
{
	/// <summary>
	/// Une requête indiquant l'arrivée d'un joueur.
	/// </summary>
	public class Join : IRequest
	{
		private int id;
		private String token;

		/// <summary>
		/// L'identifiant du joueur.
		/// </summary>
		public int Id => this.id;

		/// <summary>
		/// Le jeton de connexion.
		/// </summary>
		public String Token => this.token;

		public Join(int id, String token)
		{
			this.id = id;
			this.token = token;
		}

		public RequestType Type => RequestType.JOIN;

		public void FromData(String[] data)
		{
			this.id = Int32.Parse(data[0]);
			this.token = data[1];
		}
	}
}