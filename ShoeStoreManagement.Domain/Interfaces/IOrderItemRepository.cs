using ShoeStoreManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Domain.Interfaces
{
    public interface IOrderItemRepository
    {
        Task AddAsync(OrderItem orderItem);
        Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId);
    }
}
