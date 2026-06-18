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
    public class ReturnItemRepository : IReturnItemRepository
    {
        private readonly ApplicationDbContext _context;

        public ReturnItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ReturnItem item)
        {
            await _context.ReturnItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }
    }
}
