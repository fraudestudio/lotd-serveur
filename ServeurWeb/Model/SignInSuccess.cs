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
		public bool? Validated { get; set; }
        private bool? validated = null;

        /// <summary>
        /// the string of the token
        /// </summary>
        public String? SessionToken { get => sessionToken; set => sessionToken = value; }
        private String? sessionToken = null;
    }
}
