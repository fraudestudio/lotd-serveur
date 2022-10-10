namespace Server.Model
{
	class SignInSuccess : SignIn
	{
		private String _authToken;
		private String _username;
		private String _email;

		public SignInSuccess(String authToken, String username, String email) : base (true)
		{
			this._authToken = authToken;
			this._username = username;
			this._email = email;
		}

		public String AuthToken => this._authToken;
		public String Username => this._username;
		public String Email => this._email;
	}
}