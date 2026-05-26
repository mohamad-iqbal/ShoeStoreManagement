using ShoeStoreManagement.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponseDto> CreateProductAsync(CreateProductDto product);
        Task<IEnumerable<ProductResponseDto>> GetAllAsync();
        Task<ProductResponseDto?> GetByIdAsync(int id);
        Task<ProductResponseDto?> UpdateProductAsync(int id, UpdateProductDto product);
        Task DeleteProductAsync(int id);
    }
}
