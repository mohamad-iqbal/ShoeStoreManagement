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
    public class ProductVariantService : IProductVariantService
    {
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductVariantService(IProductVariantRepository productVariantRepository, IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productVariantRepository = productVariantRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductVariantResponseDto> CreateProductVariantAsync(CreateProductVariantDto dto)
        {
            var product = await _productRepository.
                GetByIdAsync(dto.ProductId);
            if (product == null)
            {
                throw new NotFoundException("Product not found");
            }

            bool exist = await _productVariantRepository
                .ExistProductNameAndSizeAsync(
                product.Name,
                dto.Size,
                product.StoreId);

            var productVariant = new ProductVariant
            {
                ProductId = dto.ProductId,
                Size = dto.Size,
                StockQty = dto.StockQty
            };

            await _productVariantRepository.AddAsync(productVariant);
            await _unitOfWork.SaveChangesAsync();

            return new ProductVariantResponseDto
            {
                Id = productVariant.Id,
                ProductId = productVariant.ProductId,
                Size = productVariant.Size,
                StockQty = productVariant.StockQty
            };
        }

        public async Task<ProductVariantResponseDto?> UpdateProductVariantAsync(int id, UpdateProductVariantDto dto)
        {
            var productVariant = await _productVariantRepository.GetByIdAsync(id);
            if (productVariant == null)
            {
                return null;
            }

            var product = await _productRepository
                .GetByIdAsync(dto.ProductId);
            if (product == null)
            {
                throw new BadRequestException("Product not found");
            }

            productVariant.ProductId = dto.ProductId;
            productVariant.Size = dto.Size;
            productVariant.StockQty = dto.StockQty;

            await _productVariantRepository.UpdateAsync(productVariant);
            await _unitOfWork.SaveChangesAsync();

            return new ProductVariantResponseDto
            {
                Id = productVariant.Id,
                ProductId = productVariant.ProductId,
                Size = productVariant.Size,
                StockQty = productVariant.StockQty
            };
        }

        public async Task DeleteProductVariantAsync(int id)
        {
            var productVariant = await _productVariantRepository.GetByIdAsync(id);
            if (productVariant == null)
            {
                throw new NotFoundException("Variant not found");
            }

            await _productVariantRepository.DeleteAsync(productVariant);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
