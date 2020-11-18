using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace WebBanMyPham.Author
{
    public class MyAuthorizationServerProvider: OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (CustomerRepository _repo = new CustomerRepository())
            {
                var customer = _repo.ValidateUser(context.UserName, context.Password);
                if (customer == null)
                {
                    context.SetError("invalid_grant", "Provided username and password is incorrect");
                    return;
                }
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Role, customer.Role));
                identity.AddClaim(new Claim(ClaimTypes.Name, customer.UserName));
                identity.AddClaim(new Claim("ID", customer.ID.ToString()));
                identity.AddClaim(new Claim("Email", customer.Email));
                context.Validated(identity);
            }
        }
    }
}