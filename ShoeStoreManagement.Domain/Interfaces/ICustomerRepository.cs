using ShoeStoreManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task AddAsync(Customer customer);
        Task <IEnumerable<Customer>> GetAllAsync();
        Task <Customer?> GetByIdAsync(int id);
        Task <Customer?> GetByNameAsync(string name);
        Task UpdateAsync(Customer customer) ;
        Task DeleteAsync(Customer customer);        
    }
}
