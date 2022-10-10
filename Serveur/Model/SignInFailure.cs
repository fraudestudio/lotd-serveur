namespace Server.Model
{
	class SignInFailure : SignIn
	{
		private String _reason;

		public SignInFailure(String reason) : base (true)
		{
			this._reason = reason;
		}

		public String Reason => this._reason;
	}
}