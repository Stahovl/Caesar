using Caesar.Core.DTOs;

namespace Caesar.Web.Models;

public class CreateReservationRequest
{
    public ReservationDto ReservationDto { get; set; }
    public List<int> MenuItemIds { get; set; }
}
