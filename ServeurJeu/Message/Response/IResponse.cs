using Game.Message;
using Game.Networking;

namespace Game.Message.Response
{
	public interface IResponse
	{
		public ResponseType Type { get; }

		public String[] ToData();
	}
}