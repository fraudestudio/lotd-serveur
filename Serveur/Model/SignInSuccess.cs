namespace Server.Model
{
	class SignInSuccess : SignIn
	{
		private String _authToken;

		public SignInSuccess(String authToken) : base(true)
		{
			this._authToken = authToken;
		}

		public String AuthToken => this._authToken;
	}
}
