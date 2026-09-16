using megamart_backend.Models;


namespace megamart_backend.Repositories
{
    public interface ICategoryRepository: IGenericRepository<Category>
    {
        Task<Category?> GetCategoryWithProductsAsync(int id);
    }
}
