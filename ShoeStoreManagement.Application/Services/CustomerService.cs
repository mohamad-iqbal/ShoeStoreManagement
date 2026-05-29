using ShoeStoreManagement.Application.Dtos;
using ShoeStoreManagement.Application.Interfaces;
using ShoeStoreManagement.Domain.Entities;
using ShoeStoreManagement.Domain.Enums;
using ShoeStoreManagement.Domain.Interfaces;
using ShoeStoreManagement.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService (ICustomerRepository customerRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerResponseDto> CreateCustomerAsync(CreateCustomerDto dto)
        {
            var role = _currentUserService.Role;

            if (role != Role.Admin && role != Role.Sales)
            {
                throw new FormatException("You cannot create customer");
            }
            
            var customer = new Customer
            {
                Name = dto.Name,
                Address = dto.Address,
                Phone = dto.Phone,
                Source = dto.Source
            };

            await _customerRepository.AddAsync(customer);
            await _unitOfWork.SaveChangesAsync();

            return new CustomerResponseDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Address = customer.Address,
                Phone = customer.Phone,
                Source = customer.Source
            };
        }

        public async Task<IEnumerable<CustomerResponseDto>> GetAllAsync()
        {
            var role = _currentUserService.Role;
            if (role != Role.Admin && role != Role.Sales)
            {
                throw new ForbiddenException("You cannot view all customers");
            }
            
            var customers = await _customerRepository.GetAllAsync();
            
            return customers.Select(customer => new CustomerResponseDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Address = customer.Address,
                Phone = customer.Phone,
                Source = customer.Source
            });
        }

        public async Task<CustomerResponseDto?> GetByIdAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return null;
            }

            var role = _currentUserService.Role;
            if (role != Role.Admin && role != Role.Sales)
            {
                throw new ForbiddenException("You cannot view customer");
            }

            return new CustomerResponseDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Address = customer.Address,
                Phone = customer.Phone,
                Source = customer.Source
            };
        }

        public async Task<CustomerResponseDto?> UpdateCustomerAsync(int id, UpdateCustomerDto dto)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return null;
            }

            var role = _currentUserService.Role;
            if (role != Role.Admin && role != Role.Sales)
            {
                throw new ForbiddenException("you cannot update this customer");
            }

            customer.Name = dto.Name;
            customer.Address = dto.Address;
            customer.Phone = dto.Phone;
            customer.Source = dto.Source;

            await _customerRepository.UpdateAsync(customer);
            await _unitOfWork.SaveChangesAsync();

            return new CustomerResponseDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Address = customer.Address,
                Phone = customer.Phone,
                Source = customer.Source
            };
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer =  await _customerRepository.GetByIdAsync(id);
            if (customer == null)                
            {
                throw new NotFoundException("Customer not found");
            }

            var role = _currentUserService.Role;
            if (role != Role.Admin && role != Role.Sales)
            {
                throw new ForbiddenException("You cannot delete this customer");
            }

            await _customerRepository.DeleteAsync(customer);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
