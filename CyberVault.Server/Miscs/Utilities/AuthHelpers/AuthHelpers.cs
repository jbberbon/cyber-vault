using CyberVault.Server.Miscs.Constants;

namespace CyberVault.Server.Miscs.Utilities.AuthHelpers;

public class AuthHelpers : IAuthHelpers
{
    // Method to return error messages based on the error enum
    public string GetAuthErrorMessage(AuthErrorsEnum? error)
    {
        return error switch
        {
            AuthErrorsEnum.CredentialsRequired => "Credentials required.",
            AuthErrorsEnum.InvalidCredentials => "The credentials provided are invalid.",
            AuthErrorsEnum.EmailAlreadyTaken => "The email is already taken.",
            AuthErrorsEnum.WeakPassword => "The password is too weak.",
            AuthErrorsEnum.UserNotFound => "User not found.",
            AuthErrorsEnum.EmailNotConfirmed => "Login not allowed. Please confirm your email first.",
            AuthErrorsEnum.AccountLockedOut => "Account locked due to many failed login attempts.",
            _ => "An unknown error occurred."
        };
    }
}