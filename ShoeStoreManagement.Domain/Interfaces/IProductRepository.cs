using ShoeStoreManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetByStoreIdAsync(int id);
        Task<Product?> GetBySkuAsync(string sku, int storeId);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
    }
}
