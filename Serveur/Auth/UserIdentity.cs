using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Principal;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Server.Auth
{
    public class UserIdentity : IIdentity
    {
        private static Dictionary<String, UserIdentity> _users = new Dictionary<String, UserIdentity>();

        private String _authType;
        private String _name;
        private int _id;

        public UserIdentity(String authType, int userId, String name)
        {
            this._authType = authType;
            this._name = name;
            this._id = userId;

            UserIdentity._users[this._name] = this;
        }

        public static UserIdentity Get(String name)
        {
            return UserIdentity._users[name];
        }

        public string? AuthenticationType => this._authType;

        public bool IsAuthenticated => true;

        public string? Name => _name;

        public int Id => _id;
    }

    public class AuthOptions : AuthenticationSchemeOptions
    {

    }
}