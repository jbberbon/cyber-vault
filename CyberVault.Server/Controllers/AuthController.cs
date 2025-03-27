using System.Security.Claims;
using CyberVault.Server.DTO.User;
using CyberVault.Server.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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

        // 03. Success
        if (response.IsSuccess) return StatusCode(201);

        // 04. Error
        return StatusCode(response.ErrorCode, new { errors = response.Errors });
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

        // 03. Success
        if (response.IsSuccess) return Ok(new { email = response.User!.Email });

        // 04. Error
        return response.Errors.Any()
            ? StatusCode(response.ErrorCode, new { errors = response.Errors })
            : StatusCode(response.ErrorCode);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        // 01. Retrieve and check user id if exists
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return Unauthorized();
        }

        // 02. Call register service
        var response = await _authService.LogoutAsync();

        // 03. Return
        return response.IsSuccess ? Ok() : StatusCode(response.ErrorCode);
    }

    [Authorize]
    [HttpGet("check")]
    public async Task<IActionResult> Check()
    {
        // 01. Retrieve and check user id if exists
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return Unauthorized();
        }

        // 02. Call Check Service
        var response = await _authService.CheckAsync(userId);

        // 03. Success
        if (response.IsSuccess) return Ok();

        // 04. Error
        return response.Errors.Any()
            ? StatusCode(response.ErrorCode, new { errors = response.Errors })
            : StatusCode(response.ErrorCode);
    }
}