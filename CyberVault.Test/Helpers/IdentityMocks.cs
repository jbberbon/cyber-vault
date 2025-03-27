using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace CyberVault.Test.Helpers
{
    public static class IdentityMocks
    {
        public static UserManager<TUser> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var options = new Mock<IOptions<IdentityOptions>>();
            var idOptions = new IdentityOptions();
            idOptions.Lockout.AllowedForNewUsers = false;
            options.Setup(o => o.Value).Returns(idOptions);
            var userValidators = new List<IUserValidator<TUser>>();
            var passwordValidators = new List<IPasswordValidator<TUser>>();
            
            var userManager = new Mock<UserManager<TUser>>(
                store.Object,
                options.Object,
                new PasswordHasher<TUser>(),
                userValidators,
                passwordValidators,
                new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(),
                null,
                new Mock<ILogger<UserManager<TUser>>>().Object);

            // Setup common methods that might be used in tests
            userManager.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            
            userManager.Setup(x => x.UpdateAsync(It.IsAny<TUser>()))
                .ReturnsAsync(IdentityResult.Success);
                
            return userManager.Object;
        }

        public static SignInManager<TUser> MockSignInManager<TUser>(UserManager<TUser> userManager = null) where TUser : class
        {
            userManager ??= MockUserManager<TUser>();
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<TUser>>();
            var options = new Mock<IOptions<IdentityOptions>>();
            var logger = new Mock<ILogger<SignInManager<TUser>>>();
            var schemes = new Mock<IAuthenticationSchemeProvider>();
            var confirmation = new Mock<IUserConfirmation<TUser>>();
            
            var signInManager = new Mock<SignInManager<TUser>>(
                userManager,
                contextAccessor.Object,
                userPrincipalFactory.Object,
                options.Object,
                logger.Object,
                schemes.Object,
                confirmation.Object);
                
            // Setup common methods
            signInManager.Setup(x => x.PasswordSignInAsync(
                    It.IsAny<string>(), 
                    It.IsAny<string>(), 
                    It.IsAny<bool>(), 
                    It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);
                
            return signInManager.Object;
        }
    }
}