namespace Game.Message.Request
{
	/// <summary>
	/// Une demande de déplacement
	/// </summary>
	public class Move : IRequest
	{
		private int id;
		private int x;
		private int y;

		/// <summary>
		/// L'identifiant du personnage.
		/// </summary>
		public int Id => this.id;

		/// <summary>
		/// La coordonée x.
		/// </summary>
		public int X => this.x;

		/// <summary>
		/// La coordonnée y.
		/// </summary>
		public int Y => this.y;

		public Move(int id, int x, int y)
		{
			this.id = id;
			this.x = x;
			this.y = y;
		}

		public RequestType Type => RequestType.MOVE;

		public void FromData(String[] data)
		{
			this.id = Int32.Parse(data[0]);
			this.x = Int32.Parse(data[1]);
			this.y = Int32.Parse(data[2]);
		}	
	}
}