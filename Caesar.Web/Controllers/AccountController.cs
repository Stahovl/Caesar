using Caesar.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Caesar.Core.Entities;
using Caesar.Web.Models;

namespace Caesar.Web.Controllers;

public class AccountController : Controller
{
    private readonly CaesarDbContext _context;

    public AccountController(CaesarDbContext context)
    {
        _context = context;
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
            if (await _context.Users.AnyAsync(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Username is already taken.");
                return View(model);
            }

            var user = new User
            {
                Username = model.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Role = "Customer"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await LoginUser(user);

            return RedirectToAction("Index", "Home");
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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);

            if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                await LoginUser(user);
                return RedirectToAction("Cart", "Menu");
            }

            ModelState.AddModelError("", "Invalid username or password");
        }

        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    private async Task LoginUser(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }
}