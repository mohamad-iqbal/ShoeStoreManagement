using ShoeStoreManagement.Application.Dtos;
using ShoeStoreManagement.Application.Exceptions;
using ShoeStoreManagement.Application.Interfaces;
using ShoeStoreManagement.Domain.Entities;
using ShoeStoreManagement.Domain.Enums;
using ShoeStoreManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository productRepository,IProductVariantRepository productVariantRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _productVariantRepository = productVariantRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductResponseDto> CreateProductAsync(CreateProductDto dto)
        {
            if (dto.Variants == null || !dto.Variants.Any())
            {
                throw new BadRequestException("Product must have variant");
            }

            foreach (var variant in dto.Variants)
            {
                bool exists = await _productVariantRepository
                    .ExistProductNameAndSizeAsync(
                    dto.Name,
                    variant.Size,
                    dto.StoreId);

                if (exists)
                {
                    throw new BadRequestException($"Product {dto.Name} with size {variant.Size} already exists");
                }
            }

            var product = new Product
            {
                Sku = dto.Sku,
                Name = dto.Name,
                StoreId = dto.StoreId,
                Price = dto.Price
            };

            await _productRepository.AddAsync(product);
            
            foreach(var variantDto in dto.Variants )
            {
                var variant = new ProductVariant
                {
                    Product = product,
                    Size = variantDto.Size,
                    StockQty = variantDto.StockQty
                };
                await _productVariantRepository.AddAsync(variant);
            }

            await _unitOfWork.SaveChangesAsync();

            return new ProductResponseDto
            {
                Id = product.Id,
                Sku = product.Sku,
                Name = product.Name,
                StoreId = product.StoreId,
                Price = product.Price,

                Variants = product.ProductVariants
                    .Select(v => new ProductVariantResponseDto
                    {
                        Id = v.Id,
                        ProductId = v.ProductId,
                        Size = v.Size,
                        StockQty = v.StockQty
                    }).ToList()
            };
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            return products.Select(product => new ProductResponseDto
            {
                Id = product.Id,
                Sku = product.Sku,
                Name = product.Name,
                StoreId = product.StoreId,
                Price= product.Price,

                Variants = product.ProductVariants
                .Select(v => new ProductVariantResponseDto
                {
                    Id = v.Id,
                    ProductId = v.ProductId,
                    Size = v.Size,
                    StockQty = v.StockQty
                }).ToList()
            });
        }

        public async Task<ProductResponseDto?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if(product == null)
                return null;

            var role = _currentUserService.Role;
            var storeId = _currentUserService.StoreId;
            if (role != Role.Admin)
            {
                if (product.StoreId != storeId)
                {
                    throw new ForbiddenException(
                        "You cannot access this product");
                }
            }

            return new ProductResponseDto
            {
                Id = product.Id,
                Sku = product.Sku,
                Name = product.Name,
                StoreId = product.StoreId,
                Price = product.Price,

                Variants = product.ProductVariants
                .Select(v => new ProductVariantResponseDto
                {
                    Id = v.Id,
                    ProductId = v.ProductId,
                    Size = v.Size,
                    StockQty = v.StockQty
                }).ToList()
            };
        }

        public async Task<ProductResponseDto?> UpdateProductAsync(int id, UpdateProductDto dto)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return null;

            product.Name = dto.Name;
            product.Sku = dto.Sku;
            product.StoreId = dto.StoreId;
            product.Price = dto.Price;
            product.ProductVariants.Clear();
            foreach (var variantDto in dto.Variants)
            {
                product.ProductVariants.Add(new ProductVariant
                {
                    Size = variantDto.Size,
                    StockQty = variantDto.StockQty
                });
            }
            await _productRepository.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Sku = product.Sku,
                StoreId = product.StoreId,
                Price = product.Price,
                Variants = product.ProductVariants
                    .Select(v => new ProductVariantResponseDto
                    {
                        Size = v.Size,
                        StockQty = v.StockQty
                    }).ToList()
            };
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                throw new NotFoundException("Product Not Found");
            }

            await _productRepository.DeleteAsync(product);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
