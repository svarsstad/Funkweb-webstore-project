using Microsoft.AspNetCore.Mvc;
using Project_Backend.Models;
using Project_Backend.Services;

namespace Project_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        // GET ALL

        [HttpGet]
        public async Task<List<Order>> Get()
        {
            return await _orderService.GetAllOrdersAsync();
        }

        // GET BY ID

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> Get(string id)
        {
            var orders = await _orderService.GetAllOrdersAsync();

            var order = orders.FirstOrDefault(p => p.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // CREATE

        [HttpPost]
        public async Task<ActionResult> Create(Order order)
        {
            await _orderService.CreateOrderAsync(order);

            return Ok();
        }

        // UPDATE

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, Order updatedOrder)
        {
            updatedOrder.Id = id;

            await _orderService.UpdateOrderAsync(id, updatedOrder);

            return Ok();
        }

        // DELETE

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _orderService.DeleteOrderAsync(id);

            return Ok();
        }
    }
}

