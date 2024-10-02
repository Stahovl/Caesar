﻿using Caesar.Core.DTOs;
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
            UserId = reservationDto.UserId,
            ReservationDate = reservationDto.ReservationDate,
            ReservationTime = reservationDto.ReservationTime,
            NumberOfGuests = reservationDto.NumberOfGuests
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

    public async Task<ReservationDto> GetReservationByIdAsync(int id)
    {
        var reservation = await _repository.GetByIdAsync(id);
        return reservation != null ? MapToDto(reservation) : null;
    }

    public async Task<IEnumerable<ReservationDto>> GetReservationsByUserIdAsync(string userId)
    {
        var reservations = await _repository.GetByUserIdAsync(userId);
        return reservations.Select(MapToDto);
    }

    public async Task UpdateReservationAsync(ReservationDto reservationDto)
    {
        var reservation = new Reservation
        {
            Id = reservationDto.Id,
            UserId = reservationDto.UserId,
            ReservationDate = reservationDto.ReservationDate,
            ReservationTime = reservationDto.ReservationTime,
            NumberOfGuests = reservationDto.NumberOfGuests
        };
        await _repository.UpdateAsync(reservation);
    }

    public async Task<IEnumerable<DateTime>> GetAvailableSlotsAsync(DateTime startDate, DateTime endDate)
    {
        // Реализация этого метода зависит от вашей бизнес-логики
        // Здесь должна быть логика для определения доступных слотов
        throw new NotImplementedException();
    }

    private static ReservationDto MapToDto(Reservation reservation)
    {
        return new ReservationDto
        {
            Id = reservation.Id,
            UserId = reservation.UserId,
            ReservationDate = reservation.ReservationDate,
            ReservationTime = reservation.ReservationTime,
            NumberOfGuests = reservation.NumberOfGuests
        };
    }
}
