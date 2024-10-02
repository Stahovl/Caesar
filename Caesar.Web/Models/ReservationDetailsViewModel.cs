using Caesar.Core.DTOs;

namespace Caesar.Web.Models;

public class ReservationDetailsViewModel
{
    public ReservationDto Reservation { get; set; }
    public OrderDto Order { get; set; }
}
