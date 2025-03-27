namespace CyberVault.Server.DTO.User;

public class AuthServiceResponseDto
{
    public bool IsSuccess { get; set; }

    public IEnumerable<string> Errors { get; set; } = [];

    public int ErrorCode { get; set; } = 0;
    public Models.User? User { get; set; }
}