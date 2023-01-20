namespace Game.Message.Response
{
	/// <summary>
	/// Un message indiquant le déplacement d'un personnage ou d'un ennemi
	/// </summary>
	public class Move : IResponse
	{
		public bool isCharacter;
		private int id;
		private int x;
		private int y;

		public Move(bool isCharacter, int id, int x, int y)
		{
			this.isCharacter = isCharacter;
			this.id = id;
			this.x = x;
			this.y = y;
		}

		public ResponseType Type => ResponseType.MOVE;

		public String[] ToData()
		{
			return new String[4] {
				this.isCharacter ? "CHARACTER" : "ENEMY",
				this.id.ToString(),
				this.x.ToString(),
				this.y.ToString(),
			};
		}	
	}
}