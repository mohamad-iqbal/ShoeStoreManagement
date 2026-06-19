using ShoeStoreManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Domain.Interfaces
{
    public interface IReturnRepository
    {
        Task AddAsync(Return returnEntity);
        Task<Return?> GetByIdAsync(int id);
        Task<IEnumerable<Return>> GetAllAsync();
    }
}
