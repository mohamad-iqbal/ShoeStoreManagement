using ShoeStoreManagement.Application.Dtos;
using ShoeStoreManagement.Application.Interfaces;
using ShoeStoreManagement.Domain.Entities;
using ShoeStoreManagement.Domain.Enums;
using ShoeStoreManagement.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoeStoreManagement.Domain.Interfaces;

namespace ShoeStoreManagement.Application.Services
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _storeRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitofOfWork;

        public StoreService (IStoreRepository storeRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _storeRepository = storeRepository;
            _currentUserService = currentUserService;
            _unitofOfWork = unitOfWork;
        }

        public async Task<StoreResponseDto> CreateStoreAsync(CreateStoreDto dto)
        {
            var role = _currentUserService.Role;
            if (role != Role.Admin)
            {
                throw new ForbiddenException("Only admin can create store");
            }

            var store = new Store
            {
                
                Name = dto.Name
            };

            await _storeRepository.AddAsync(store);
            await _unitofOfWork.SaveChangesAsync();

            return new StoreResponseDto
            {
                Id = store.Id,
                Name = store.Name
            };
        }

        public async Task<IEnumerable<StoreResponseDto>> GetAllStoresAsync()
        {
            var role = _currentUserService.Role;
            if (role != Role.Admin)
            {
                throw new ForbiddenException("Only admin can view all stores");
            }

            var stores = await _storeRepository.GetAllAsync();

            return stores.Select(stores => new StoreResponseDto
            {
                Id = stores.Id,
                Name = stores.Name
            });
        }

        public async Task<StoreResponseDto?> GetStoreByIdAsync(int id)
        {
            var store = await _storeRepository.GetByIdAsync(id);
            if (store == null)
            {
                return null;
            }

            var role = _currentUserService.Role;
            var storeId = _currentUserService.StoreId;

            if (role != Role.Admin && storeId != id)
            {
                throw new ForbiddenException("You cannot access this store");
            }

            return new StoreResponseDto
            {
                Id = store.Id,
                Name = store.Name,
            };
        }

        public async Task<StoreResponseDto?> UpdateStoreAsync(int id, UpdateStoreDto dto)
        {
            var store = await _storeRepository.GetByIdAsync(id);
            if (store == null)
            {
                return null;
            }

            var role = _currentUserService.Role;
            if (role != Role.Admin)
            {
                throw new ForbiddenException("Only admin can update");
            }

            store.Name = dto.Name;

            await _storeRepository.UpdateAsync(store);
            await _unitofOfWork.SaveChangesAsync();

            return new StoreResponseDto
            {
                Id = store.Id,
                Name = store.Name
            };
        }

        public async Task DeleteStoreAsync(int id)
        {
            var store = await _storeRepository.GetByIdAsync(id);
            if (store == null)
            {
                throw new NotFoundException("Store not found");
            }

            var role = _currentUserService.Role;
            if (role != Role.Admin)
            {
                throw new ForbiddenException("Only admin can delete store");
            }

            await _storeRepository.DeleteAsync(store);
            await _unitofOfWork.SaveChangesAsync();
        }
    }
}
