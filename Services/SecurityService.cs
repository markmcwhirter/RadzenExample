using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using Radzen;

using LOBR.Models;

namespace LOBR
{
    public partial class SecurityService
    {
        private readonly HttpClient httpClient;

        private readonly NavigationManager navigationManager;

        public ApplicationUser User { get; private set; } = new ApplicationUser { Name = "Anonymous" };

        public ClaimsPrincipal Principal { get; private set; }

        public SecurityService(NavigationManager navigationManager, IHttpClientFactory factory)
        {
            this.httpClient = factory.CreateClient("LOBR");
            this.navigationManager = navigationManager;
        }

        public async Task<ApplicationAuthenticationState> GetAuthenticationStateAsync()
        {
            var uri =  new Uri($"{navigationManager.BaseUri}Account/CurrentUser");

            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, uri));

            return await response.ReadAsync<ApplicationAuthenticationState>();
        }

        public bool IsInRole(params string[] roles)
        {
            if (roles.Contains("Everybody"))
            {
                return true;
            }

            if (!IsAuthenticated())
            {
                return false;
            }

            if (roles.Contains("Authenticated"))
            {
                return true;
            }

            return roles.Any(role => Principal.IsInRole(role));
        }

        public bool IsAuthenticated()
        {
            return Principal?.Identity.IsAuthenticated == true;
        }

        public bool Initialize(AuthenticationState result)
        {
            Principal = result.User;

            var name = Principal.FindFirstValue(ClaimTypes.Name) ?? Principal.FindFirstValue("name");

            if (name != null)
            {
                User = new ApplicationUser { Name = name };
            }

            return IsAuthenticated();
        }
    }
}