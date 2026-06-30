using ShoeStoreManagement.Application.Dtos;
using ShoeStoreManagement.Application.Interfaces;
using ShoeStoreManagement.Application.Exceptions;
using ShoeStoreManagement.Domain.Enums;
using ShoeStoreManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoeStoreManagement.Domain.Entities;

namespace ShoeStoreManagement.Application.Services
{
    public class ReturnService : IReturnService
    {
        private readonly IReturnRepository _returnRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReturnItemRepository _returnItemRepository;
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IInventoryTransactionRepository _inventoryTransactionRepository;

        public ReturnService(IReturnRepository returnRepository, ICurrentUserService currentUserService, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IUnitOfWork unitOfWork, IReturnItemRepository returnItemRepository, IProductVariantRepository productVariantRepository, IInventoryTransactionRepository inventoryTransactionRepository)
        {
            _returnRepository = returnRepository;
            _currentUserService = currentUserService;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _unitOfWork = unitOfWork;
            _returnItemRepository = returnItemRepository;
            _productVariantRepository = productVariantRepository;
            _inventoryTransactionRepository = inventoryTransactionRepository;

        }

        public async Task<ReturnResponseDto> CreateReturnAsync(CreateReturnDto dto)
        {
            var role = _currentUserService.Role;
            if (role != Role.Sales)
            {
                throw new ForbiddenException("You cannot create this return");
            }

            var storeId = _currentUserService.StoreId;

            var order = await _orderRepository.GetByIdAsync(dto.OrderId);
            if (order == null)
            {
                throw new NotFoundException("Order cannot found");
            }

            if (order.StoreId != storeId)
            {
                throw new ForbiddenException("You cannot access this order");
            }

            if (order.Status == Status.Canceled)
            {
                throw new BadRequestException("Cancelled order cannot be returned");
            }

            if (order.Status == Status.Returned || order.Status == Status.PartiallyReturned)
            {
                throw new BadRequestException("Order already returned");
            }

            decimal totalRefund = 0;

            foreach (var item in dto.ReturnItems)
            {
                var orderItem = await _orderItemRepository.GetByIdAsync(item.OrderItemId);

                if (orderItem == null)
                {
                    throw new BadRequestException($"Order item {item.OrderItemId} not found");
                }

                if (orderItem.OrderId != order.Id)
                {
                    throw new BadRequestException("Order item does not belong to this order");
                }


                if (item.Quantity <= 0)
                {
                    throw new BadRequestException("Quantity must be greater than zero");
                }

                if (item.Quantity > orderItem.Quantity)
                {
                    throw new BadRequestException("Returned quantity exceeds purchased quantity");
                }

                totalRefund += orderItem.Price * item.Quantity;
            }

            var returnEntity = new Return
            {
                OrderId = dto.OrderId,
                ReturnDate = DateTime.UtcNow,
                TotalRefund = totalRefund
            };

            await _returnRepository.AddAsync(returnEntity);
            await _unitOfWork.SaveChangesAsync();

            var createdReturnItems = new List<ReturnItem>();

            foreach (var item in dto.ReturnItems)
            {
                var orderItem = await _orderItemRepository
                    .GetByIdAsync(item.OrderItemId);

                if (orderItem == null)
                {
                    throw new BadRequestException(
                        $"Order item {item.OrderItemId} not found");
                }

                var refundAmount = orderItem.Price * item.Quantity;

                var returnItem = new ReturnItem
                {
                    ReturnId = returnEntity.Id,
                    OrderItemId = item.OrderItemId,
                    Quantity = item.Quantity,
                    RefundAmount = refundAmount
                };

                await _returnItemRepository.AddAsync(returnItem);

                createdReturnItems.Add(returnItem);

                var productVariant = await _productVariantRepository
                    .GetByIdAsync(orderItem.ProductVariantId);

                if (productVariant == null)
                {
                    throw new BadRequestException(
                        $"Product variant {orderItem.ProductVariantId} not found");
                }

                productVariant.StockQty += item.Quantity;

                await _productVariantRepository.UpdateAsync(productVariant);

                var inventoryTransaction = new InventoryTransaction
                {
                    ProductVariantId = productVariant.Id,
                    Type = TypeTransaction.Return,
                    Quantity = item.Quantity,
                    Date = DateTime.UtcNow,
                    OrderId = order.Id,
                    UserId = _currentUserService.Id
                };

                await _inventoryTransactionRepository
                    .AddAsync(inventoryTransaction);
            }

            var totalReturnedQty = dto.ReturnItems.Sum(x => x.Quantity);
            var totalOrderedQty = order.OrderItems.Sum(x => x.Quantity);

            if (totalReturnedQty >= totalOrderedQty)
            {
                order.Status = Status.Returned;
            }
            else
            {
                order.Status = Status.PartiallyReturned;
            }

            await _orderRepository.UpdateAsync(order);

            await _unitOfWork.SaveChangesAsync();

            return new ReturnResponseDto
            {
                Id = returnEntity.Id,
                OrderId = returnEntity.OrderId,
                ReturnDate = returnEntity.ReturnDate,
                TotalRefund = returnEntity.TotalRefund,

                ReturnItems = createdReturnItems.Select(item =>
                    new ReturnItemResponseDto
                    {
                        Id = item.Id,
                        ReturnId = item.ReturnId,
                        OrderItemId = item.OrderItemId,
                        Quantity = item.Quantity,
                        RefundAmount = item.RefundAmount
                    }).ToList()
            };
        }

        public async Task<ReturnResponseDto?> GetByIdAsync(int id)
        {
            var returned = await _returnRepository.GetByIdAsync(id);
            
            if (returned == null)
            {
                return null;
            }

            var storeId = _currentUserService.StoreId;

            var order = await _orderRepository.GetByIdAsync(returned.OrderId);

            if (order == null || order.StoreId != storeId)
            {
                throw new ForbiddenException("You cannot access this return");
            }

            return new ReturnResponseDto
            {
                Id = returned.Id,
                OrderId = returned.OrderId,
                ReturnDate = returned.ReturnDate,
                TotalRefund = returned.TotalRefund,

                ReturnItems = returned.ReturnItems.Select(item => new ReturnItemResponseDto
                {
                    Id = item.Id,
                    ReturnId = item.ReturnId,
                    OrderItemId = item.OrderItemId,
                    Quantity = item.Quantity,
                    RefundAmount = item.RefundAmount
                }).ToList()
            };
        }

        public async Task<IEnumerable<ReturnResponseDto>> GetAllAsync()
        {
            var storeId = _currentUserService.StoreId;

            var returns = await _returnRepository.GetAllAsync();

            var result = new List<ReturnResponseDto>();

            foreach (var returned in returns)
            {
                var order = await _orderRepository.GetByIdAsync(returned.OrderId);

                if (order == null || order.StoreId != storeId)
                {
                    continue;
                }

                result.Add(new ReturnResponseDto
                {
                    Id = returned.Id,
                    OrderId = returned.OrderId,
                    ReturnDate = returned.ReturnDate,
                    TotalRefund = returned.TotalRefund,

                    ReturnItems = returned.ReturnItems.Select(item => new ReturnItemResponseDto
                    {
                        Id = item.Id,
                        ReturnId = item.ReturnId,
                        OrderItemId = item.OrderItemId,
                        Quantity = item.Quantity,
                        RefundAmount = item.RefundAmount
                    }).ToList(),
                });
            }
            return result;        
        }
    }
}
