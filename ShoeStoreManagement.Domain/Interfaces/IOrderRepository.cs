using ShoeStoreManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task <IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task<Order?> GetWithItemsAsync(int orderId);
        Task UpdateAsync (Order order);
    }
}
