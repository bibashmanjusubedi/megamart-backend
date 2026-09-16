using megamart_backend.Models;

namespace megamart_backend.Repositories
{
    public interface IProductRepository: IGenericRepository<Product>
    {
        Task<IReadOnlyList<Product>> GetProductsWithCategoryAsync();
        Task<Product?> GetProductWithDetailsAsync(int id);

        Task<IReadOnlyList<Product>> GetProductsByCategoryAsync(int categoryId);
    }
}
