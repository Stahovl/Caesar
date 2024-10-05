using Caesar.Core.Entities;
using Caesar.Core.Interfaces;
using Caesar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Caesar.Infrastructure.Repositories;

/// <summary>
/// Repository for managing Reservation entities in the database.
/// </summary>
public class ReservationRepository : IReservationRepository
{
    private readonly CaesarDbContext _context;

    /// <summary>
    /// Constructor that initializes the database context.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ReservationRepository(CaesarDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Asynchronously gets a Reservation entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <returns>The Reservation entity.</returns>
    public async Task<Reservation> GetByIdAsync(int id)
    {
        return await _context.Reservations.FindAsync(id);
    }

    /// <summary>
    /// Asynchronously gets all Reservation entities.
    /// </summary>
    /// <returns>A list of Reservation entities.</returns>
    public async Task<IEnumerable<Reservation>> GetAllAsync()
    {
        return await _context.Reservations.ToListAsync();
    }

    /// <summary>
    /// Asynchronously adds a new Reservation entity.
    /// </summary>
    /// <param name="reservation">The new Reservation entity.</param>
    /// <returns>The added Reservation entity.</returns>
    public async Task<Reservation> AddAsync(Reservation reservation)
    {
        await _context.Reservations.AddAsync(reservation);
        await _context.SaveChangesAsync();
        return reservation;
    }

    /// <summary>
    /// Asynchronously updates an existing Reservation entity.
    /// </summary>
    /// <param name="reservation">The Reservation entity to update.</param>
    public async Task UpdateAsync(Reservation reservation)
    {
        _context.Entry(reservation).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Asynchronously deletes a Reservation entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation != null)
        {
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Asynchronously gets all Reservation entities for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A list of Reservation entities for the specified user.</returns>
    public async Task<IEnumerable<Reservation>> GetByUserIdAsync(int userId)
    {
        return await _context.Reservations
            .Where(r => r.UserId == userId)
            .ToListAsync();
    }

    /// <summary>
    /// Asynchronously gets available time slots for reservations within a date range.
    /// </summary>
    /// <param name="startDate">The start date of the range.</param>
    /// <param name="endDate">The end date of the range.</param>
    /// <returns>A list of available DateTime slots.</returns>
    public async Task<IEnumerable<DateTime>> GetAvailableSlotsAsync(DateTime startDate, DateTime endDate)
    {
        // Предполагаем, что ресторан работает с 11:00 до 22:00
        var openingTime = new TimeSpan(11, 0, 0);
        var closingTime = new TimeSpan(22, 0, 0);
        var slotDuration = TimeSpan.FromHours(1); // Предполагаем, что каждый слот длится 1 час

        var allPossibleSlots = new List<DateTime>();
        for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
        {
            var currentTime = openingTime;
            while (currentTime <= closingTime.Subtract(slotDuration))
            {
                allPossibleSlots.Add(date.Add(currentTime));
                currentTime = currentTime.Add(slotDuration);
            }
        }

        var bookedSlots = await _context.Reservations
              .Where(r => r.ReservationDate >= startDate && r.ReservationDate <= endDate)
              .Select(r => r.ReservationDate.Add(TimeSpan.Parse(r.ReservationTime)))
              .ToListAsync();

        return allPossibleSlots.Except(bookedSlots);
    }
}
