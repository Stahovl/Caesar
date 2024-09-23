using Caesar.Core.Entities;

namespace Caesar.Core.Interfaces;

public interface IAuthService
{
    public Task<(bool IsSuccess, string Token)> LoginAsync(string username, string password);
    public string GenerateJwtToken(User user);
}
