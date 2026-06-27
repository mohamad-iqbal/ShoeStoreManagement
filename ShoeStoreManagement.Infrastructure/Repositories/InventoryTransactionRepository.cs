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
        }

        public async Task<IEnumerable<InventoryTransaction>> GetByOrderIdAsync(int orderId)
        {
            return await _context.InventoryTransactions
                .Where(o => o.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<IEnumerable<InventoryTransaction>> GetAllAsync()
        {
            return await _context.InventoryTransactions.ToListAsync();
        }

        public async Task<InventoryTransaction?> GetByIdAsync(int id)
        {
            return await _context.InventoryTransactions.FindAsync(id);
        }
    }
}
