using Game.Message;
using Game.Networking;

namespace Game.Message.Response
{
	/// <summary>
	/// Une r�ponse � un ou plusieurs clients.
	/// </summary>
	public interface IResponse
	{
		/// <summary>
		/// Le type de r�ponse.
		/// </summary>
		public ResponseType Type { get; }

		/// <summary>
		/// Exporte les donn�es de la requ�te.
		/// </summary>
		/// <returns>Les donn�es de la requ�te.</returns>
		public String[] ToData();
	}
}