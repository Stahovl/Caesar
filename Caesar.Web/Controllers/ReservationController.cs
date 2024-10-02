using Caesar.Core.DTOs;
using Caesar.Core.Entities;
using Caesar.Core.Interfaces;
using Caesar.Core.Services;
using Caesar.Infrastructure.Data;
using Caesar.Web.Extensions;
using Caesar.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Caesar.Web.Controllers;

[Authorize]
public class ReservationController : Controller
{
    private readonly IReservationService _reservationService;
    private readonly IOrderService _orderService;
    private readonly IMenuItemService _menuItemService;

    public ReservationController(IReservationService reservationService, IOrderService orderService, IMenuItemService menuItemService)
    {
        _reservationService = reservationService;
        _orderService = orderService;
        _menuItemService = menuItemService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var reservations = await _reservationService.GetReservationsByUserIdAsync(userId);
        return View(reservations);
    }

    public async Task<IActionResult> Create()
    {
        var availableSlots = await _reservationService.GetAvailableSlotsAsync(DateTime.Today, DateTime.Today.AddDays(14));
        var cart = HttpContext.Session.Get<List<int>>("Cart") ?? new List<int>();
        var menuItems = await _menuItemService.GetMenuItemsByIdsAsync(cart);
        var viewModel = new CreateReservationViewModel
        {
            AvailableSlots = availableSlots,
            CartItems = menuItems
        };
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateReservationViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = HttpContext.Session.Get<List<int>>("Cart") ?? new List<int>();
            var reservationDto = new ReservationDto
            {
                UserId = userId,
                ReservationDate = viewModel.SelectedDate,
                ReservationTime = viewModel.SelectedTime,
                NumberOfGuests = viewModel.NumberOfGuests
            };
            try
            {
                var createdReservation = await _reservationService.CreateReservationAsync(reservationDto);
                await _orderService.CreateOrderForReservationAsync(createdReservation.Id, cart);
                HttpContext.Session.Remove("Cart");
                return RedirectToAction(nameof(Details), new { id = createdReservation.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the reservation. Please try again.");
                // Log the exception
            }
        }

        // If we got this far, something failed; redisplay form
        viewModel.AvailableSlots = await _reservationService.GetAvailableSlotsAsync(DateTime.Today, DateTime.Today.AddDays(14));
        viewModel.CartItems = await _menuItemService.GetMenuItemsByIdsAsync(HttpContext.Session.Get<List<int>>("Cart") ?? new List<int>());
        return View(viewModel);
    }

    public async Task<IActionResult> Details(int id)
    {
        var reservation = await _reservationService.GetReservationByIdAsync(id);
        if (reservation == null)
        {
            return NotFound();
        }

        var order = await _orderService.GetOrderByReservationIdAsync(id);
        var viewModel = new ReservationDetailsViewModel
        {
            Reservation = reservation,
            Order = order
        };
        return View(viewModel);
    }
}
