using Caesar.Core.DTOs;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace Caesar.Mobile.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthService _authService;
    private readonly string _baseUrl = "https://your-api-url.com/api/";

    public ApiService(IAuthService authService)
    {
        _httpClient = new HttpClient();
        _authService = authService;
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    private async Task SetAuthHeader()
    {
        var token = await _authService.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<LoginResult> LoginAsync(string username, string password)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}auth/login", new { Username = username, Password = password });
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<LoginResult>();
            return result;
        }
        return new LoginResult { IsSuccess = false };
    }

    public async Task<IEnumerable<MenuItemDto>> GetMenuItemsAsync()
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}menuitems");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<MenuItemDto>>();
    }

    public async Task<MenuItemDto> GetMenuItemAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}menuitems/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<MenuItemDto>();
    }

    public async Task<bool> SaveMenuItemAsync(MenuItemDto menuItem)
    {
        await SetAuthHeader();
        HttpResponseMessage response;
        if (menuItem.Id == 0)
        {
            response = await _httpClient.PostAsJsonAsync($"{_baseUrl}menuitems", menuItem);
        }
        else
        {
            response = await _httpClient.PutAsJsonAsync($"{_baseUrl}menuitems/{menuItem.Id}", menuItem);
        }
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteMenuItemAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{_baseUrl}menuitems/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<ReservationDto>> GetReservationsAsync()
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}reservations");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<ReservationDto>>();
    }

    public async Task<ReservationDto> GetReservationAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}reservations/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ReservationDto>();
    }

    public async Task<bool> CreateReservationAsync(ReservationDto reservation)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}reservations", reservation);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateReservationAsync(ReservationDto reservation)
    {
        var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}reservations/{reservation.Id}", reservation);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> CancelReservationAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{_baseUrl}reservations/{id}");
        return response.IsSuccessStatusCode;
    }
}

public class LoginResult
{
    public bool IsSuccess { get; set; }
    public string Token { get; set; }
}
