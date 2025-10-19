namespace Public.DTO.Credentials.Common;

// Minimal projection of an n8n credential record
public sealed record N8nCredentialDto(string Id, string Name, string Type); 