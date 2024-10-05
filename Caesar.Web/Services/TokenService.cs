using Caesar.Web.Intrefaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

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
        Console.WriteLine("Clear token start, key = " + TokenKey);
        _httpContextAccessor.HttpContext.Session.Remove(TokenKey);
        await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        Console.WriteLine("Clear token end");
    }
}
