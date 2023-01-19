using Game.Message.Request;
using Game.Message.Response;

namespace Game.Message
{
	/// <summary>
	/// Permet de sérialiser/déserialiser les messages.
	/// </summary>
	public interface ISerialiser
	{
		/// <summary>
		/// Sérialise une réponse.
		/// </summary>
		/// <param name="response">La réponse à sérialiser.</param>
		/// <returns>La réponse sérialisée.</returns>
		public String Serialise(IResponse response);

		/// <summary>
		/// Déserialise une requête.
		/// </summary>
		/// <param name="request">La requête à déserialiser.</param>
		/// <returns>La requête déserialisée.</returns>
		public IRequest? Deserialise(String request);
	}
}