namespace Server.Model
{
	class SignIn
	{
		private bool _success;

		public SignIn(bool success)
		{
			this._success = success;
		}

		public bool Success => this._success;
	}
}