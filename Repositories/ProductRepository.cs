using Microsoft.EntityFrameworkCore;
using megamart_backend.Data;
using megamart_backend.Models;
using Npgsql.PostgresTypes;

namespace megamart_backend.Repositories
{
    public class ProductRepository:GenericRepository<Product>,IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {

        } 

        public async Task<IReadOnlyList<Product>> GetProductsWithCategoryAsync()
        {
            return await _context.Products
                            .Include(p => p.Category)
                            .AsNoTracking()
                            .ToListAsync();
        }

        public async Task<Product?> GetProductWithDetailsAsync(int id)
        {
            return await _context.Products
                       .Include(p => p.Category)
                       .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IReadOnlyList<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products
                            .Where(p => p.CategoryId == categoryId)
                            .Include(p => p.Category)
                            .AsNoTracking()
                            .ToListAsync();
        }



    }
}
