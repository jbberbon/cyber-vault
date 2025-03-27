using System.ComponentModel.DataAnnotations;
using AutoMapper;
using CyberVault.Server.Controllers;
using CyberVault.Server.Data;
using CyberVault.Server.DTO.User;
using CyberVault.Server.Miscs.Constants;
using CyberVault.Server.Miscs.Utilities.AuthHelpers;
using CyberVault.Server.Models;
using CyberVault.Server.Services.AuthService;
using CyberVault.Test.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace CyberVault.Test.UnitTests.Controllers.AuthControllerTests;

public class LoginTests
{
    // Not Mocked
    private readonly AuthController _controller;

    // Mocked
    private readonly Mock<IAuthService> _authServiceMock;

    public LoginTests()
    {
        // Setup mocks
        _authServiceMock = new Mock<IAuthService>();

        // Instantiate AuthController
        _controller = new AuthController(_authServiceMock.Object);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData("jbberbon", "sfsfs")]
    public async Task LoginAsync_ShouldReturn400Error_WhenRequestIsInvalid(string? email, string? password)
    {
        // 01. Arrange
        UserForLoginDto request = email is null && password is null
            ? null
            : new UserForLoginDto
            {
                Email = email,
                Password = password
            };

        // Trigger ModelState Validation
        if (request != null)
        {
            var validationContext = new ValidationContext(request);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(request, validationContext, validationResults, true);

            // Add validation errors to ModelState
            foreach (var validationResult in validationResults)
            {
                foreach (var memberName in validationResult.MemberNames)
                {
                    _controller.ModelState.AddModelError(memberName, validationResult.ErrorMessage!);
                }
            }
        }

        // 02. Act
        var result = await _controller.Login(request);

        // 03. Assert
        var badRequestResult =
            Assert.IsType<BadRequestObjectResult>(result); // Check that the result is a BadRequestObjectResult
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Theory]
    [InlineData("test@gmail.com", "password")]
    [InlineData("test2@gmail.com", "password")]
    public async Task LoginAsync_ShouldReturnOk_WhenRequestIsValid(string email, string password)
    {
        // 01. Arrange
        // 01.01 Mock the behavior of LoginAsync in AuthService to return a successful response
        AuthServiceResponseDto authServiceRes = new AuthServiceResponseDto
        {
            IsSuccess = true,
            User = new User { Email = email }
        };
        _authServiceMock.Setup(x => x.LoginAsync(It.IsAny<UserForLoginDto>()))
            .ReturnsAsync(authServiceRes);

        // 01.02 Prepare the controller argument
        UserForLoginDto request = new UserForLoginDto
        {
            Email = email,
            Password = password
        };

        // 01.03 Trigger ModelState Validation
        var validationContext = new ValidationContext(request);
        var validationResults = new List<ValidationResult>();
        Validator.TryValidateObject(request, validationContext, validationResults, true);

        // Add validation errors to ModelState
        foreach (var validationResult in validationResults)
        {
            foreach (var memberName in validationResult.MemberNames)
            {
                _controller.ModelState.AddModelError(memberName, validationResult.ErrorMessage!);
            }
        }


        // 02. Act
        var result = await _controller.Login(request);

        // 03. Assert
        var okRequestResult =
            Assert.IsType<OkObjectResult>(result); // Check that the result is a BadRequestObjectResult
        Assert.Equal(200, okRequestResult.StatusCode);
    }

    /*public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }*/
}