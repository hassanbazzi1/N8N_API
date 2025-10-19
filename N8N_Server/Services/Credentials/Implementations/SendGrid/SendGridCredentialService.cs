using Google_OAuth2.Services.Credentials.Abstract;
using Public.DTO.Credentials.Common;
using System.Text.Json;

namespace Google_OAuth2.Services.Credentials.Implementations.SendGrid
{
    public sealed class SendGridCredentialService : CredentialServiceBase
    {
        public SendGridCredentialService(HttpClient n8n, IConfiguration cfg) : base(n8n, cfg) { }

        public override async Task<CredentialResponse> CreateAsync(CredentialCreateRequest req, CancellationToken ct = default)
        {
            var data = req.SendGridData ?? throw new ArgumentException("SendGridData is required.");
            var body = new
            {
                name = req.Name,
                type = "sendGridApi",
                data = new { apiKey = data.ApiKey }
            };

            var resp = await _n8n.PostAsJsonAsync(CredentialEndpoint, body, ct);
            if (!resp.IsSuccessStatusCode)
                throw new HttpRequestException($"n8n SendGrid credential create failed: {await resp.Content.ReadAsStringAsync(ct)}");

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
                Message = "SendGrid credential created."
            };
        }

        public override async Task<CredentialResponse> UpdateAsync(string credentialId, CredentialCreateRequest req, CancellationToken ct = default)
        {
            var data = req.SendGridData ?? throw new ArgumentException("SendGridData is required.");
            var body = new
            {
                name = req.Name,
                type = "sendGridApi",
                data = new { apiKey = data.ApiKey }
            };

            var resp = await _n8n.PatchAsJsonAsync($"{CredentialEndpoint}/{credentialId}", body, ct);
            if (!resp.IsSuccessStatusCode)
                throw new HttpRequestException($"n8n SendGrid credential update failed: {await resp.Content.ReadAsStringAsync(ct)}");

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
                Message = "SendGrid credential created."
            };
        }
    }
}
