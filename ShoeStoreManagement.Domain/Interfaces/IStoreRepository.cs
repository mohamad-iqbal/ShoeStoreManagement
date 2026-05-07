using ShoeStoreManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Domain.Interfaces
{
    public interface IStoreRepository
    {
        Task AddAsync(Store store);
        Task <IEnumerable<Store>> GetAllAsync();
        Task <Store?> GetByIdAsync(int id);
        Task UpdateAsync(Store store);
        Task DeleteAsync(Store store);
    }
}
