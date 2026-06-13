using ShoeStoreManagement.Application.Dtos;
using ShoeStoreManagement.Application.Interfaces;
using ShoeStoreManagement.Domain.Enums;
using ShoeStoreManagement.Domain.Interfaces;
using ShoeStoreManagement.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoeStoreManagement.Domain.Entities;

namespace ShoeStoreManagement.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IInventoryTransactionRepository _inventoryTransactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService (IOrderRepository orderRepository, ICurrentUserService currentUserService, ICustomerRepository customerRepository, IStoreRepository storeRepository, IProductVariantRepository productVariantRepository, IOrderItemRepository orderItemRepository, IInventoryTransactionRepository inventoryTransactionRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _currentUserService = currentUserService;
            _customerRepository = customerRepository;
            _storeRepository = storeRepository;
            _productVariantRepository = productVariantRepository;
            _orderItemRepository = orderItemRepository;
            _inventoryTransactionRepository = inventoryTransactionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto dto)
        {
            var role = _currentUserService.Role;
            var storeId = _currentUserService.StoreId;
            if (role != Role.Sales)
            {
                throw new ForbiddenException("You cannot create this order");
            }

            var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);
            if (customer == null)
            {
                throw new BadRequestException("Customer not found");
            }

            var store = await _storeRepository.GetByIdAsync(storeId);
            if (store == null)
            {
                throw new BadRequestException("Store not found");
            }

            if (!dto.OrderItems.Any())
            {
                throw new BadRequestException("Order must contain at least one item");
            }

            var existingOrder = await _orderRepository.GetByOrderNumberAsync(dto.OrderNumber);
            if (existingOrder != null)
            {
                throw new BadRequestException("Order number already exits");
            }


            decimal totalAmount = 0;

            foreach (var item in dto.OrderItems)
            {
                var productVariant = await _productVariantRepository.GetByIdWithProductAsync(item.ProductVariantId);
                if (productVariant == null)
                {
                    throw new BadRequestException($"Product variant {item.ProductVariantId} not found");
                }

                if (item.Quantity <= 0)
                {
                    throw new BadRequestException("Quantity must be greater than zero");
                }

                if (productVariant.StockQty < item.Quantity)
                {
                    throw new BadRequestException("Stock not sufficient");
                }

                totalAmount += item.Quantity * productVariant.Product.Price;
            }

            var order = new Order
            {
                StoreId = storeId,
                CustomerId = dto.CustomerId,
                OrderNumber = dto.OrderNumber,
                TotalAmount = totalAmount,
                Date = DateTime.UtcNow,
                Status = Status.Completed
            };

            await _orderRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            foreach (var item in dto.OrderItems)
            {
                var productVariant = await _productVariantRepository.GetByIdWithProductAsync(item.ProductVariantId);
                if (productVariant == null)
                {
                    throw new BadRequestException($"Product variant {item.ProductVariantId} not found");
                }

                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductVariantId = item.ProductVariantId,
                    Quantity = item.Quantity,
                    Price = productVariant.Product.Price
                };

                await _orderItemRepository.AddAsync(orderItem);

                productVariant.StockQty -= item.Quantity;

                await _productVariantRepository.UpdateAsync(productVariant);

                var inventoryTransaction = new InventoryTransaction
                {
                    ProductVariantId = item.ProductVariantId,
                    Type = TypeTransaction.Sale,
                    Quantity = item.Quantity,
                    Date = DateTime.UtcNow,
                    OrderId = order.Id,
                    UserId = _currentUserService.Id
                };

                await _inventoryTransactionRepository.AddAsync(inventoryTransaction);
            }

            await _unitOfWork.SaveChangesAsync();

            return new OrderResponseDto
            {
                Id = order.Id,
                StoreId = order.StoreId,
                CustomerId = order.CustomerId,
                OrderNumber = order.OrderNumber,
                TotalAmount = order.TotalAmount,
                Date = order.Date,
                Status = order.Status,
                OrderItems = dto.OrderItems.Select(item => new OrderItemsResponseDto
                {
                    ProductVariantId = item.ProductVariantId,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };
        }

        public async Task<IEnumerable<OrderResponseDto>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.Select(order => new OrderResponseDto
            {
                Id = order.Id,
                StoreId = order.StoreId,
                StoreName = order.Store.Name,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer.Name,
                OrderNumber = order.OrderNumber,
                TotalAmount = order.TotalAmount,
                Date = order.Date,
                Status = order.Status,

                OrderItems = order.OrderItems.Select(item =>
                new OrderItemsResponseDto
                {
                    Id = item.Id,
                    OrderId = item.OrderId,
                    ProductVariantId = item.ProductVariantId,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            }).ToList();
        }

        public async Task<OrderResponseDto?> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return null;
            }

            var storeId = _currentUserService.StoreId;
            if (order.StoreId != storeId)
            {
                throw new ForbiddenException("You cannot access this order");
            }

            return new OrderResponseDto
            {
                Id = order.Id,
                StoreId = order.StoreId,
                StoreName = order.Store.Name,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer.Name,
                OrderNumber = order.OrderNumber,
                TotalAmount = order.TotalAmount,
                Date = order.Date,
                Status = order.Status,

                OrderItems = order.OrderItems.Select(item =>
                new OrderItemsResponseDto
                {
                    Id = item.Id,
                    OrderId = item.OrderId,
                    ProductVariantId = item.ProductVariantId,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };
        }

        public async Task<OrderResponseDto> CancelOrderAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                throw new NotFoundException("Order not found");
            }

            var storeId = _currentUserService.StoreId;
            var store = await _storeRepository.GetByIdAsync(storeId);
            if (store == null)
            {
                throw new BadRequestException("Store not found");
            }
            
            if (order.StoreId != storeId)
            {
                throw new ForbiddenException("You cannot access this order");
            }

            if (order.Status == Status.Canceled)
            {
                throw new BadRequestException("Order already canceled");
            }

            if (order.Status == Status.Returned || order.Status == Status.PartiallyReturned)
            {
                throw new BadRequestException("Returned order cannot be cancelled");
            }

            foreach (var item in order.OrderItems)
            {
                var productVariant = await _productVariantRepository.GetByIdAsync(item.ProductVariantId);

                if (productVariant == null)
                {
                    throw new BadRequestException($"Product variant {item.ProductVariantId} not found");
                }

                productVariant.StockQty += item.Quantity;

                await _productVariantRepository.UpdateAsync(productVariant);

                var inventoryTransaction = new InventoryTransaction
                {
                    ProductVariantId = item.ProductVariantId,
                    Type = TypeTransaction.Cancel,
                    Quantity = item.Quantity,
                    Date = DateTime.UtcNow,
                    OrderId = item.OrderId,
                    UserId = _currentUserService.Id
                };

                await _inventoryTransactionRepository.AddAsync(inventoryTransaction);
            }

            order.Status = Status.Canceled;

            await _orderRepository.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return new OrderResponseDto
            {
                Id = order.Id,
                StoreId = order.StoreId,
                CustomerId = order.CustomerId,
                OrderNumber = order.OrderNumber,
                TotalAmount = order.TotalAmount,
                Date = order.Date,
                Status = order.Status,
                OrderItems = order.OrderItems.Select(item => new OrderItemsResponseDto
                {
                    ProductVariantId = item.ProductVariantId,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };
        }
    }
}
