namespace Game.Message.Response
{
	/// <summary>
	/// Une notification indiquant la seed de la map aux clients.
	/// </summary>
	public class Seed : IResponse
	{
		private int seed;

		public Seed(int seed)
		{
			this.seed = seed;
		}

		public ResponseType Type => ResponseType.SEED;

		public String[] ToData()
		{
			return new String[1] { this.seed.ToString() };
		}	
	}
}