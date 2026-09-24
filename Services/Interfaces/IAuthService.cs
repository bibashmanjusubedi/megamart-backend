using megamart_backend.DTOs;

namespace megamart_backend.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<bool> UpdateUserRoleAsync(int targetUserId, string newRole, int currentAdminId);
    }
}
