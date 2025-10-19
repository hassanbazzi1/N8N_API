using Google_OAuth2.Services.Credentials.Abstract;
using Public.DTO.Credentials.Common;
using System.Text.Json;

namespace Google_OAuth2.Services.Credentials.Implementations.Google
{
    public sealed class GmailCredentialService : CredentialServiceBase
    {
        private const string DefaultGmailScopes =
            "https://mail.google.com/ https://www.googleapis.com/auth/gmail.labels https://www.googleapis.com/auth/gmail.send https://www.googleapis.com/auth/gmail.compose https://www.googleapis.com/auth/gmail.modify https://www.googleapis.com/auth/gmail.addons.current.action.compose https://www.googleapis.com/auth/gmail.addons.current.message.action https://www.googleapis.com/auth/gmail.addons.current.message.readonly";
        private const string N8nType = "googleOAuth2Api";

        public GmailCredentialService(HttpClient n8n, IConfiguration cfg)
            : base(n8n, cfg) { }

        public override async Task<CredentialResponse> CreateAsync(CredentialCreateRequest req, CancellationToken ct = default)
        {
            var gmail = req.GmailData ?? throw new ArgumentException("GmailData is required.");
            var scope = string.IsNullOrWhiteSpace(gmail.Scope) ? DefaultGmailScopes : gmail.Scope;

            var body = new
            {
                name = req.Name,
                type = N8nType,
                data = new
                {
                    clientId = gmail.ClientId,
                    clientSecret = gmail.ClientSecret,
                    scope
                }
            };

            var resp = await _n8n.PostAsJsonAsync(CredentialEndpoint, body, ct);
            if (!resp.IsSuccessStatusCode)
                throw new HttpRequestException($"n8n Gmail credential create failed: {await resp.Content.ReadAsStringAsync(ct)}");

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
                Message = "Gmail credential created."
            };
        }

        public override async Task<CredentialResponse> UpdateAsync(string credentialId, CredentialCreateRequest req, CancellationToken ct = default)
        {
            var gmail = req.GmailData ?? throw new ArgumentException("GmailData is required.");
            var scope = string.IsNullOrWhiteSpace(gmail.Scope) ? DefaultGmailScopes : gmail.Scope;

            var body = new
            {
                name = req.Name,
                type = N8nType,
                data = new
                {
                    clientId = gmail.ClientId,
                    clientSecret = gmail.ClientSecret,
                    scope
                }
            };

            var resp = await _n8n.PatchAsJsonAsync($"{CredentialEndpoint}/{credentialId}", body, ct);
            if (!resp.IsSuccessStatusCode)
                throw new HttpRequestException($"n8n Gmail credential update failed: {await resp.Content.ReadAsStringAsync(ct)}");


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
                Message = "Gmail credential created."
            };
        }
    }
}
