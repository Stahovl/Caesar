﻿namespace Caesar.Core.DTOs;

public class ReservationDto
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public DateTime ReservationDate { get; set; }
    public string ReservationTime { get; set; }
    public int NumberOfGuests { get; set; }
}
