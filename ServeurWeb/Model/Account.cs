namespace Server.Model
{
    /// <summary>
    /// Class that represent the Account
    /// </summary>
    public class Account
    {
        /// <summary>
        /// The id of the account
        /// </summary>
        public String? Username { get => username ; set => username = value; }
        private String? username;
        
        /// <summary>
        /// The Email of the account
        /// </summary>
        public String? Email { get => email; set => email =  value; }
        private String? email;
        
        /// <summary>
        /// The account is validated
        /// </summary>
        public bool? IsValidated { get => isValidated; set => isValidated = value ; }
        private bool? isValidated;

        /// <summary>
        /// Constructor of the Account
        /// </summary>
        /// <param name="username"> the username of the account</param>
        /// <param name="email"> the email of the account</param>
        /// <param name="isValidated"> the validation of the account</param>
        public Account(String username, String email, bool isValidated)
        {
            this.Username = username;
            this.Email = email;
            this.IsValidated = isValidated;
        }
    }
}
