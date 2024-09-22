using Caesar.Core.Entities;

namespace Caesar.Core.Interfaces;

public interface IAuthService
{
    public string GenerateJwtToken(User user);

    // Здесь могут быть дополнительные методы, например:
    // Task<bool> ValidateUserAsync(string username, string password);
    // Task<User> GetUserByUsernameAsync(string username);
}
