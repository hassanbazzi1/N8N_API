using Google_OAuth2.Services.Credentials.Interface;
using Google_OAuth2.Services.Workflows;
using Public.DTO.Credentials.Common;
using System.Net;
using System.Text.Json;

namespace Google_OAuth2.Services.Credentials.Abstract
{
    public abstract class CredentialServiceBase : ICredentialService
    {
        protected readonly HttpClient _n8n;
        protected readonly string _baseUrl;
        protected readonly IWorkflowService _workflowService;
        protected const string CredentialEndpoint = "/rest/credentials";
        protected const string AuthUrlEndpoint = "/rest/oauth2-credential/auth";

        protected CredentialServiceBase(HttpClient n8n, IConfiguration cfg)
        {
            _n8n = n8n;
            _baseUrl = "https://n8n.srv898423.hstgr.cloud";
        }

        public abstract Task<CredentialResponse> CreateAsync(
            CredentialCreateRequest req, CancellationToken ct = default);

        public abstract Task<CredentialResponse> UpdateAsync(
            string credentialId, CredentialCreateRequest req, CancellationToken ct = default);

        public virtual async Task DeleteAsync(string credentialId, CancellationToken ct = default)
        {
            var resp = await _n8n.DeleteAsync($"{CredentialEndpoint}/{credentialId}", ct);
            if (!resp.IsSuccessStatusCode)
            {
                var err = await resp.Content.ReadAsStringAsync(ct);
                throw new HttpRequestException($"n8n credential delete failed ({(int)resp.StatusCode}): {err}");
            }
        }

        public virtual async Task<N8nCredentialDto?> GetByIdAsync(string credentialId, CancellationToken ct = default)
        {
            var resp = await _n8n.GetAsync($"{CredentialEndpoint}/{credentialId}", ct);
            if (!resp.IsSuccessStatusCode)
            {
                var err = await resp.Content.ReadAsStringAsync(ct);
                throw new HttpRequestException($"n8n credential get failed ({(int)resp.StatusCode}): {err}");
            }

            var root = await resp.Content.ReadFromJsonAsync<N8nCredentialSingleDto>(cancellationToken: ct);
            return root?.data;
        }

        public virtual async Task<IEnumerable<N8nCredentialDto>> GetAllAsync(CancellationToken ct = default)
        {
            var resp = await _n8n.GetAsync(CredentialEndpoint, ct);
            if (!resp.IsSuccessStatusCode)
            {
                var err = await resp.Content.ReadAsStringAsync(ct);
                throw new HttpRequestException($"n8n credential list failed ({(int)resp.StatusCode}): {err}");
            }

            // Deserialize as a wrapper object, then return the 'data' property
            var root = await resp.Content.ReadFromJsonAsync<N8nCredentialListDto>(cancellationToken: ct);
            return root?.data ?? new List<N8nCredentialDto>();
        }


        public virtual Task<CredentialStatusResponse> GetStatusAsync(string credentialId, CancellationToken ct = default)
        {
            return Task.FromResult(new CredentialStatusResponse
            {
                CredentialId = credentialId,
                Status = CredentialState.Authorized,
                Message = "Credential stored (non-OAuth, default status)."
            });
        }

        public virtual string GetAuthUrl(string credentialId)
            => $"{_baseUrl}/rest/oauth2-credential/auth?id={credentialId}";

        public virtual async Task<string?> GetConsentUrlAsync(string credentialId, CancellationToken ct = default)
        {
            var baseResp = await _n8n.GetStringAsync($"{_baseUrl}{AuthUrlEndpoint}?id={credentialId}", ct);
            if (string.IsNullOrWhiteSpace(baseResp)) return null;

            var url = JsonDocument.Parse(baseResp).RootElement.TryGetProperty("data", out var d)
                        ? d.GetString()
                        : JsonDocument.Parse(baseResp).RootElement.TryGetProperty("url", out var u)
                            ? u.GetString()
                            : null;

            if (string.IsNullOrWhiteSpace(url)) return null;

            // append access_type=offline&prompt=consent (avoid duplicates)
            var ub = new UriBuilder(url);
            var query = System.Web.HttpUtility.ParseQueryString(ub.Query);
            query["access_type"] = "offline";
            query["prompt"]      = "consent";
            ub.Query = query.ToString()!;

            return ub.ToString();
        }


        public class AuthUrlResponse
        {
            public string? url { get; set; }
        }
    }
}
