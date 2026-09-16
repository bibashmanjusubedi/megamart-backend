using megamart_backend.Models;

namespace megamart_backend.Repositories
{
    public interface IUserRepository: IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
    }
}
