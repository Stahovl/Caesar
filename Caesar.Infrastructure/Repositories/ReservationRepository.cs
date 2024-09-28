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
}
