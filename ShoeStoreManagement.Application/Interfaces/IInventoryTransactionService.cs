using ShoeStoreManagement.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Application.Interfaces
{
    public interface IInventoryTransactionService
    {
        Task<InventoryTransactionResponseDto?> GetByIdAsync(int id);
        Task<IEnumerable<InventoryTransactionResponseDto>> GetAllAsync();
    }
}
