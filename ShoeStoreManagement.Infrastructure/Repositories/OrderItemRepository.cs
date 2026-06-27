using Microsoft.EntityFrameworkCore;
using ShoeStoreManagement.Domain.Entities;
using ShoeStoreManagement.Domain.Interfaces;
using ShoeStoreManagement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Infrastructure.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(OrderItem orderItem)
        {
            await _context.OrderItems.AddAsync(orderItem);
        }

        public async Task<OrderItem?> GetByIdAsync(int id)
        {
            return await _context.OrderItems
                .FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}
