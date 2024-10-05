using Microsoft.AspNetCore.Mvc;
using Caesar.Web.Models;
using Caesar.Web.Intrefaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Caesar.Web.Controllers;

public class AccountController : Controller
{
    private readonly IApiService _apiService;
    private readonly ITokenService _tokenService;

    public AccountController(IApiService apiService, ITokenService tokenService)
    {
        _apiService = apiService;
        _tokenService = tokenService;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _apiService.RegisterAsync(model);
            if (result)
            {
                // Login after register
                var loginResult = await _apiService.LoginAsync(model.Username, model.Password);
                if (loginResult.IsSuccess)
                {
                    await _tokenService.SetTokenAsync(loginResult.Token);
                    return RedirectToAction("Index", "Menu");
                }
            }
            ModelState.AddModelError("", "Registration failed. Please try again.");
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _apiService.LoginAsync(model.Username, model.Password);
            if (result.IsSuccess)
            {
                await _tokenService.SetTokenAsync(result.Token);

                // Create the authentication cookie
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username),
            };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Cart", "Menu");
            }
            ModelState.AddModelError("", "Invalid username or password");
        }
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        Console.WriteLine("Logout start");
        await _tokenService.ClearTokenAsync();
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Menu");
    }
}