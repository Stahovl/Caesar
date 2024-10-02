﻿using Caesar.Core.DTOs;

namespace Caesar.Core.Interfaces;

public interface IReservationService
{
    public Task<ReservationDto> GetReservationByIdAsync(int id);
    public Task<IEnumerable<ReservationDto>> GetAllReservationsAsync();
    public Task<ReservationDto> CreateReservationAsync(ReservationDto reservationDto);
    public Task UpdateReservationAsync(ReservationDto reservationDto);
    public Task DeleteReservationAsync(int id);
    public Task<IEnumerable<ReservationDto>> GetReservationsByUserIdAsync(string userId);
    public Task<IEnumerable<DateTime>> GetAvailableSlotsAsync(DateTime startDate, DateTime endDate);
}
