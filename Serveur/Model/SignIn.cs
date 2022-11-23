namespace Server.Model
{
	class SignIn
	{
		public bool Success { get; set; } = false;

		public bool? Validated { get; set; } = null;

		public String? Reason { get; set; } = null;

		public String? SessionToken { get; set; } = null;
	}
}
