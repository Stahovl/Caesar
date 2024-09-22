using Caesar.Core.DTOs;
using Caesar.Core.Entities;
using Caesar.Core.Interfaces;
using System.Linq;

namespace Caesar.Core.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _repository;

    public ReservationService(IReservationRepository repository)
    {
        _repository = repository;
    }

    public async Task<ReservationDto> CreateReservationAsync(ReservationDto reservationDto)
    {
        var reservation = new Reservation
        {
            Date = reservationDto.Date,
            Time = reservationDto.Time,
            NumberOfGuests = reservationDto.NumberOfGuests,
            CustomerName = reservationDto.CustomerName,
            ContactNumber = reservationDto.ContactNumber
        };

        var createdReservation = await _repository.AddAsync(reservation);
        return MapToDto(createdReservation);
    }

    public async Task DeleteReservationAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<ReservationDto>> GetAllReservationsAsync()
    {
        var reservations = await _repository.GetAllAsync();
        return reservations.Select(MapToDto);
    }

    public async Task<ReservationDto> GetReservationAsync(int id)
    {
        var reservation = await _repository.GetByIdAsync(id);
        return reservation != null ? MapToDto(reservation) : null;
    }

    public async Task UpdateReservationAsync(ReservationDto reservationDto)
    {
        var reservation = new Reservation
        {
            Id = reservationDto.Id,
            Date = reservationDto.Date,
            Time = reservationDto.Time,
            NumberOfGuests = reservationDto.NumberOfGuests,
            CustomerName = reservationDto.CustomerName,
            ContactNumber = reservationDto.ContactNumber
        };

        await _repository.UpdateAsync(reservation);
    }

    private static ReservationDto MapToDto(Reservation reservation)
    {
        return new ReservationDto
        {
            Id = reservation.Id,
            Date = reservation.Date,
            Time = reservation.Time,
            NumberOfGuests = reservation.NumberOfGuests,
            CustomerName = reservation.CustomerName,
            ContactNumber = reservation.ContactNumber
        };
    }
}
