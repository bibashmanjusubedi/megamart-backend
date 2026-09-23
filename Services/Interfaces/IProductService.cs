using megamart_backend.DTOs;

namespace megamart_backend.Services.Interfaces
{
    public interface IProductService
    {
        Task<IReadOnlyList<ProductResponseDto>> GetAllProductsAsync(int? categoryId = null);
        Task<ProductResponseDto?> GetProductByIdAsync(int id);

        Task<ProductResponseDto> CreateProductAsync(ProductCreateDto dto);

        Task<bool> UpdateProductAsync(int id, ProductUpdateDto dto);

        Task<bool> DeleteProductAsync(int id);

    }
}
