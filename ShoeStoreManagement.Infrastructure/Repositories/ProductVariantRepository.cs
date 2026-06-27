using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShoeStoreManagement.Domain.Entities;
using ShoeStoreManagement.Domain.Interfaces;
using ShoeStoreManagement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Infrastructure.Repositories
{
    public class ProductVariantRepository : IProductVariantRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductVariantRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ProductVariant product)
        {
            await _context.ProductVariants.AddAsync(product);
        }
        public async Task<IEnumerable<ProductVariant>> GetAllAsync()
        {
            return await _context.ProductVariants.ToListAsync();
        }

        public async Task<ProductVariant?> GetByIdAsync(int id)
        {
            return await _context.ProductVariants.FindAsync(id);
        }

        public async Task<IEnumerable<ProductVariant>> GetByProductIdAsync(int id)
        {
            return await _context.ProductVariants
                .Where(p => p.ProductId == id)
                .ToListAsync();
        }

        public async Task<ProductVariant?> GetByIdWithProductAsync(int id)
        {
            return await _context.ProductVariants.Include(pv => pv.Product)
                .FirstOrDefaultAsync(pv => pv.Id == id);
        }

        public Task UpdateAsync(ProductVariant productVariant)
        {
            _context.ProductVariants.Update(productVariant);
            return Task.CompletedTask;

        }
        public Task DeleteAsync(ProductVariant productVariant)
        {
            _context.ProductVariants.Remove(productVariant);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistProductNameAndSizeAsync(string productName, int size, int storeId)
        {
            return await _context.ProductVariants.AnyAsync(v => v.Product.Name == productName &&
            v.Size == size &&
            v.Product.StoreId == storeId);
        }
    }
}
