using AutoMapper;
using CyberVault.Server.DTO.User;
using CyberVault.Server.Miscs.Constants;
using CyberVault.Server.Miscs.Utilities.AuthHelpers;
using CyberVault.Server.Models;
using CyberVault.Server.Services.DirectoryService;
using Microsoft.AspNetCore.Identity;

namespace CyberVault.Server.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<AuthService> _logger;
    private readonly IAuthHelpers _authHelpers;
    private readonly SignInManager<User> _signInManager;
    private readonly IDirectoryService _directoryService;

    public AuthService(
        IMapper mapper,
        UserManager<User> userManager,
        ILogger<AuthService> logger,
        IAuthHelpers authHelpers,
        SignInManager<User> signInManager,
        IDirectoryService directoryService
    )
    {
        _mapper = mapper;
        _userManager = userManager;
        _logger = logger;
        _authHelpers = authHelpers;
        _signInManager = signInManager;
        _directoryService = directoryService;
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
            var errors = result.Errors.Select(e => e.Description).ToList();

            response.IsSuccess = false;
            response.ErrorCode = ErrorCodes.InternalServerError;
            response.Errors = errors;

            // Log the error
            _logger.LogError("Something went wrong while creating user. Errors: {Errors}", string.Join(", ", errors));

            return response;
        }
        
        // 03. Create root directory for the user
        var newUser = await _userManager.FindByEmailAsync(request.Email);
        var newDirectoryResult = await _directoryService.CreateAsync(newUser!.Id);
        if (!newDirectoryResult.IsSuccess)
        {
            response.IsSuccess = false;
            response.ErrorCode = newDirectoryResult.ErrorCode;
            response.Errors = newDirectoryResult.Errors;
            
            // Log the error
            _logger.LogError("Something went wrong while creating user. Errors: {Errors}", string.Join(", ", newDirectoryResult.Errors));
            
            return response;
        }

        // 04. Success
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