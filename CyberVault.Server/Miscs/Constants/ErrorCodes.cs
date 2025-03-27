namespace CyberVault.Server.Miscs.Constants;

public static class ErrorCodes
{
    public const int BadRequest = 400;
    public const int Unauthorized = 401;
    public const int NotFound = 404;
    public const int InternalServerError = 500;
    public const int ValidationError = 422;
    public const int AlreadyExists = 409;
    public const int Forbidden = 403;
    public const int NoContent = 204;
    public const int TooManyAttempts = 429;
}