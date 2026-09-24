namespace megamart_backend.DTOs
{
    public record RegisterDto
    (
        string Name,
        string Email,
        string Password
    );

    public record LoginDto
    (
        string Email,
        string Password
    );

    public record AuthResponseDto
    (
        int Id,
        string Name,
        string Email,
        string Role,
        string Token
    );

    public record UpdateUserRoleDto(string Role);
}
