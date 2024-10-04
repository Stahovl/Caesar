namespace Caesar.App.Interfaces;

public interface ITokenService
{
    public Task<bool> IsAuthenticatedAsync();
    public Task SetTokenAsync(string token);
    public Task<string> GetTokenAsync();
    public Task ClearTokenAsync();
}
