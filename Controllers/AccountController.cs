using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using LOBR.Models;

namespace LOBR.Controllers
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    [Route("Account/[action]")]
    public partial class AccountController : Controller
    {
        [HttpPost]
        public ApplicationAuthenticationState CurrentUser()
        {
            var identity = User.Identity as WindowsIdentity;

            var subject = new ClaimsIdentity(identity);

            subject.AddClaim(new Claim(ClaimTypes.Name, identity.Name));

            try
            {
                var groups = identity.Groups.Translate(typeof(NTAccount));

                foreach (var group in groups)
                {
                    subject.AddClaim(new Claim(ClaimTypes.Role, group.Value.Split("\\").Last()));
                    subject.AddClaim(new Claim(ClaimTypes.Role, group.Value));
                }
            }
            catch (IdentityNotMappedException ex)
            {
            }

            return new ApplicationAuthenticationState
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                Name = User.Identity.Name,
                Claims = subject.Claims.Select(c => new ApplicationClaim { Type = c.Type, Value = c.Value })
            };
        }
    }
}