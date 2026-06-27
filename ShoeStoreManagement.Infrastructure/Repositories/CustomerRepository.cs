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
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
        }
        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }
        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }
        public async Task<Customer?> GetByNameAsync(string name)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Name == name);
        }
        public async Task<IEnumerable<Customer>> SearchAsync(string keyword)
        {
            return await _context.Customers
                .Where(c => c.Name.Contains(keyword))
                .ToListAsync();
        }
        public Task UpdateAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Customer customer)
        {
            _context.Customers.Remove(customer);
            return Task.CompletedTask;
        }
    }
}
