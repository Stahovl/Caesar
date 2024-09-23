namespace Caesar.Mobile.Services;

public interface IAuthService
{
    public Task SetTokenAsync(string token);
    public Task<string> GetTokenAsync();
    public Task ClearTokenAsync();
    public bool IsAuthenticated { get; }
}
