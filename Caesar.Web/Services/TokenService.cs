using Caesar.Web.Intrefaces;

namespace Caesar.Web.Services;

public class TokenService : ITokenService
{
    private const string TokenKey = "AuthToken";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task SetTokenAsync(string token)
    {
        _httpContextAccessor.HttpContext.Session.SetString(TokenKey, token);
    }

    public async Task<string> GetTokenAsync()
    {
        return _httpContextAccessor.HttpContext.Session.GetString(TokenKey);
    }

    public async Task ClearTokenAsync()
    {
        _httpContextAccessor.HttpContext.Session.Remove(TokenKey);
    }
}
