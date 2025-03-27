using AutoMapper;
using CyberVault.Server.DTO.User;
using CyberVault.Server.Miscs.Constants;
using CyberVault.Server.Miscs.Utilities.AuthHelpers;
using CyberVault.Server.Models;
using Microsoft.AspNetCore.Identity;

namespace CyberVault.Server.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<AuthService> _logger;
    private readonly IAuthHelpers _authHelpers;
    private readonly SignInManager<User> _signInManager;

    public AuthService(
        IMapper mapper,
        UserManager<User> userManager,
        ILogger<AuthService> logger,
        IAuthHelpers authHelpers,
        SignInManager<User> signInManager
    )
    {
        _mapper = mapper;
        _userManager = userManager;
        _logger = logger;
        _authHelpers = authHelpers;
        _signInManager = signInManager;
    }

    public async Task<AuthServiceResponseDto> RegisterAsync(UserForRegistrationDto request)
    {
        var response = new AuthServiceResponseDto();
        // 01. Check if email is taken
        if (await _userManager.FindByEmailAsync(request.Email) is not null)
        {
            response.IsSuccess = false;
            response.ErrorCode = ErrorCodes.AlreadyExists;
            response.Errors = [_authHelpers.GetAuthErrorMessage(AuthErrorsEnum.EmailAlreadyTaken)];
            return response;
        }

        // 02. Create user
        var user = _mapper.Map<User>(request);
        user.EmailConfirmed = true;
        user.CreatedAt = DateTime.Now;
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            // Retrieve errors from result
            var errors = result.Errors.Select(e => e.Description).ToList();

            response.IsSuccess = false;
            response.ErrorCode = ErrorCodes.InternalServerError;
            response.Errors = errors;

            // 03.1. Log the error
            _logger.LogError("Something went wrong while creating user. Errors: {Errors}", string.Join(", ", errors));

            return response;
        }

        // 03. Success
        response.IsSuccess = true;
        return response;
    }

    public async Task<AuthServiceResponseDto> LoginAsync(UserForLoginDto request)
    {
        var response = new AuthServiceResponseDto();
        // 01. Find user by email
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            response.IsSuccess = false;
            response.ErrorCode = ErrorCodes.Unauthorized;
            return response;
        }

        // 02. Login
        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);
        if (!result.Succeeded)
        {
            // 02.01 Email not yet confirmed
            if (result.IsNotAllowed)
            {
                response.IsSuccess = false;
                response.ErrorCode = ErrorCodes.Forbidden;
                response.Errors = [_authHelpers.GetAuthErrorMessage(AuthErrorsEnum.EmailNotConfirmed)];
                return response;
            }

            // 02.02 Account locked out
            if (result.IsLockedOut)
            {
                response.IsSuccess = false;
                response.ErrorCode = ErrorCodes.AccLockedOut;
                response.Errors = [_authHelpers.GetAuthErrorMessage(AuthErrorsEnum.AccountLockedOut)];
                return response;
            }

            // 02.03 Invalid credentials
            response.IsSuccess = false;
            response.ErrorCode = ErrorCodes.Unauthorized;
            return response;
        }

        // 03. Success
        response.IsSuccess = true;
        response.User = user;
        return response;
    }

    public async Task<AuthServiceResponseDto> LogoutAsync()
    {
        var response = new AuthServiceResponseDto();
        try
        {
            await _signInManager.SignOutAsync();
            response.IsSuccess = true;
            return response;
        }
        catch
        {
            response.IsSuccess = false;
            response.ErrorCode = ErrorCodes.InternalServerError;
            return response;
        }
    }

    public async Task<AuthServiceResponseDto> CheckAsync(string userId)
    {
        var response = new AuthServiceResponseDto();

        var user = await _userManager.FindByIdAsync(userId);

        // 01. Check if user still exists
        if (user == null)
        {
            response.IsSuccess = false;
            response.ErrorCode = ErrorCodes.NotFound;
            return response;
        }

        // 02. Check if email is confirmed
        if (!user.EmailConfirmed)
        {
            response.IsSuccess = false;
            response.ErrorCode = ErrorCodes.Forbidden;
            response.Errors = [_authHelpers.GetAuthErrorMessage(AuthErrorsEnum.EmailNotConfirmed)];
            return response;
        }

        // 03. Check if user is locked out
        if (user.LockoutEnd is not null)
        {
            response.IsSuccess = false;
            response.ErrorCode = ErrorCodes.AccLockedOut;
            response.Errors = [_authHelpers.GetAuthErrorMessage(AuthErrorsEnum.AccountLockedOut)];
            return response;
        }

        // 04. Return
        response.IsSuccess = true;
        return response;
    }
}