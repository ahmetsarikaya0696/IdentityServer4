using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Globalization;

namespace Client1.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            //await HttpContext.SignOutAsync("oidc");

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> GetRefreshToken()
        {
            HttpClient httpClient = new HttpClient();

            var discovery = await httpClient.GetDiscoveryDocumentAsync("https://localhost:7061");

            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);


            RefreshTokenRequest refreshTokenRequest = new RefreshTokenRequest()
            {
                ClientId = _configuration["Client1-Mvc:ClientId"],
                ClientSecret = _configuration["Client1-Mvc:ClientSecret"],
                RefreshToken = refreshToken,
                Address = discovery.TokenEndpoint,
            };

            var tokenResponse = await httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

            if (tokenResponse.IsError)
            {
                // Hata sayfasına yönlendir
            }

            List<AuthenticationToken> tokens =
            [
                new AuthenticationToken() {Name = OpenIdConnectParameterNames.IdToken, Value = tokenResponse.IdentityToken},
                new AuthenticationToken() {Name = OpenIdConnectParameterNames.AccessToken, Value = tokenResponse.AccessToken},
                new AuthenticationToken() {Name = OpenIdConnectParameterNames.RefreshToken, Value = tokenResponse.RefreshToken},
                new AuthenticationToken() {Name = OpenIdConnectParameterNames.ExpiresIn, Value = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn).ToString("o", CultureInfo.InvariantCulture)},
            ];

            var authenticationResult = await HttpContext.AuthenticateAsync();
            var authenticationProperties = authenticationResult.Properties;

            authenticationProperties.StoreTokens(tokens);

            await HttpContext.SignInAsync("Cookies", authenticationResult.Principal, authenticationProperties);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin")]
        public IActionResult AdminAction()
        {
            return View();
        }

        [Authorize(Roles = "admin,customer")]
        public IActionResult CustomerAction()
        {
            return View();
        }
    }
}
