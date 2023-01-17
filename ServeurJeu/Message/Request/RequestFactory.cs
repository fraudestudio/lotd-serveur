namespace Game.Message.Request
{
	public class RequestFactory
	{
        /// <summary>
        /// Crée une nouvelle <see cref="IRequest"/>
        /// </summary>
        /// <param name="type">Le type de requête à créer</param>
        /// <returns>Une nouvelle <see cref="IRequest"/></returns>
        public static IRequest CreateRequest(RequestType type)
        {
            return type switch
            {
            	RequestType.AUTH => new Authenticate(""),
            	_ => throw new ArgumentException("Invalid request type"),
            };
        }
	}
}