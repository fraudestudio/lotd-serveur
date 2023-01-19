using Game.Message;

namespace Game.Message.Request
{
	/// <summary>
	/// Une requête d'un client.
	/// </summary>
	public interface IRequest
	{
		/// <summary>
		/// Le type de requête.
		/// </summary>
		public RequestType Type { get; }

		/// <summary>
		/// Importe les données de la requête.
		/// </summary>
		/// <param name="data"></param>
		public void FromData(String[] data);
	}
}