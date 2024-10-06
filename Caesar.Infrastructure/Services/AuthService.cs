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

/// <summary>
/// Provides authentication services for the application.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly CaesarDbContext _context;

    /// <summary>
    /// Initializes a new instance of the AuthService class.
    /// </summary>
    /// <param name="configuration">The configuration containing JWT settings.</param>
    /// <param name="context">The database context for user operations.</param>
    public AuthService(IConfiguration configuration, CaesarDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    /// <summary>
    /// Authenticates a user and generates a JWT token upon successful login.
    /// </summary>
    /// <param name="username">The username of the user attempting to log in.</param>
    /// <param name="password">The password of the user attempting to log in.</param>
    /// <returns>
    /// A tuple containing:
    /// - IsSuccess: A boolean indicating whether the login was successful.
    /// - Token: The generated JWT token if login was successful, otherwise null.
    /// - UserId: The ID of the authenticated user if login was successful, otherwise 0.
    /// </returns>
    public async Task<(bool IsSuccess, string Token, int UserId)> LoginAsync(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        if (user == null || !VerifyPasswordHash(password, user.PasswordHash))
        {
            return (false, null, 0);
        }

        var token = GenerateJwtToken(user);
        return (true, token, user.Id);
    }

    /// <summary>
    /// Generates a JWT token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to generate the token.</param>
    /// <returns>A JWT token as a string.</returns>
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

    /// <summary>
    /// Verifies a plain text password against a stored hash.
    /// </summary>
    /// <param name="password">The plain text password to verify.</param>
    /// <param name="storedHash">The stored hash to compare against.</param>
    /// <returns>True if the password matches the hash, false otherwise.</returns>
    private bool VerifyPasswordHash(string password, string storedHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, storedHash);
    }


    // The following methods are not implemented in the server-side version
    // and are left to satisfy the interface requirements.

    /// <summary>
    /// Not implemented. Throws NotImplementedException.
    /// </summary>
    public Task<string> GetTokenAsync() => throw new NotImplementedException();

    /// <summary>
    /// Not implemented. Throws NotImplementedException.
    /// </summary>
    public Task SetTokenAsync(string token) => throw new NotImplementedException();

    /// <summary>
    /// Not implemented. Throws NotImplementedException.
    /// </summary>
    public Task ClearTokenAsync() => throw new NotImplementedException();

    /// <summary>
    /// Not implemented. Throws NotImplementedException.
    /// </summary>
    public bool IsAuthenticated => throw new NotImplementedException();
}
