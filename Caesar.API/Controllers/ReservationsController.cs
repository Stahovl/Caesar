using Caesar.Core.DTOs;
using Caesar.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Caesar.API.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationsController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> GetReservations()
    {
        var reservations = await _reservationService.GetAllReservationsAsync();
        return Ok(reservations);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReservationDto>> GetReservation(int id)
    {
        var reservation = await _reservationService.GetReservationByIdAsync(id);
        if (reservation == null)
        {
            return NotFound();
        }
        return Ok(reservation);
    }

    [HttpPost]
    public async Task<ActionResult<ReservationDto>> CreateReservation(ReservationDto reservationDto)
    {
        var createdReservation = await _reservationService.CreateReservationAsync(reservationDto);
        return CreatedAtAction(nameof(GetReservation), new { id = createdReservation.Id }, createdReservation);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReservation(int id, ReservationDto reservationDto)
    {
        if (id != reservationDto.Id)
        {
            return BadRequest();
        }

        await _reservationService.UpdateReservationAsync(reservationDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReservation(int id)
    {
        await _reservationService.DeleteReservationAsync(id);
        return NoContent();
    }
}