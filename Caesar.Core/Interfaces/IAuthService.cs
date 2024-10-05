using Caesar.Core.Entities;

namespace Caesar.Core.Interfaces;

public interface IAuthService
{
    public Task<(bool IsSuccess, string Token, int UserId)> LoginAsync(string username, string password);
    public string GenerateJwtToken(User user);
}
