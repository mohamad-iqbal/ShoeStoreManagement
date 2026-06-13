using ShoeStoreManagement.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto dto);
        Task<IEnumerable<OrderResponseDto>> GetAllAsync();
        Task<OrderResponseDto?> GetOrderByIdAsync(int id);
        Task<OrderResponseDto> CancelOrderAsync(int id);
    }
}
