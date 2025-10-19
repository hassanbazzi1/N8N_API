namespace Public.DTO.Credentials.Google;

public sealed class GoogleCredentialResponse
{
    public required string CredentialId { get; init; }
    public required string AuthUrl      { get; init; }
    public bool IsAuthorised            { get; init; } = false;
} 