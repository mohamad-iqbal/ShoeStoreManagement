using ShoeStoreManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Domain.Interfaces
{
    public interface IInventoryTransactionRepository
    {
        Task AddAsync(InventoryTransaction transaction);
        Task <IEnumerable<InventoryTransaction>> GetByOrderIdAsync(int orderId);
    }
}
