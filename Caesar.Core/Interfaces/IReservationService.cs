using Caesar.Core.DTOs;

namespace Caesar.Core.Interfaces;

public interface IReservationService
{
    public Task<ReservationDto> GetReservationByIdAsync(int id);
    public Task<IEnumerable<ReservationDto>> GetAllReservationsAsync();
    public Task<ReservationDto> CreateReservationAsync(ReservationDto reservationDto, List<int> menuItemIds);
    public Task UpdateReservationAsync(ReservationDto reservationDto);
    public Task DeleteReservationAsync(int id);
    public Task<IEnumerable<ReservationDto>> GetReservationsByUserIdAsync(int userId);
    public Task<IEnumerable<DateTime>> GetAvailableSlotsAsync(DateTime startDate, DateTime endDate);
}
