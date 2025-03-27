using AutoMapper;
using CyberVault.Server.DTO.User;
using CyberVault.Server.Miscs.Constants;
using CyberVault.Server.Miscs.Utilities.AuthHelpers;
using CyberVault.Server.Models;
using CyberVault.Server.Services.AuthService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace CyberVault.Test.UnitTests.Services.AuthServiceTests;

public class LoginServiceTests
{
    // Not Mocked
    private AuthService _authService;

    // Mocked
    private Mock<UserManager<User>> _userMock;
    private Mock<SignInManager<User>> _signInManagerMock;
    private readonly Mock<ILogger<AuthService>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IAuthHelpers> _authHelpersMock;
    
    public LoginServiceTests()
    {
        _loggerMock = new Mock<ILogger<AuthService>>();
        _mapperMock = new Mock<IMapper>();
        _authHelpersMock = new Mock<IAuthHelpers>();
    }
    
    private void SetupMocksForAuthService(string? email, SignInResult signInResult)
    {
        // 01. Mock UserManager
        _userMock = new Mock<UserManager<User>>(
            new Mock<IUserStore<User>>().Object,
            null, null, null, null, null, null, null, null
        );
        
        // 02. Handle user retrieval
        _userMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((string e) => email == null ? null : new User { Email = e });

        // 03. Mock SignInManager
        _signInManagerMock = new Mock<SignInManager<User>>(
            _userMock.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<User>>().Object,
            null, null, null, null
        );
        
        // 04. Handle user login
        _signInManagerMock.Setup(sm =>
                sm.PasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(signInResult);

        // 05. Initialize AuthService AFTER mocks are created
        _authService = new AuthService(
            _mapperMock.Object,
            _userMock.Object,
            _loggerMock.Object,
            _authHelpersMock.Object,
            _signInManagerMock.Object
        );
    }

    [Fact]
    public async Task LoginAsync_ShouldReturn401Error_WhenWrongCredentials()
    {
        // Arrange
        SetupMocksForAuthService("test1@example.com", SignInResult.Failed); // Invalid password scenario

        var request = new UserForLoginDto
        {
            Email = "test1@example.com",
            Password = "wrongPassword"
        };

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorCodes.Unauthorized, result.ErrorCode);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnForbidden_WhenEmailNotConfirmed()
    {
        // Arrange
        SetupMocksForAuthService("test1@example.com", SignInResult.NotAllowed); // Email not confirmed scenario

        var request = new UserForLoginDto
        {
            Email = "test1@example.com",
            Password = "correctPassword"
        };

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorCodes.Forbidden, result.ErrorCode);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnSuccess_WhenValidCredentials()
    {
        // Arrange
        SetupMocksForAuthService("test1@example.com", SignInResult.Success); // Valid login scenario

        var request = new UserForLoginDto
        {
            Email = "test1@example.com",
            Password = "correctPassword"
        };

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("test1@example.com", result.User!.Email);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturn401_WhenUserNotFound()
    {
        // Arrange
        SetupMocksForAuthService(null, SignInResult.Failed); // Valid login scenario

        var request = new UserForLoginDto
        {
            Email = "notFoundEmail@example.com",
            Password = "notFoundUser"
        };

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorCodes.Unauthorized, result.ErrorCode);
    }
}