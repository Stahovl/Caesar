using Caesar.Core.Entities;
using Caesar.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Caesar.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController: ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginModel model)
    {
        // Здесь должна быть проверка учетных данных
        var user = new User { Username = model.Username, Role = "User" };
        var token = _authService.GenerateJwtToken(user);
        return Ok(new { token });
    }
}

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}

