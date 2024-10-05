using Caesar.Core.Entities;
using Caesar.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Caesar.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController: ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        _logger.LogInformation($"Login attempt for user: {model.Username}");
        try
        {
            var result = await _authService.LoginAsync(model.Username, model.Password);
            if (result.IsSuccess)
            {
                _logger.LogInformation($"Login successful for user: {model.Username}");
                return Ok(new { token = result.Token, userId = result.UserId });
            }
            _logger.LogWarning($"Login failed for user: {model.Username}");
            return Unauthorized(new { message = "Invalid username or password" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred during login for user: {model.Username}");
            return StatusCode(500, new { message = "An error occurred during login" });
        }
    }
}

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}

