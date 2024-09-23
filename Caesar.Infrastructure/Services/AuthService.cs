using Caesar.Core.Entities;
using Caesar.Core.Interfaces;
using Caesar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Caesar.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly CaesarDbContext _context;

    public AuthService(IConfiguration configuration, CaesarDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    public async Task<(bool IsSuccess, string Token)> LoginAsync(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        if (user == null || !VerifyPasswordHash(password, user.PasswordHash))
        {
            return (false, null);
        }

        var token = GenerateJwtToken(user);
        return (true, token);
    }

    public string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private bool VerifyPasswordHash(string password, string storedHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, storedHash);
    }

    // Эти методы не используются в серверной реализации, но оставлены для соответствия интерфейсу
    public Task<string> GetTokenAsync() => throw new NotImplementedException();
    public Task SetTokenAsync(string token) => throw new NotImplementedException();
    public Task ClearTokenAsync() => throw new NotImplementedException();
    public bool IsAuthenticated => throw new NotImplementedException();
}
