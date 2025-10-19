using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Public.DTO.Workflows;

namespace Google_OAuth2.Services.Workflows
{
    public class WorkflowService : IWorkflowService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WorkflowService> _logger;
        public WorkflowService(HttpClient httpClient, ILogger<WorkflowService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<WorkflowResponse> CreateWorkflowAsync(WorkflowCreateRequest req, CancellationToken ct = default)
        {
            

            var resp = await _httpClient.PostAsJsonAsync("/api/v1/workflows", req, ct);
            await EnsureSuccessOrThrow(resp);

            return await resp.Content.ReadFromJsonAsync<WorkflowResponse>(cancellationToken: ct)
                   ?? throw new InvalidOperationException("Failed to deserialize workflow response.");
        }

        public async Task<List<WorkflowResponse>> GetAllWorkflowsAsync(CancellationToken ct = default)
        {
            try
            {
                var resp = await _httpClient.GetAsync("/api/v1/workflows", ct);
                await EnsureSuccessOrThrow(resp);
                return await resp.Content.ReadFromJsonAsync<List<WorkflowResponse>>(cancellationToken: ct)
                    ?? throw new InvalidOperationException("Failed to deserialize workflows list.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all workflows");
                throw;
            }
        }

        public async Task<WorkflowResponse> GetWorkflowByIdAsync(string id, CancellationToken ct = default)
        {
            try
            {
                var resp = await _httpClient.GetAsync($"/api/v1/workflows/{id}", ct);
                await EnsureSuccessOrThrow(resp);
                return await resp.Content.ReadFromJsonAsync<WorkflowResponse>(cancellationToken: ct)
                    ?? throw new InvalidOperationException($"Failed to deserialize workflow with id {id}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting workflow by id {WorkflowId}", id);
                throw;
            }
        }

        public async Task DeleteWorkflowAsync(string id, CancellationToken ct = default)
        {
            try
            {
                var resp = await _httpClient.DeleteAsync($"/api/v1/workflows/{id}", ct);
                await EnsureSuccessOrThrow(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting workflow {WorkflowId}", id);
                throw;
            }
        }

        public async Task<WorkflowResponse> UpdateWorkflowAsync(string id, WorkflowCreateRequest req, CancellationToken ct = default)
        {
            try
            {
                var resp = await _httpClient.PutAsJsonAsync($"/api/v1/workflows/{id}", req, ct);
                await EnsureSuccessOrThrow(resp);
                return await resp.Content.ReadFromJsonAsync<WorkflowResponse>(cancellationToken: ct)
                    ?? throw new InvalidOperationException($"Failed to deserialize updated workflow with id {id}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating workflow {WorkflowId}", id);
                throw;
            }
        }

        public async Task ActivateWorkflowAsync(string id, CancellationToken ct = default)
        {
            try
            {
                var resp = await _httpClient.PostAsync($"/api/v1/workflows/{id}/activate", null, ct);
                await EnsureSuccessOrThrow(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating workflow {WorkflowId}", id);
                throw;
            }
        }

        public async Task DeactivateWorkflowAsync(string id, CancellationToken ct = default)
        {
            try
            {
                var resp = await _httpClient.PostAsync($"/api/v1/workflows/{id}/deactivate", null, ct);
                await EnsureSuccessOrThrow(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deactivating workflow {WorkflowId}", id);
                throw;
            }
        }

        public async Task TransferWorkflowAsync(string id, WorkflowTransferRequest req, CancellationToken ct = default)
        {
            try
            {
                var resp = await _httpClient.PostAsJsonAsync($"/api/v1/workflows/{id}/transfer", req, ct);
                await EnsureSuccessOrThrow(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error transferring workflow {WorkflowId}", id);
                throw;
            }
        }

        public async Task TransferCredentialAsync(string id, CredentialTransferRequest req, CancellationToken ct = default)
        {
            try
            {
                var resp = await _httpClient.PostAsJsonAsync($"/api/v1/credentials/{id}/transfer", req, ct);
                await EnsureSuccessOrThrow(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error transferring credential {CredentialId}", id);
                throw;
            }
        }

        public async Task<List<string>> GetWorkflowTagsAsync(CancellationToken ct = default)
        {
            try
            {
                var resp = await _httpClient.GetAsync("/api/v1/workflows/tags", ct);
                await EnsureSuccessOrThrow(resp);
                return await resp.Content.ReadFromJsonAsync<List<string>>(cancellationToken: ct)
                    ?? throw new InvalidOperationException("Failed to deserialize workflow tags.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting workflow tags");
                throw;
            }
        }

        public async Task UpdateWorkflowTagsAsync(string id, WorkflowTagsUpdateRequest req, CancellationToken ct = default)
        {
            try
            {
                var resp = await _httpClient.PatchAsJsonAsync($"/api/v1/workflows/{id}/tags", req, ct);
                await EnsureSuccessOrThrow(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating workflow tags for {WorkflowId}", id);
                throw;
            }
        }

        private static async Task EnsureSuccessOrThrow(HttpResponseMessage resp)
        {
            if (!resp.IsSuccessStatusCode)
                {
                var content = await resp.Content.ReadAsStringAsync();
                throw new HttpRequestException($"n8n API error: {(int)resp.StatusCode} {resp.ReasonPhrase} - {content}", null, resp.StatusCode);
            }
        }
    }
} 