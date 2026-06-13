using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoeStoreManagement.Application.Dtos;
using ShoeStoreManagement.Application.Interfaces;

namespace ShoeStoreManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
        {
            var order = await _orderService.CreateOrderAsync(dto);
            return Ok(order);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [Authorize]
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var order = await _orderService.CancelOrderAsync(id);
            return Ok(order);
        }
    }
}
