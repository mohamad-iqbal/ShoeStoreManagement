using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoeStoreManagement.Application.Dtos;
using ShoeStoreManagement.Application.Interfaces;

namespace ShoeStoreManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductVariantController : ControllerBase
    {
        private readonly IProductVariantService _productVariantService;

        public ProductVariantController(IProductVariantService productVariantService)
        {
            _productVariantService = productVariantService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProductVariant(CreateProductVariantDto dto)
        {
            var productVariant = await _productVariantService.CreateProductVariantAsync(dto);
            return Ok(productVariant);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductVariant(int id, UpdateProductVariantDto dto)
        {
            var productVariant = await _productVariantService.UpdateProductVariantAsync(id, dto);
            if (productVariant == null)
            {
                return NotFound();
            }
            return Ok(productVariant);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductVariant(int id)
        {
            await _productVariantService.DeleteProductVariantAsync(id);
            return Ok("Product variant was delete permanently");
        }
    }
}
