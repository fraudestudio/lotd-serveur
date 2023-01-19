using Game.Message;

namespace Game.Message.Request
{
	/// <summary>
	/// Une requ�te d'un client.
	/// </summary>
	public interface IRequest
	{
		/// <summary>
		/// Le type de requ�te.
		/// </summary>
		public RequestType Type { get; }

		/// <summary>
		/// Importe les donn�es de la requ�te.
		/// </summary>
		/// <param name="data"></param>
		public void FromData(String[] data);
	}
}