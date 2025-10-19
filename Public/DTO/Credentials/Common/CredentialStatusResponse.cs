namespace Public.DTO.Credentials.Common;

public sealed class CredentialStatusResponse
{
    public required string         CredentialId { get; init; }
  
    public required CredentialState Status      { get; init; }
    public string?                 Message      { get; init; }
} 