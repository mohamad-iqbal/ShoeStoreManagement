using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoeStoreManagement.Application.Dtos;
using ShoeStoreManagement.Application.Interfaces;

namespace ShoeStoreManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnController : ControllerBase
    {
        private readonly IReturnService _returnService;

        public ReturnController(IReturnService returnService)
        {
            _returnService = returnService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateReturn(CreateReturnDto dto)
        {
            var returned = await _returnService.CreateReturnAsync(dto);
            return Ok(returned);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var returned = await _returnService.GetByIdAsync(id);

            if (returned == null)
            {
                return NotFound();
            }

            return Ok(returned);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var returns = await _returnService.GetAllAsync();
            return Ok(returns);
        }
    }
}
