using Caesar.Core.DTOs;

namespace Caesar.Mobile.Services;

public interface IApiService
{
    public Task<IEnumerable<MenuItemDto>> GetMenuItemsAsync();
    public Task<MenuItemDto> GetMenuItemAsync(int id);
    public Task<bool> SaveMenuItemAsync(MenuItemDto menuItem);
    public Task<bool> DeleteMenuItemAsync(int id);

    public Task<IEnumerable<ReservationDto>> GetReservationsAsync();
    public Task<ReservationDto> GetReservationAsync(int id);
    public Task<bool> CreateReservationAsync(ReservationDto reservation);
    public Task<bool> UpdateReservationAsync(ReservationDto reservation);
    public Task<bool> CancelReservationAsync(int id);
}
