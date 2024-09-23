namespace Caesar.Mobile.Services;

public class AuthService : IAuthService
{
    private const string TokenKey = "auth_token";

    public bool IsAuthenticated => !string.IsNullOrEmpty(SecureStorage.GetAsync(TokenKey).Result);

    public async Task SetTokenAsync(string token)
    {
        await SecureStorage.SetAsync(TokenKey, token);
    }

    public async Task<string> GetTokenAsync()
    {
        return await SecureStorage.GetAsync(TokenKey);
    }

    public async Task ClearTokenAsync()
    {
        SecureStorage.Remove(TokenKey);
    }
}
