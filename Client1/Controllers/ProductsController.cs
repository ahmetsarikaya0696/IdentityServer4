using Client1.Models;
using Client1.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;

namespace Client1.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientService _httpClientService;

        public ProductsController(IConfiguration configuration, IHttpClientService httpClientService)
        {
            _configuration = configuration;
            _httpClientService = httpClientService;
        }

        public async Task<IActionResult> Index()
        {
            /*
            var discovery = await httpClient.GetDiscoveryDocumentAsync("https://localhost:7061");

            ClientCredentialsTokenRequest clientCredentialsTokenRequest = new ClientCredentialsTokenRequest();
            clientCredentialsTokenRequest.ClientId = _configuration["Client:ClientId"];
            clientCredentialsTokenRequest.ClientSecret = _configuration["Client:ClientSecret"];
            clientCredentialsTokenRequest.Address = discovery.TokenEndpoint;

            var token = await httpClient.RequestClientCredentialsTokenAsync(clientCredentialsTokenRequest);
            */

            var httpClient = await _httpClientService.GetHttpClientAsync();

            var response = await httpClient.GetAsync("https://localhost:7027/api/products/getproducts");

            List<Product> products = new();
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                products = JsonConvert.DeserializeObject<List<Product>>(content);
            }

            return View(products);
        }
    }
}
