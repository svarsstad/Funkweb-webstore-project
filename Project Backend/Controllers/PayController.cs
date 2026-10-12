using Microsoft.AspNetCore.Mvc;
using Project_Backend.Models;
using Project_Backend.Services;
using System.Diagnostics;

namespace Project_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PayController : ControllerBase
    {
        private List<Product> Products = new();
        private readonly UserService _userService;
        private readonly CurrencyService _currencyService;
        private readonly OrderService _orderService;
        private readonly ProductService _productService;
        public PayController(UserService userService, CurrencyService currencyService, OrderService orderService, ProductService productService)
        {
            _userService = userService;
            _currencyService = currencyService;
            _orderService = orderService;
            _productService = productService;
            Products = _productService.GetAllProductsAsync().Result;
        }

        [HttpPost("pay")]
        public async Task<ActionResult<PayResponse>> Pay([FromBody] PayRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new PayResponse { Success = false, Message = "EMAIL AND PASSWORD REQUIRED" });
            }

            var result = await _userService.AuthenticateAsync(request.Email, request.Password);

            if (!result.Success)
            {
                return Unauthorized(result);
            }

            

        List<double> prices = new();
            foreach (var item in request.cart)
            {
                prices.Add((double)Products.First(p => p.Id == item.ProductId).Price);
                prices[prices.Count - 1] -= prices[prices.Count - 1] * item.DiscountPercentage * 0.01;
                foreach (Product product in Products)
                {
                    if (product.Id == item.ProductId && !product.Curruency.StartsWith("CZK-IID"))
                    {
                        prices[prices.Count - 1] *= _currencyService.GetExchangeRate(product.Curruency);
                    }
                }
                prices[prices.Count - 1] *= item.Quantity;
            }

            SaveOrderRequest orderRequest = new SaveOrderRequest(request.UserId, request.cart.ToArray<OrderItem>(), prices.ToArray<double>());
            await _orderService.SaveOrderAsync(orderRequest);

            return Ok(result);
        }
    }
}
