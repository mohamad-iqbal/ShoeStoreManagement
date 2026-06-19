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
    public class ReturnRepository : IReturnRepository
    {
        private readonly ApplicationDbContext _context;

        public ReturnRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Return returnEntity)
        {
            await _context.Returns.AddAsync(returnEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<Return?> GetByIdAsync(int id)
        {
            return await _context.Returns
                .Include(r => r.ReturnItems)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Return>> GetAllAsync()
        {
            return await _context.Returns
                .Include(r => r.ReturnItems)
                .ToListAsync();
        }
}
}
