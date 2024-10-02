using Caesar.Core.Entities;

namespace Caesar.Core.Interfaces;

public interface IReservationRepository
{
    public Task<Reservation> GetByIdAsync(int id);
    public Task<IEnumerable<Reservation>> GetAllAsync();
    public Task<IEnumerable<Reservation>> GetByUserIdAsync(string userId);
    public Task<Reservation> AddAsync(Reservation reservation);
    public Task UpdateAsync(Reservation reservation);
    public Task DeleteAsync(int id);
    public Task<IEnumerable<DateTime>> GetAvailableSlotsAsync(DateTime startDate, DateTime endDate);
}
