using Caesar.Core.DTOs;
using Caesar.Web.Models;

namespace Caesar.Web.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ReservationDto> CreateReservationAsync(CreateReservationRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Reservations", request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<ReservationDto>();
    }
}
