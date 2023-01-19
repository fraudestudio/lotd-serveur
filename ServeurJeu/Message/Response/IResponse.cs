using Game.Message;
using Game.Networking;

namespace Game.Message.Response
{
	/// <summary>
	/// Une réponse à un ou plusieurs clients.
	/// </summary>
	public interface IResponse
	{
		/// <summary>
		/// Le type de réponse.
		/// </summary>
		public ResponseType Type { get; }

		/// <summary>
		/// Exporte les données de la requête.
		/// </summary>
		/// <returns>Les données de la requête.</returns>
		public String[] ToData();
	}
}