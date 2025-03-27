using CyberVault.Server.Miscs.Constants;

namespace CyberVault.Server.Miscs.Utilities.AuthHelpers;

public interface IAuthHelpers
{
    string GetAuthErrorMessage(AuthErrorsEnum? error);
}