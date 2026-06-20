using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoeStoreManagement.Application.Interfaces;

namespace ShoeStoreManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryTransactionController : ControllerBase
    {
        private readonly IInventoryTransactionService _inventoryTransactionService;

        public InventoryTransactionController(IInventoryTransactionService inventoryTransactionService)
        {
            _inventoryTransactionService = inventoryTransactionService;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInventoryTransactionById(int id)
        {
            var inventoryTransaction = await _inventoryTransactionService.GetByIdAsync(id);
            if (inventoryTransaction == null)
            {
                return NotFound();
            }
            return Ok(inventoryTransaction);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllInventoryTransactions()
        {
            var inventoryTransactions = await _inventoryTransactionService.GetAllAsync();
            return Ok(inventoryTransactions);
        }
    }
}
