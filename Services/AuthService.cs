namespace FastLead.Services
{
    using FastLead.Interfaces;
    using FastLead.Models;
    using FastLead.Repositories;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using BCrypt.Net;
    using FastLead.Enums;

    public class AuthService
    {
        IUserRepository _userRepo;
        IConfiguration _configBuilder;
        public AuthService(IUserRepository repo, IConfiguration configBuilder)
        {
            _userRepo = repo;
            _configBuilder = configBuilder;
        }
        public async Task<bool> Register(string Name, string password)
        {
            if (await _userRepo.Exists(Name))
            {
                return false;
            }
            string hashedPassword = PasswordHasher.GeneratePassword(password);
            await _userRepo.AddAsync(User.Create(new Guid(), Name, hashedPassword));
            return true;
        }

        public async Task<string> Login(string Name, string password)
        {
            User? user = await _userRepo.GetByUsernameAsync(Name);
            if (user == null || !PasswordHasher.VerifyPassword(password, user.Password))
            {
                throw new UnauthorizedAccessException("Неверное имя пользователя или пароль.");
            }
            return GenerateToken(Name, user.Role);
        }

        private string GenerateToken(string Name, Role role)
        {
            var jwtSettings = _configBuilder.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, Name),
                    new Claim(ClaimTypes.Role, role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
