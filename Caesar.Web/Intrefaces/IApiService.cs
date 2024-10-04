using Caesar.Core.DTOs;
using Caesar.Core.SharedModels;
using Caesar.Web.Models;

namespace Caesar.Web.Intrefaces;

public interface IApiService
{
    public Task<LoginResult> LoginAsync(string username, string password);
    public Task<bool> RegisterAsync(RegisterViewModel model);
    public Task<IEnumerable<MenuItemDto>> GetMenuItemsAsync();
    public Task<MenuItemDto> GetMenuItemAsync(int id);
    public Task<bool> SaveMenuItemAsync(MenuItemDto menuItem);
    public Task<bool> DeleteMenuItemAsync(int id);
    public Task<IEnumerable<ReservationDto>> GetReservationsAsync();
    public Task<ReservationDto> GetReservationAsync(int id);
    public Task<ReservationDto> CreateReservationAsync(CreateReservationRequest request);
    public Task<bool> UpdateReservationAsync(ReservationDto reservation);
    public Task<bool> CancelReservationAsync(int id);
}
