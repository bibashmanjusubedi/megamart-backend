using Microsoft.EntityFrameworkCore;
using megamart_backend.Data;
using megamart_backend.Models;

namespace megamart_backend.Repositories
{
    public class CategoryRepository: GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context) { }

        public async Task<Category?> GetCategoryWithProductsAsync(int id)
        {
            return await _context.Categories
                            .Include(c => c.Products)
                            .FirstOrDefaultAsync(c => c.Id == id);

        }
  
    }
}
