using Caesar.Core.DTOs;
using Caesar.Core.Interfaces;
using Caesar.Web.Extensions;
using Caesar.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace Caesar.Web.Controllers;

[Authorize]
public class ReservationController : Controller
{
    private readonly IReservationService _reservationService;
    private readonly IOrderService _orderService;
    private readonly IMenuItemService _menuItemService;

    private readonly HttpClient _httpClient;

    public ReservationController(IReservationService reservationService, IOrderService orderService,
        IMenuItemService menuItemService, HttpClient httpClient)
    {
        _reservationService = reservationService;
        _orderService = orderService;
        _menuItemService = menuItemService;

        _httpClient = httpClient;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var reservations = await _reservationService.GetReservationsByUserIdAsync(userId);
        return View(reservations);
    }

    public async Task<IActionResult> Create()
    {
        try
        {
            var startDate = DateTime.Today;
            var endDate = startDate.AddDays(14);
            var availableSlots = await _reservationService.GetAvailableSlotsAsync(startDate, endDate);
            var cart = HttpContext.Session.Get<List<int>>("Cart") ?? new List<int>();
            var menuItems = await _menuItemService.GetMenuItemsByIdsAsync(cart);

            var viewModel = new CreateReservationViewModel
            {
                AvailableSlots = availableSlots.ToList(),
                CartItems = menuItems,
                SelectedDate = startDate
            };
            return View(viewModel);
        }
        catch (Exception ex)
        {
            // Логирование ошибки
            Console.WriteLine($"Error in Create method: {ex.Message}");
            // Можно также добавить внутренние исключения
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }
            // Возвращаем ошибку или перенаправляем на страницу ошибки
            return View("Error");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateReservationViewModel viewModel)
    {
        Console.WriteLine("Create post request with viewmodel start");

        if (!ModelState.IsValid)
        {
            Console.WriteLine("Model state not valid");

            viewModel.AvailableSlots = (await _reservationService
                .GetAvailableSlotsAsync(DateTime.Today, DateTime.Today.AddDays(14))).ToList();

            viewModel.CartItems = await _menuItemService
                .GetMenuItemsByIdsAsync(HttpContext.Session.Get<List<int>>("Cart") ?? new List<int>());
           
            return View(viewModel);
        }

        if (ModelState.IsValid)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = HttpContext.Session.Get<List<int>>("Cart") ?? new List<int>();

            // Create object to send to API
            var request = new
            {
                reservationDto = new ReservationDto
                {
                    UserId = userId,
                    ReservationDate = viewModel.SelectedDate,
                    ReservationTime = viewModel.SelectedTime,
                    NumberOfGuests = viewModel.NumberOfGuests
                },
                menuItemIds = cart
            };

            Console.WriteLine("ModelState is valid");

            try
            {
                // Convert object to JSON
                Console.WriteLine("Start convert request to json");
                var json = JsonConvert.SerializeObject(request);
                Console.WriteLine("get content from json"+json);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send POST request to API
                Console.WriteLine("Start sending request");
                var response = await _httpClient.PostAsync("https://localhost:7114/api/Reservations", content);
                Console.WriteLine("Request sent, awaiting response");

                if (response.IsSuccessStatusCode)
                {
                    // Read response content as string and deserialize to ReservationDto
                    Console.WriteLine($"Response status code: {response.StatusCode}");
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response content: {responseContent}");

                    var createdReservation = JsonConvert.DeserializeObject<ReservationDto>(responseContent);

                    // Clear cart after successful reservation
                    HttpContext.Session.Remove("Cart");

                    // Redirect to the details page for the newly created reservation
                    return RedirectToAction(nameof(Details), new { id = createdReservation.Id });
                }
                else
                {
                    ModelState.AddModelError("", "An error occurred while creating the reservation. Please try again.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the reservation. Please try again.");
                Console.WriteLine("Error " + ex.Message);
                // Log error if necessary
            }
        }

        // Show form again if ModelState is invalid or if there's an error
        viewModel.AvailableSlots = (await _reservationService.GetAvailableSlotsAsync(DateTime.Today, DateTime.Today.AddDays(14))).ToList();
        viewModel.CartItems = await _menuItemService.GetMenuItemsByIdsAsync(HttpContext.Session.Get<List<int>>("Cart") ?? new List<int>());
       
        Console.WriteLine("Viewmodel selected time before return = " + viewModel.SelectedTime);
        
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
