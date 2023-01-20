using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Principal;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Runtime.CompilerServices;

namespace Server.Auth
{
    /// <summary>
    /// Model for the user identity.
    /// </summary>
    public class UserIdentity : ClaimsIdentity
    {
        private String _authType; // The type of authentication.
        private String _name; // The name of the user.

        /// <summary>
        /// Constructor for the UserIdentity class.
        /// </summary>
        /// <param name="authType"></param>
        /// <param name="userId"></param>
        /// <param name="validated"></param>
        public UserIdentity(String authType, int userId, bool validated)
        {
            this._authType = authType;
            this._name = userId.ToString();
            this.AddClaim(new Claim("userId", this._name));
            this.AddClaim(new Claim("isValidated", validated.ToString()));
        }

        /// <summary>
        /// Property for the authentication type.
        /// </summary>
        public override string? AuthenticationType => this._authType;

        /// <summary>
        /// Property for the name of the user.
        /// </summary>
        public override string? Name => _name;

        /// <summary>
        /// Property boolean that represant if the user is authenticated.
        /// </summary>
        public override bool IsAuthenticated => true;
    }

    /// <summary>
    /// Model for the authentication options.
    /// </summary>
    public class AuthOptions : AuthenticationSchemeOptions
    {

    }

    /// <summary>
    /// That class contains the extensions of the user identity
    /// </summary>
    public static class UserIdentityExtensions
    {
        /// <summary>
        /// Method that return the id of the clients 
        /// </summary>
        /// <param name="this"> the clients </param>
        /// <returns></returns>
        public static int? UserId(this ClaimsPrincipal @this)
        {
            try
            {
                Claim userIdClaim = @this.Claims.Single(claim => claim.Type == "userId");
                return Int32.Parse(userIdClaim.Value);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            catch (FormatException)
            {
                return null;
            }
            catch (OverflowException)
            {
                return null;
            }
        }

        /// <summary>
        /// Method that returns a boolean is the client is authenticated   
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool? IsValidated(this ClaimsPrincipal @this)
        {
            try
            {
                Claim isValidatedClaim = @this.Claims.Single(claim => claim.Type == "isValidated");
                return Boolean.Parse(isValidatedClaim.Value);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }
}