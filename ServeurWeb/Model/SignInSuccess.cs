namespace Server.Model
{
    /// <summary>
    /// Class that represent the SignInSuccess
    /// </summary>
    class SignInSuccess
	{
        /// <summary>
        /// the boolean if the clients is validated
        /// </summary>
		public bool Validated { get => validated; set => validated = value; }
        private bool validated;

        /// <summary>
        /// the string of the token
        /// </summary>
        public String? SessionToken { get => sessionToken; set => sessionToken = value; }
        private String? sessionToken = null;
    }
}
