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
    public class InventoryTransactionRepository : IInventoryTransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryTransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(InventoryTransaction transaction)
        {
            await _context.InventoryTransactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<InventoryTransaction>> GetByOrderIdAsync(int orderId)
        {
            return await _context.InventoryTransactions
                .Where(o => o.OrderId == orderId)
                .ToListAsync();
        }
    }
}
