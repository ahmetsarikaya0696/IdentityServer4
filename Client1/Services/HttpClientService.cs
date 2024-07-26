
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net.Http;

namespace Client1.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private HttpClient _httpClient;

        public HttpClientService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = new HttpClient();
        }

        public async Task<HttpClient> GetHttpClientAsync()
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            _httpClient.SetBearerToken(accessToken);

            return _httpClient;
        }
    }
}
