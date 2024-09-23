using Caesar.Core.Interfaces;
using Caesar.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Caesar.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMenuItemService _menuItemService;

    public HomeController(IMenuItemService menuItemService)
    {
        _menuItemService = menuItemService;
    }

    public async Task<IActionResult> Index()
    {
        var menuItems = await _menuItemService.GetAllMenuItemsAsync();
        return View(menuItems);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
