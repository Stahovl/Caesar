namespace Caesar.Web.Intrefaces;

public interface ITokenService
{
    public Task SetTokenAsync(string token);
    public Task<string> GetTokenAsync();
    public Task ClearTokenAsync();
}
