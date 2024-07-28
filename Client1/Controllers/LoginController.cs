using Client1.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Globalization;
using System.Security.Claims;

namespace Client1.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginVM loginVM)
        {
            var httpClient = new HttpClient();

            string authServerUrl = _configuration["AuthServerUrl"];

            var discovery = await httpClient.GetDiscoveryDocumentAsync(authServerUrl);

            if (discovery.IsError)
            {
                // hata ve loglama
            }

            PasswordTokenRequest passwordTokenRequest = new PasswordTokenRequest()
            {
                Address = discovery.TokenEndpoint,
                UserName = loginVM.Email,
                Password = loginVM.Password,
                ClientId = _configuration["ClientResourceOwner:ClientId"],
                ClientSecret = _configuration["ClientResourceOwner:ClientSecret"],
            };

            TokenResponse tokenResponse = await httpClient.RequestPasswordTokenAsync(passwordTokenRequest);

            if (tokenResponse.IsError)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre yanlış!");
                return View();  
            }

            UserInfoRequest userInfoRequest = new UserInfoRequest()
            {
                Token = tokenResponse.AccessToken,
                Address = discovery.UserInfoEndpoint
            };

            UserInfoResponse userInfoResponse = await httpClient.GetUserInfoAsync(userInfoRequest);

            if (userInfoResponse.IsError)
            {
                // hata ve loglama
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfoResponse.Claims, "Cookies", "name", "role");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            AuthenticationProperties authenticationProperties = new AuthenticationProperties();

            List<AuthenticationToken> tokens =
            [
                new AuthenticationToken() {Name = OpenIdConnectParameterNames.AccessToken, Value = tokenResponse.AccessToken},
                new AuthenticationToken() {Name = OpenIdConnectParameterNames.RefreshToken, Value = tokenResponse.RefreshToken},
                new AuthenticationToken() {Name = OpenIdConnectParameterNames.ExpiresIn, Value = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn).ToString("o", CultureInfo.InvariantCulture)},
            ];
            authenticationProperties.StoreTokens(tokens);

            await HttpContext.SignInAsync("Cookies", claimsPrincipal, authenticationProperties);

            return RedirectToAction("Index", "User");
        }
    }
}
