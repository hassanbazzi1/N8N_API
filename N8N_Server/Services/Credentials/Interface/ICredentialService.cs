using Google_OAuth2.Services.Workflows;
using Public.DTO.Credentials.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Google_OAuth2.Services.Credentials.Interface
{
    public interface ICredentialService
    {
        Task<CredentialResponse> CreateAsync(CredentialCreateRequest req, CancellationToken ct = default);

        Task<CredentialResponse> UpdateAsync(string credentialId, CredentialCreateRequest req, CancellationToken ct = default);

        Task<CredentialStatusResponse> GetStatusAsync(string credentialId, CancellationToken ct = default);

        Task DeleteAsync(string credentialId, CancellationToken ct = default);

        Task<N8nCredentialDto?> GetByIdAsync(string credentialId, CancellationToken ct = default);

        Task<IEnumerable<N8nCredentialDto>> GetAllAsync(CancellationToken ct = default);

        string GetAuthUrl(string credentialId);

        Task<string?> GetConsentUrlAsync(string credentialId, CancellationToken ct = default); // Add this
    }
}
