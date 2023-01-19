using Game.Message.Request;
using Game.Message.Response;

namespace Game.Message
{
	/// <summary>
	/// Sérialise les requêtes/réponses sous la forme « COMMANDE ARG1 ARG2 ... »
	/// </summary>
	public class SpaceSeparatedSerialiser : ISerialiser
	{
		public String Serialise(IResponse response)
		{
			String[] type = new String[1] { response.Type.ToString() };
			String[] data = type.Concat(response.ToData()).ToArray();

			return String.Join(' ', data);
		}

		public IRequest? Deserialise(String request)
		{
			String[] parts = request.Split(' ', 2);

			try
			{
				RequestType type = Enum.Parse<RequestType>(parts[0]);
				String[] data;
				if (parts.Length == 2)
				{
					data = parts[1].Split(' ');
				}
				else
				{
					data = new String[0];
				}

				IRequest requestObject = RequestFactory.CreateRequest(type);
				requestObject.FromData(data);
				return requestObject;
			}
			catch (ArgumentException)
			{
				return null;
			}
		}
	}
}