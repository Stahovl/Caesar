using Caesar.Core.DTOs;
using Caesar.Core.SharedModels;
using Caesar.Web.Intrefaces;
using Caesar.Web.Models;
using System.Net.Http.Headers;

namespace Caesar.Web.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenService _tokenService;

    private readonly string _baseUrl = "https://localhost:7114/api/";

    public ApiService(HttpClient httpClient, ITokenService tokenService)
    {
        _httpClient = httpClient;
        _tokenService = tokenService;
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    private async Task SetAuthHeader()
    {
        var token = await _tokenService.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<LoginResult> LoginAsync(string username, string password)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}auth/login", new { Username = username, Password = password });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResult>();
                if (result != null && !string.IsNullOrEmpty(result.Token))
                {
                    await _tokenService.SetTokenAsync(result.Token);
                    return new LoginResult { IsSuccess = true, Token = result.Token };
                }
            }

            return new LoginResult { IsSuccess = false, ErrorMessage = "Invalid username or password" };
        }
        catch (Exception ex)
        {
            return new LoginResult { IsSuccess = false, ErrorMessage = $"An error occurred: {ex.Message}" };
        }
    }

    public async Task<bool> RegisterAsync(RegisterViewModel model)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}auth/register", model);
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<MenuItemDto>> GetMenuItemsAsync()
    {
        await SetAuthHeader();
        var response = await _httpClient.GetAsync($"{_baseUrl}menuitems");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<MenuItemDto>>();
    }

    public async Task<MenuItemDto> GetMenuItemAsync(int id)
    {
        await SetAuthHeader();
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
        await SetAuthHeader();
        var response = await _httpClient.DeleteAsync($"{_baseUrl}menuitems/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<ReservationDto>> GetReservationsAsync()
    {
        await SetAuthHeader();
        var response = await _httpClient.GetAsync($"{_baseUrl}reservations");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IEnumerable<ReservationDto>>();
    }

    public async Task<ReservationDto> GetReservationAsync(int id)
    {
        await SetAuthHeader();
        var response = await _httpClient.GetAsync($"{_baseUrl}reservations/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ReservationDto>();
    }

    public async Task<ReservationDto> CreateReservationAsync(CreateReservationRequest request)
    {
        await SetAuthHeader();
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}reservations", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ReservationDto>();
    }

    public async Task<bool> UpdateReservationAsync(ReservationDto reservation)
    {
        await SetAuthHeader();
        var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}reservations/{reservation.Id}", reservation);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> CancelReservationAsync(int id)
    {
        await SetAuthHeader();
        var response = await _httpClient.DeleteAsync($"{_baseUrl}reservations/{id}");
        return response.IsSuccessStatusCode;
    }
}
