using Game.Message;

namespace Game.Message.Request
{
	public interface IRequest
	{
		public RequestType Type { get; }

		public void FromData(String[] data);
	}
}