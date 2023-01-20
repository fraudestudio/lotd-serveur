namespace Server.Model
{
	public class SignUpRequest
	{
        /// <summary>
		/// The username of the account
		/// </summary>
		public String Username { get => username; set=> username = value; }
        private String username = "";

        /// <summary>
		/// The email of the account
		/// </summary>
        public String Email { get => email ; set => email = value; }
        private String email = "";

        /// <summary>
		///	The token of the captcha 
		/// </summary>
        public String CaptchaToken { get => captchaToken; set => captchaToken = value; }
		private String captchaToken = "";
	}
}