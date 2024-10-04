using Caesar.Core.Entities;

namespace Caesar.Web.Models;

public class CreateReservationViewModel
{
    public DateTime SelectedDate { get; set; }
    public string SelectedTime { get; set; }
    public int NumberOfGuests { get; set; }
    public List<DateTime> AvailableSlots { get; set; }
    public List<MenuItem> CartItems { get; set; }
}
