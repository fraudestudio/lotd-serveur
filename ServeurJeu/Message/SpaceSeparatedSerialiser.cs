using Game.Message.Request;
using Game.Message.Response;

namespace Game.Message
{
	public class SpaceSeparatedSerialiser : ISerialiser
	{
		public String Serialise(IResponse response)
		{
			String[] data = response.ToData();

			return String.Format("{0} {1}", response.Type, String.Join(' ', data));
		}

		public IRequest Deserialise(String request)
		{
			String[] parts = request.Split(' ', 2);
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
	}
}