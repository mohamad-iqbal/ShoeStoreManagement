using ShoeStoreManagement.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Application.Interfaces
{
    public interface IStoreService
    {
        Task<StoreResponseDto> CreateStoreAsync(CreateStoreDto dto);
        Task<IEnumerable<StoreResponseDto>> GetAllStoresAsync();
        Task<StoreResponseDto?> GetStoreByIdAsync(int id);
        Task<StoreResponseDto?> UpdateStoreAsync(int id, UpdateStoreDto dto);
        Task DeleteStoreAsync(int id);
    }
}
