using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ClientSideBoard
{
    public class ClientAuthentificationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        public string Token { get; set; } = string.Empty;
        public ClientAuthentificationStateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            HttpRequestMessage requestMessage = new(HttpMethod.Get, $"https://localhost:5001/User/ValidateToken/");
            requestMessage.Headers.Add("authToken", Token);
            var response = await _httpClient.SendAsync(requestMessage);
            var jwt = new JwtSecurityToken(Token);
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            AuthenticationState state = new(user);
            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;
        }

        

    }
}
