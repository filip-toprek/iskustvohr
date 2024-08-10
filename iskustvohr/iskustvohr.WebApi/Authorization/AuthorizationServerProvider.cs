using iskustvohr.Model;
using iskustvohr.Repository;
using iskustvohr.Service;
using iskustvohr.Service.Common;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace iskustvohr.WebApi.Authorization
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (UserRepository userRepository = new UserRepository())
            {
                User userToLogin = await userRepository.GetUserByEmailAsync(new User
                {
                    Email = context.UserName.ToLower()
                });

                if (userToLogin == null || !userToLogin.IsActive || !BC.EnhancedVerify(context.Password, userToLogin.Password))
                {
                    context.SetError("Invalid_Credentials", "Provided email and password are incorrect");
                    return;
                }

                if(userToLogin.EmailConfirmed == false)
                {
                    context.SetError("Email_Not_Confirmed", "Please confirm your email.");
                    return;
                }

               using(RoleRepository roleRepository = new RoleRepository())
                {
                    Role role = await roleRepository.GetRoleByIdAsync((Role)userToLogin.Role);
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userToLogin.Id.ToString()));
                    identity.AddClaim(new Claim(ClaimTypes.Role, role.RoleName));

                    context.Validated(identity);
                }

            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            context.AdditionalResponseParameters.Add("role", context.Identity.FindFirst(ClaimTypes.Role)?.Value);
            context.AdditionalResponseParameters.Add("userId", context.Identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return Task.FromResult<object>(null);
        }
    }
}