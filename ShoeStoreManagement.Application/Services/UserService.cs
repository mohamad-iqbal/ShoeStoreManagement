using ShoeStoreManagement.Application.Dtos;
using ShoeStoreManagement.Application.Exceptions;
using ShoeStoreManagement.Application.Interfaces;
using ShoeStoreManagement.Domain.Entities;
using ShoeStoreManagement.Domain.Enums;
using ShoeStoreManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IStoreRepository _storeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService (IUserRepository userRepository, ICurrentUserService currentUser, IStoreRepository storeRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
            _storeRepository = storeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserResponseDto> CreateUserAsync(CreateUserDto dto)
        {
            var store = await _storeRepository.GetByIdAsync(dto.StoreId);
            if (store == null)
            {
                throw new NotFoundException("Store Not Found");
            }

            // Existing Email User Already
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);

            if (existingUser != null)
            {
                throw new BadRequestException("Email already exist");
            }
                
            
            // Create new user
            var user = new User()
            {
                FullName = dto.FullName,
                Role = dto.Role,
                StoreId = dto.StoreId,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Role = user.Role,
                StoreId = user.StoreId,
                Email = user.Email
            };
        }

        public async Task<UserResponseDto?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return null;

            var userId = _currentUser.Id;
            var role = _currentUser.Role;

            if (role != Role.Admin && userId != id)
            {
                throw new ForbiddenException("Access Denied");
            }

            return new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Role = user.Role,
                StoreId = user.StoreId,
                Email = user.Email
            };
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
        {
            
            var users = await _userRepository.GetAllAsync();

            return users.Select(user => new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Role = user.Role,
                StoreId = user.StoreId,
                Email = user.Email
            });
        }

        public async Task<UserResponseDto?> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return null;

            var userId = _currentUser.Id;
            var role = _currentUser.Role;
            if (role != Role.Admin && userId != id)
            {
                throw new ForbiddenException("Access Denied");
            }

            user.FullName = dto.FullName;
            user.StoreId = dto.StoreId;

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Role = user.Role,
                StoreId = user.StoreId,
                Email = user.Email
            };
        }

        public async Task ChangePasswordAsync(ChangePasswordDto dto)
        {
            var userId = _currentUser.Id;
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("User Not Found");
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash);
            if (!isPasswordValid)
            {
                throw new BadRequestException("Current password is incorrect");
            }

            var hashPassword = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            user.PasswordHash = hashPassword;

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException("User Not Found");
            }

            await _userRepository.DeleteAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
