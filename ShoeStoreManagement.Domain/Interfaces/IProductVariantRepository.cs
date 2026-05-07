using ShoeStoreManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Domain.Interfaces
{
    public interface IProductVariantRepository
    {
        Task AddAsync(ProductVariant productVariant);
        Task <IEnumerable<ProductVariant>> GetAllAsync ();
        Task <ProductVariant?> GetByIdAsync(int id);
        Task <IEnumerable<ProductVariant>> GetByProductIdAsync(int id);
        Task UpdateAsync(ProductVariant productVariant);
        Task DeleteAsync(ProductVariant productVariant);
    }
}
