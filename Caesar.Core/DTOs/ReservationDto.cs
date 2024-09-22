namespace Caesar.Core.DTOs;

public class ReservationDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public int NumberOfGuests { get; set; }
    public string CustomerName { get; set; }
    public string ContactNumber { get; set; }
}
