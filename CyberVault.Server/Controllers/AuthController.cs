using CyberVault.Server.DTO.User;
using CyberVault.Server.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CyberVault.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(
        IAuthService authService
    )
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserForRegistrationDto? request)
    {
        // 01. Validate Request
        if (request is null)
        {
            return BadRequest(new { errors = "Request body is required" });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // 02. Call register service
        var response = await _authService.RegisterAsync(request);

        // 03. Check if there are errors
        if (response.IsSuccess == false)
        {
            return BadRequest(new { errors = response.Errors });
        }

        // 05. Success
        return StatusCode(201);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] UserForLoginDto? request)
    {
        // 01. Validate Request
        if (request is null)
        {
            return BadRequest(new { errors = "Request body is required" });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // 02. Call login service
        var response = await _authService.LoginAsync(request);

        // 03. Check if there are errors
        if (response.IsSuccess == false)
        {
            return response.Errors.Any()
                ? StatusCode(response.ErrorCode, new { errors = response.Errors })
                : StatusCode(response.ErrorCode);
        }

        // 04. Success
        return Ok(new { email = response.User!.Email });
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        // 01. Call register service
        var response = await _authService.LogoutAsync();
        if (!response.IsSuccess)
        {
            return StatusCode(response.ErrorCode);
        }

        // 02. Success
        return Ok();
    }
}