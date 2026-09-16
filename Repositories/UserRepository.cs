using Microsoft.EntityFrameworkCore;
using megamart_backend.Data;
using megamart_backend.Models;


namespace megamart_backend.Repositories
{
    public class UserRepository:GenericRepository<User>,IUserRepository 
    {
        public UserRepository(AppDbContext context): base(context)
        {

        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.
                AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

    }
}
