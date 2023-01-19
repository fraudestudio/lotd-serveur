namespace Game.Message.Request
{
    /// <summary>
    /// Une fabrique de requêtes.
    /// </summary>
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
            	RequestType.JOIN => new Join(0, ""),
            	_ => throw new ArgumentException("Invalid request type"),
            };
        }
	}
}