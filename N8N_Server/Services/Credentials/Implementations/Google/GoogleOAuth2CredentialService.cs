using Google_OAuth2.Services.Credentials.Abstract;
using System.Text.Json;
using Public.DTO.Credentials.Common;

namespace Google_OAuth2.Services.Credentials.Implementations.Google
{
    public sealed class GoogleOAuth2CredentialService : CredentialServiceBase
    {
        public GoogleOAuth2CredentialService(HttpClient n8n, IConfiguration cfg)
            : base(n8n, cfg) { }

        private const string CredentialEndpoint = "/rest/credentials";

        public override async Task<CredentialResponse> CreateAsync(CredentialCreateRequest req, CancellationToken ct = default)
        {
            var google = req.GoogleData ?? throw new ArgumentException("GoogleData is required.");

            var body = new
            {
                name = req.Name,
                type = req.Provider.ToString(),
                data = new
                {
                    clientId = google.ClientId,
                    clientSecret = google.ClientSecret,
               
                    accessTokenUrl = "https://oauth2.googleapis.com/token",
                    authUrl = "https://accounts.google.com/o/oauth2/v2/auth",
                    authQueryParameters = "",
                }
            };

            var resp = await _n8n.PostAsJsonAsync(CredentialEndpoint, body, ct);
            if (!resp.IsSuccessStatusCode)
                throw new HttpRequestException($"n8n Google credential create failed: {await resp.Content.ReadAsStringAsync(ct)}");

            var json = await resp.Content.ReadAsStringAsync(ct);

            string? id = null;
            if (!string.IsNullOrWhiteSpace(json))
            {
                using var doc = JsonDocument.Parse(json);
                // Try root "id"
                if (doc.RootElement.TryGetProperty("id", out var rid) && rid.ValueKind == JsonValueKind.String)
                {
                    id = rid.GetString();
                }
                // Try inside "data" object
                else if (doc.RootElement.TryGetProperty("data", out var d) && d.ValueKind == JsonValueKind.Object)
                {
                    if (d.TryGetProperty("id", out var did) && did.ValueKind == JsonValueKind.String)
                        id = did.GetString();
                }
            }

            return new CredentialResponse
            {
                CredentialId = id,
                Provider = req.Provider,
                IsAuthorised = false, // until OAuth completed
                AuthUrl = id != null ? $"{_baseUrl}/rest/oauth2-credential/auth?id={id}" : null,
                Message = "Google credential created."
            };
        }

        public override async Task<CredentialResponse> UpdateAsync(string credentialId, CredentialCreateRequest req, CancellationToken ct = default)
        {
            var google = req.GoogleData ?? throw new ArgumentException("GoogleData is required.");

            var body = new
            {
                name = req.Name,
                type = req.Provider.ToString(),
                data = new
                {
                    clientId = google.ClientId,
                    clientSecret = google.ClientSecret,
                    grantType = "authorizationCode",
                    accessTokenUrl = "https://oauth2.googleapis.com/token",
                    authUrl = "https://accounts.google.com/o/oauth2/v2/auth",
                    authQueryParameters = "",
                }
            };

            var resp = await _n8n.PatchAsJsonAsync($"{CredentialEndpoint}/{credentialId}", body, ct);
            if (!resp.IsSuccessStatusCode)
                throw new HttpRequestException($"n8n Google credential update failed: {await resp.Content.ReadAsStringAsync(ct)}");

            var json = await resp.Content.ReadAsStringAsync(ct);

            string? id = null;
            if (!string.IsNullOrWhiteSpace(json))
            {
                using var doc = JsonDocument.Parse(json);
                // Try root "id"
                if (doc.RootElement.TryGetProperty("id", out var rid) && rid.ValueKind == JsonValueKind.String)
                {
                    id = rid.GetString();
                }
                // Try inside "data" object
                else if (doc.RootElement.TryGetProperty("data", out var d) && d.ValueKind == JsonValueKind.Object)
                {
                    if (d.TryGetProperty("id", out var did) && did.ValueKind == JsonValueKind.String)
                        id = did.GetString();
                }
            }

            return new CredentialResponse
            {
                CredentialId = id,
                Provider = req.Provider,
                IsAuthorised = false, // until OAuth completed
                AuthUrl = id != null ? $"{_baseUrl}/rest/oauth2-credential/auth?id={id}" : null,
                Message = "Google credential created."
            };
        }
    }
}
