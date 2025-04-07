using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

using LOBR.Models;

namespace LOBR
{
    public class ApplicationAuthenticationStateProvider : AuthenticationStateProvider, IHostEnvironmentAuthenticationStateProvider
    {
        private readonly SecurityService securityService;
        private ApplicationAuthenticationState authenticationState;
        private Task<AuthenticationState> authenticationStateTask;

        public ApplicationAuthenticationStateProvider(SecurityService securityService)
        {
            this.securityService = securityService;  
        }

        public void SetAuthenticationState(Task<AuthenticationState> authenticationStateTask)
        {
            this.authenticationStateTask = authenticationStateTask;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            AuthenticationState result = null;

            if (authenticationStateTask != null)
            {
                result = await authenticationStateTask;
                securityService.Initialize(result);
            }
            else
            {
                var identity = new ClaimsIdentity();
            
                var state = await GetApplicationAuthenticationStateAsync();

                if (state.IsAuthenticated)
                {
                    identity = new ClaimsIdentity(state.Claims.Select(c => new Claim(c.Type, c.Value)), "LOBR");
                }

                result = new AuthenticationState(new ClaimsPrincipal(identity));

                securityService.Initialize(result);
            }

            return result;
        }

        private async Task<ApplicationAuthenticationState> GetApplicationAuthenticationStateAsync()
        { 
            if (authenticationState == null)
            {
                authenticationState = await securityService.GetAuthenticationStateAsync();
            }

            return authenticationState;
        }
    }
}