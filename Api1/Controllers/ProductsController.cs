using Api1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // api/products/getproducts
        [Authorize(Policy = "ReadProduct")]
        [HttpGet]
        public IActionResult GetProducts()
        {
            var productList = new List<Product>()
            {
                new Product() { Id = 1, Name = "Product 1", Price = 100,Stock =10 },
                new Product() { Id = 2, Name = "Product 2", Price = 200,Stock =20 },
                new Product() { Id = 3, Name = "Product 3", Price = 300,Stock =30 },
                new Product() { Id = 4, Name = "Product 4", Price = 400,Stock =40 },
                new Product() { Id = 5, Name = "Product 5", Price = 500,Stock =50 },
            };

            return Ok(productList);
        }

        [Authorize(Policy = "UpdateOrCreateProduct")]
        public IActionResult UpdateProduct(int id)
        {
            return Ok($"{id} ID ' li Product güncellenmiştir!");
        }

        [Authorize(Policy = "UpdateOrCreateProduct")]
        public IActionResult CreateProduct(Product product)
        {
            return Ok(product);
        }
    }
}
