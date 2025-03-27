using CyberVault.Server.DTO.User;
using Microsoft.AspNetCore.Mvc;

namespace CyberVault.Server.Services.AuthService;

public interface IAuthService
{
    Task<AuthServiceResponseDto> RegisterAsync(UserForRegistrationDto request);
    Task<AuthServiceResponseDto> LoginAsync(UserForLoginDto request);
    Task<AuthServiceResponseDto> LogoutAsync();
}