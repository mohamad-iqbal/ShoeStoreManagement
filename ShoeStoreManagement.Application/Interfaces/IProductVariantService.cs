using ShoeStoreManagement.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Application.Interfaces
{
    public interface IProductVariantService
    {
        Task<ProductVariantResponseDto> CreateProductVariantAsync(CreateProductVariantDto dto);
        Task<ProductVariantResponseDto?> UpdateProductVariantAsync (int id, UpdateProductVariantDto dto);
        Task DeleteProductVariantAsync (int id);
    }
}
