using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using megamart_backend.DTOs;
using megamart_backend.Models;
using megamart_backend.Repositories;
using megamart_backend.Services.Interfaces;

namespace megamart_backend.Services
{
    public class AuthService:IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        public AuthService(IUnitOfWork unitOfWork,IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
        }

        public async Task<AuthResponseDto> RegisterAsync (RegisterDto dto)
        {
            // 1. Verify user does not already exist
            var emailExists = await _unitOfWork.Users.EmailExistsAsync(dto.Email);
            if (emailExists)
            {
                throw new InvalidOperationException("An account with this email already exists");
            }

            // 2. Hash password with BCrypt
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // 3. Create User entity
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email.Trim().ToLowerInvariant(),
                PasswordHash = passwordHash,
                Role = "Customer",
                CreatedAt = DateTime.UtcNow
            };

            // 4. Save through Unit of Work
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();


            // 5. Generate token and return response DTO
            var token = GenerateJwtToken(user);
            return new AuthResponseDto(user.Id,user.Name, user.Email, user.Role, token);
        }


        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            // 1. Retrieve user by email
            var user = await _unitOfWork.Users.GetByEmailAsync(dto.Email.Trim().ToLowerInvariant());

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            // 2. Verify hashed password
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            // Generate token and return response DTO
            var token = GenerateJwtToken(user);

            return new AuthResponseDto(user.Id, user.Name, user.Name, user.Role, token);
        }


        private string GenerateJwtToken(User user)
        {
            var secretKey = _config["JwtSettings:Secret"]
                ?? throw new InvalidOperationException("JWT Secret is not configured in appsettings.json");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Name,user.Name),
                new Claim(ClaimTypes.Role,user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims:claims,
                expires:DateTime.UtcNow.AddDays(7),
                signingCredentials:credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
