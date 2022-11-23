using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Principal;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Runtime.CompilerServices;

namespace Server.Auth
{
    public class UserIdentity : IIdentity
    {
        private String _authType;
        private String _name;

        public UserIdentity(String authType, int userId)
        {
            this._authType = authType;
            this._name = userId.ToString();
        }

        public string? AuthenticationType => this._authType;

        public bool IsAuthenticated => true;

        public string? Name => _name;
    }

    public class AuthOptions : AuthenticationSchemeOptions
    {

    }

    public static class UserIdentityExtensions
    {
        public static int Id(this ClaimsPrincipal @this) => Int32.Parse(@this.Identity!.Name!);
    }
}