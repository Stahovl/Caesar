using Caesar.Core.Interfaces;
using Caesar.Core.Services;
using Caesar.Web.Extensions;
using Caesar.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Caesar.Web.Controllers;

public class MenuController : Controller
{
    private readonly IMenuItemService _menuItemService;

    public MenuController(IMenuItemService menuItemService)
    {
        _menuItemService = menuItemService;
    }

    // Display menu items
    public async Task<IActionResult> Index()
    {
        // Fetch menu items as DTOs from the service
        var menuItemsDto = await _menuItemService.GetAllMenuItemsAsync();

        // Map DTOs to ViewModels
        var viewModel = menuItemsDto.Select(item => new MenuItemViewModel
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Price = item.Price,
            ImageUrl = item.ImageUrl
        }).ToList();

        return View(viewModel);  // Pass the ViewModel to the view
    }

    // Add an item to the cart
    [HttpPost]
    public IActionResult AddToCart(int itemId)
    {
        var cart = HttpContext.Session.Get<List<int>>("Cart") ?? new List<int>();
        cart.Add(itemId);
        HttpContext.Session.Set("Cart", cart);

        return RedirectToAction("Cart");
    }

    [HttpPost]
    public IActionResult ClearCart()
    {
        HttpContext.Session.Remove("Cart");
        return RedirectToAction("Index");
    }

    // View the cart
    public async Task<IActionResult> Cart()
    {
        var cart = HttpContext.Session.Get<List<int>>("Cart") ?? new List<int>();
        var menuItemsDto = await _menuItemService.GetMenuItemsByIdsAsync(cart);

        // Map DTOs to ViewModels
        var viewModel = menuItemsDto.Select(item => new MenuItemViewModel
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Price = item.Price,
            ImageUrl = item.ImageUrl
        }).ToList();

        return View(viewModel);
    }
}
