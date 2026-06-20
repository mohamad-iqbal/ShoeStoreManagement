using ShoeStoreManagement.Application.Dtos;
using ShoeStoreManagement.Application.Interfaces;
using ShoeStoreManagement.Application.Exceptions;
using ShoeStoreManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Application.Services
{
    public class InventoryTransactionService : IInventoryTransactionService
    {
        private readonly IInventoryTransactionRepository _inventoryTransactionRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IOrderRepository _orderRepository;

        public InventoryTransactionService (IInventoryTransactionRepository inventoryTransactionRepository, ICurrentUserService currentUserService, IOrderRepository orderRepository)
        {
            _inventoryTransactionRepository = inventoryTransactionRepository;
            _currentUserService = currentUserService;
            _orderRepository = orderRepository;
        }

        public async Task<InventoryTransactionResponseDto?> GetByIdAsync(int id)
        {
            var inventaryTransaction = await _inventoryTransactionRepository.GetByIdAsync(id);

            if (inventaryTransaction == null)
            {
                throw new NotFoundException("Inventory transaction not found");
            }

            var storeId = _currentUserService.StoreId;

            var order = await _orderRepository.GetByIdAsync(inventaryTransaction.OrderId);

            if (order == null)
            {
                throw new NotFoundException("Order not found");
            }

            if (order.StoreId != storeId)
            {
                throw new ForbiddenException("You cannot access this inventory transaction");
            }

            return new InventoryTransactionResponseDto
            {
                Id = inventaryTransaction.Id,
                ProductVariantId = inventaryTransaction.ProductVariantId,
                Type = inventaryTransaction.Type,
                Quantity = inventaryTransaction.Quantity,
                Date = inventaryTransaction.Date,
                OrderId = inventaryTransaction.OrderId,
                UserId = inventaryTransaction.UserId
            };
        }

        public async Task<IEnumerable<InventoryTransactionResponseDto>> GetAllAsync()
        {
            var inventoryTransaction = await _inventoryTransactionRepository.GetAllAsync();

            var storeId = _currentUserService.StoreId;

            var result = new List<InventoryTransactionResponseDto>();

            foreach (var transaction in inventoryTransaction)
            {
                var order = await _orderRepository.GetByIdAsync(transaction.OrderId);

                if (order == null)
                {
                    continue;
                }

                if (order.StoreId != storeId)
                {
                    continue;
                }

                result.Add(new InventoryTransactionResponseDto
                {
                    Id = transaction.Id,
                    ProductVariantId = transaction.ProductVariantId,
                    Type = transaction.Type,
                    Quantity = transaction.Quantity,
                    Date = transaction.Date,
                    OrderId = transaction.OrderId,
                    UserId = transaction.UserId
                });
            }
            return result;
        }
    }
}

