using Microsoft.AspNetCore.Mvc;
using Project_Backend.Models;
using Project_Backend.Services;

namespace Project_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<List<Product>> Get()
        {
            return await _productService.GetAllProductsAsync();
        }
    }
}
