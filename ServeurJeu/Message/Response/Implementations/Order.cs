namespace Game.Message.Response
{
	/// <summary>
	/// Un message indiquant l'ordre d'un joueur.
	/// </summary>
	public class Order : IResponse
	{
		private int order;

		public Order(int order)
		{
			this.order = order;
		}

		public ResponseType Type => ResponseType.ORDER;

		public String[] ToData()
		{
			return new String[1] { this.order.ToString() };
		}	
	}
}