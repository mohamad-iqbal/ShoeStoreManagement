using ShoeStoreManagement.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto> CreateUserAsync(CreateUserDto dto);
        Task<UserResponseDto?> GetByIdAsync(int id);
        Task<IEnumerable<UserResponseDto>> GetAllAsync();
        Task<UserResponseDto?> UpdateUserAsync(int id, UpdateUserDto dto);
        Task ChangePasswordAsync(ChangePasswordDto dto);
        Task DeleteUserAsync(int id);
    }
}
