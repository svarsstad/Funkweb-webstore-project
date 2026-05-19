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

        // GET ALL

        [HttpGet]
        public async Task<List<Product>> Get()
        {
            return await _productService.GetAllProductsAsync();
        }

        // GET BY ID

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(string id)
        {
            var products = await _productService.GetAllProductsAsync();

            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // CREATE

        [HttpPost]
        public async Task<ActionResult> Create(Product product)
        {
            await _productService.CreateProductAsync(product);

            return Ok();
        }

        // UPDATE

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, Product updatedProduct)
        {
            updatedProduct.Id = id;

            await _productService.UpdateProductAsync(id, updatedProduct);

            return Ok();
        }

        // DELETE

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _productService.DeleteProductAsync(id);

            return Ok();
        }
    }
}
