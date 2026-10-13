using megamart_backend.Models;

namespace megamart_backend.Repositories
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartByUserIdAsync(int userId);
        Task AddCartAsync(Cart cart);
    }
}
