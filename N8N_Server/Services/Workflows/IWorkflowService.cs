using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Public.DTO.Workflows;

namespace Google_OAuth2.Services.Workflows
{
    public interface IWorkflowService
    {
        Task<WorkflowResponse> CreateWorkflowAsync(WorkflowCreateRequest req, CancellationToken ct = default);
        Task<List<WorkflowResponse>> GetAllWorkflowsAsync(CancellationToken ct = default);
        Task<WorkflowResponse> GetWorkflowByIdAsync(string id, CancellationToken ct = default);
        Task DeleteWorkflowAsync(string id, CancellationToken ct = default);
        Task<WorkflowResponse> UpdateWorkflowAsync(string id, WorkflowCreateRequest req, CancellationToken ct = default);
        Task ActivateWorkflowAsync(string id, CancellationToken ct = default);
        Task DeactivateWorkflowAsync(string id, CancellationToken ct = default);
        Task TransferWorkflowAsync(string id, WorkflowTransferRequest req, CancellationToken ct = default);
        Task TransferCredentialAsync(string id, CredentialTransferRequest req, CancellationToken ct = default);
        Task<List<string>> GetWorkflowTagsAsync(CancellationToken ct = default);
        Task UpdateWorkflowTagsAsync(string id, WorkflowTagsUpdateRequest req, CancellationToken ct = default);
    }
} 