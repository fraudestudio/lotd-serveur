using Game.Message.Request;
using Game.Message.Response;

namespace Game.Message
{
	public interface ISerialiser
	{
		public String Serialise(IResponse response);

		public IRequest Deserialise(String request);
	}
}