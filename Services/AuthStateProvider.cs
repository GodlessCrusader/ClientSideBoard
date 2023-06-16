using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Identity.Client;
using System.Net.Http.Json;
using System.Security.Claims;

namespace ClientSideBoard.Services
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private ServerApiCaller _api;
        public AuthStateProvider(ServerApiCaller api)
        {
            _api = api;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var response = await _api.AuthorizeAsync();
            var identity = new ClaimsIdentity();
            
            var claims = await response.Content.ReadFromJsonAsync<List<Claim>>();
            identity.AddClaims(claims);
            
            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }
    }
}
