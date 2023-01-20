namespace Game.Message.Response
{
	/// <summary>
	/// Un message indiquant le tour qui va se dérouler.
	/// </summary>
	public class Turn : IResponse
	{
		private bool playerTurn;
		private int turnNumber;

		public Turn(bool playerTurn, int turnNumber)
		{
			this.playerTurn = playerTurn;
			this.turnNumber = turnNumber;
		}

		public ResponseType Type => ResponseType.TURN;

		public String[] ToData()
		{
			return new String[2] {
				this.playerTurn ? "PLAYER" : "ENEMY",
				this.turnNumber.ToString(),
			};
		}	
	}
}