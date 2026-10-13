using megamart_backend.Data;
using Microsoft.EntityFrameworkCore;
using megamart_backend.Models;

namespace megamart_backend.Repositories
{
    public class CartRepository: ICartRepository
    {
        private readonly AppDbContext _context;
        
        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cart?> GetCartByUserIdAsync(int userId)
        {
            return await _context.Carts
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task AddCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
        }

    }
}
