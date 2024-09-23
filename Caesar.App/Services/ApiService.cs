using System.Net.Http.Json;
using System.Net.Http.Headers;
using Caesar.Core.DTOs;
using Caesar.App.Models;
using System.Diagnostics;

namespace Caesar.App.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenService _tokenService;

    private readonly string _baseUrl = "https://localhost:7114/api/";
    public ApiService(ITokenService tokenService)
    {
        _httpClient = new HttpClient();
        _tokenService = tokenService;
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        Console.WriteLine($"API base URL: {_baseUrl}");
    }

    private async Task SetAuthHeader()
    {
        var token = await _tokenService.GetTokenAsync();
        Debug.WriteLine($"SetAuthHeader - Token: {token?.Substring(0, 10)}...");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Debug.WriteLine("Authorization header set successfully");
        }
        else
        {
            Debug.WriteLine("Token is null or empty, Authorization header not set");
        }
    }

    public async Task<LoginResult> LoginAsync(string username, string password)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}auth/login", new { Username = username, Password = password });

            Console.WriteLine($"Status Code: {response.StatusCode}");
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Content: {content}");

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
            Console.WriteLine($"Exception in LoginAsync: {ex}");
            return new LoginResult { IsSuccess = false, ErrorMessage = $"An error occurred: {ex.Message}" };
        }
    }

    public async Task<IEnumerable<MenuItemDto>> GetMenuItemsAsync()
    {
        Debug.WriteLine("Starting GetMenuItemsAsync");
        await SetAuthHeader();
        var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}menuitems");
        Debug.WriteLine($"Request URI: {request.RequestUri}");
        Debug.WriteLine($"Request Headers: {string.Join(", ", request.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}"))}");

        var response = await _httpClient.SendAsync(request);
        Debug.WriteLine($"Response status: {response.StatusCode}");

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            Debug.WriteLine("Unauthorized. Token might be invalid or expired.");
            return new List<MenuItemDto>();
        }

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Debug.WriteLine($"Response content: {content}");
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
        try
        {
            await SetAuthHeader();
            HttpResponseMessage response;
            string requestUrl;

            if (menuItem.Id == 0)
            {
                requestUrl = $"{_baseUrl}menuitems";
                Console.WriteLine($"Sending POST request to {requestUrl}");
                response = await _httpClient.PostAsJsonAsync(requestUrl, menuItem);
            }
            else
            {
                requestUrl = $"{_baseUrl}menuitems/{menuItem.Id}";
                Console.WriteLine($"Sending PUT request to {requestUrl}");
                response = await _httpClient.PutAsJsonAsync(requestUrl, menuItem);
            }

            Console.WriteLine($"Request URL: {requestUrl}");
            Console.WriteLine($"Request content: {System.Text.Json.JsonSerializer.Serialize(menuItem)}");
            Console.WriteLine($"Response status code: {response.StatusCode}");

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response content: {content}");

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in SaveMenuItemAsync: {ex}");
            return false;
        }
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
