using megamart_backend.DTOs;
using megamart_backend.Models;
using megamart_backend.Repositories;
using megamart_backend.Services.Interfaces;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace megamart_backend.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<ProductResponseDto>> GetAllProductsAsync(int? categoryId = null)
        {
            IReadOnlyList<Product> products;

            if (categoryId.HasValue)
            {
                products = await _unitOfWork.Products.FindAsync(p => p.CategoryId == categoryId.Value);
            }

            else
            {
                products = await _unitOfWork.Products.GetAllAsync();
            }

            return products.Select(p => new ProductResponseDto(
               p.Id,
               p.Name,
               p.Price,
               p.StockQuantity,
               p.ImageUrl,
               p.Description,
               p.CategoryId,
               p.Category?.Name
            )).ToList();

        }


        public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                return null;
            }

            return new ProductResponseDto(
               product.Id,
               product.Name,
               product.Price,
               product.StockQuantity,
               product.ImageUrl,
               product.Description,
               product.CategoryId,
               product.Category?.Name
            );

        }

        public async Task<ProductResponseDto> CreateProductAsync(ProductCreateDto dto)
        {
            // Verify the referenced category exists
            var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {dto.CategoryId} does not exist.");
            }

            var product = new Product
            {
                Name = dto.Name.Trim(),
                Description = dto.Description?.Trim(),
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                ImageUrl = dto.ImageUrl?.Trim(),
                CategoryId = dto.CategoryId
            };


            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CompleteAsync();

            return new ProductResponseDto(
                product.Id,
                product.Name,
                product.Price,
                product.StockQuantity,
                product.ImageUrl,
                product.Description,
                product.CategoryId,
                product.Category.Name
            );
        }


        public async Task<bool> UpdateProductAsync(int id, ProductUpdateDto dto)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                return false;
            }

            // If updating category reference,ensure target still exists
            var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {dto.CategoryId} does not exist.");
            }

            product.Name = dto.Name.Trim();
            product.Description = dto.Description?.Trim();
            product.Price = dto.Price;
            product.StockQuantity = dto.StockQuantity;
            product.ImageUrl = dto.ImageUrl?.Trim();
            product.CategoryId = dto.CategoryId;

            _unitOfWork.Products.Update(product);
            await _unitOfWork.CompleteAsync();

            return true;
        }


        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);

            if (product == null)
            {
                return false;
            }

            _unitOfWork.Products.Delete(product);
            await _unitOfWork.CompleteAsync();

            return true;
        }
        


    }
}
