using Game.Message.Request;
using Game.Message.Response;

namespace Game.Message
{
	/// <summary>
	/// Permet de s�rialiser/d�serialiser les messages.
	/// </summary>
	public interface ISerialiser
	{
		/// <summary>
		/// S�rialise une r�ponse.
		/// </summary>
		/// <param name="response">La r�ponse � s�rialiser.</param>
		/// <returns>La r�ponse s�rialis�e.</returns>
		public String Serialise(IResponse response);

		/// <summary>
		/// D�serialise une requ�te.
		/// </summary>
		/// <param name="request">La requ�te � d�serialiser.</param>
		/// <returns>La requ�te d�serialis�e.</returns>
		public IRequest? Deserialise(String request);
	}
}