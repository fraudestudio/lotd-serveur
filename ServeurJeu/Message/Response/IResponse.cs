namespace Game.Message.Response
{
	public interface IResponse
	: ISerializable<IResponse, ResponseType>, Dispatchable
	{
	}
}