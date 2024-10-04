﻿namespace Caesar.Core.Entities;

public class Reservation
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public DateTime ReservationDate { get; set; }
    public string ReservationTime { get; set; }
    public int NumberOfGuests { get; set; }
}