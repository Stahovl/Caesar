using Caesar.Core.DTOs;
using System.Net.Http.Json;

namespace Caesar.Mobile.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "https://your-api-url.com/api/"; // Замените на URL вашего API

    public ApiService()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
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
