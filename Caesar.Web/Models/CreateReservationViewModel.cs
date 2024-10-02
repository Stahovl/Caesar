using Caesar.Core.Entities;

namespace Caesar.Web.Models;

public class CreateReservationViewModel
{
    public IEnumerable<DateTime> AvailableSlots { get; set; }
    public List<MenuItem> CartItems { get; set; }
    public DateTime SelectedDate { get; set; }
    public TimeSpan SelectedTime { get; set; }
    public int NumberOfGuests { get; set; }
}
