using AutoMapper;
using CyberVault.Server.DTO.User;
using CyberVault.Server.Miscs.Utilities.AuthHelpers;
using CyberVault.Server.Models;
using CyberVault.Server.Services.AuthService;
using CyberVault.Server.Services.DirectoryService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace CyberVault.Test.UnitTests.Services.AuthServiceTests;

public class RegisterTests
{
    // Not Mocked
    private AuthService _authService;

    // Mocked
    private Mock<SignInManager<User>> _signInManagerMock;
    private Mock<UserManager<User>> _userManager;
    private Mock<IMapper> _mapperMock;

    private readonly Mock<ILogger<AuthService>> _loggerMock;
    private readonly Mock<IAuthHelpers> _authHelpersMock;
    private readonly Mock<IDirectoryService> _directoryServiceMock;

    public RegisterTests()
    {
        // Setup mocks
        _loggerMock = new Mock<ILogger<AuthService>>();
        _authHelpersMock = new Mock<IAuthHelpers>(); 
        _mapperMock = new Mock<IMapper>();
    }

    private void SetupMocksForAuthService(string? email, IdentityResult createUserResult)
    {
        // 01. Mock UserManager
        _userManager = new Mock<UserManager<User>>(
            new Mock<IUserStore<User>>().Object,
            null, null, null, null, null, null, null, null
        );

        // 02. Handle user retrieval
        _userManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((string e) => email == null ? null : new User { Email = e });

        // 03. Setup Mapper Mock with Conditional Behavior
        _mapperMock.Setup(m => m.Map<User>(It.IsAny<UserForRegistrationDto>()))
            .Returns((UserForRegistrationDto dto) =>
            {
                // If email exists in mock, return null, else return a User object
                return email == null ? new User { Email = dto.Email } : null;
            });

        // 04. Handle user creation
        _userManager.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(createUserResult);

        // 05. Initialize SignInManager mock
        _signInManagerMock = new Mock<SignInManager<User>>(
            _userManager.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<User>>().Object,
            null, null, null, null
        );

        // 06. Initialize AuthService AFTER mocks are created
        _authService = new AuthService(
            _mapperMock.Object,
            _userManager.Object,
            _loggerMock.Object,
            _authHelpersMock.Object,
            _signInManagerMock.Object,
            _directoryServiceMock.Object
        );
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturn409Error_WhenEmailIsTaken()
    {
        // Arrange
        SetupMocksForAuthService("emailExists@email.com", IdentityResult.Failed()); // User already registered
        var request = new UserForRegistrationDto
        {
            Email = "emailExists@email.com",
            Password = "emailExists",
            ConfirmPassword = "emailExists"
        };

        // Act
        var result = await _authService.RegisterAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(CyberVault.Server.Miscs.Constants.ErrorCodes.AlreadyExists, result.ErrorCode);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturn500Error_WhenUserManagerCreateFails()
    {
        // Arrange
        SetupMocksForAuthService(null, IdentityResult.Failed()); // User not yet registered
        var request = new UserForRegistrationDto
        {
            Email = "newEmail@email.com",
            Password = "newEmail",
            ConfirmPassword = "newEmail"
        };

        // Act
        var result = await _authService.RegisterAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(CyberVault.Server.Miscs.Constants.ErrorCodes.InternalServerError, result.ErrorCode);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnSuccess_WhenValidRequest()
    {
        // Arrange
        SetupMocksForAuthService(null, IdentityResult.Success); // User not yet registered
        var request = new UserForRegistrationDto
        {
            Email = "newEmail@email.com",
            Password = "newEmail",
            ConfirmPassword = "newEmail"
        };

        // Act
        var result = await _authService.RegisterAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
    }
}