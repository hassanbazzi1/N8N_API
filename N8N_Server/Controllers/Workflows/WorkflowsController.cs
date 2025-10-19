using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using N8N_API.Entities;

using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Google_OAuth2.Services.Workflows;
using Public.DTO.Workflows;
using System;

namespace Google_OAuth2.Controllers.Workflows
{
    [ApiController]
    [Route("api/workflows")]
    public class WorkflowsController : ControllerBase
    {
        private readonly IWorkflowService _workflowService;
        private readonly ILogger<WorkflowsController> _logger;
   

        public WorkflowsController(
            IWorkflowService workflowService,
            ILogger<WorkflowsController> logger
          )
        {
            _workflowService = workflowService;
            _logger = logger;
       
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WorkflowCreateRequest req, CancellationToken ct)
        {
            try
            {
                var result = await _workflowService.CreateWorkflowAsync(req, ct);
                
                // Persist to local database after successful n8n operation
                if (result.Id != null)
                {
                    var workflowEntity = new WorkflowEntity
                    {
                        Id = Guid.NewGuid(),
                        ExternalId = result.Id,
                        Created = DateTime.UtcNow
                    };
                    
                 
                }
                
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "n8n API error during workflow creation");
                return StatusCode(
                    (int)(ex.StatusCode.GetValueOrDefault(HttpStatusCode.InternalServerError)),
                    Problem(
                        detail: ex.Message,
                        statusCode: (int)(ex.StatusCode.GetValueOrDefault(HttpStatusCode.InternalServerError))
                    )
                );
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Request timed out during workflow creation");
                return StatusCode(504, Problem(detail: "Request timed out.", statusCode: 504));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Deserialization error during workflow creation");
                return BadRequest(Problem(detail: ex.Message, statusCode: 400));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during workflow creation");
                return StatusCode(500, Problem(detail: "An unexpected error occurred.", statusCode: 500));
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            try
            {
                var result = await _workflowService.GetAllWorkflowsAsync(ct);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "n8n API error during get all workflows");
                return StatusCode(
                    (int)(ex.StatusCode.GetValueOrDefault(HttpStatusCode.InternalServerError)),
                    Problem(
                        detail: ex.Message,
                        statusCode: (int)(ex.StatusCode.GetValueOrDefault(HttpStatusCode.InternalServerError))
                    )
                );
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Request timed out during get all workflows");
                return StatusCode(504, Problem(detail: "Request timed out.", statusCode: 504));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during get all workflows");
                return StatusCode(500, Problem(detail: "An unexpected error occurred.", statusCode: 500));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id, CancellationToken ct)
        {
            try
            {
                var result = await _workflowService.GetWorkflowByIdAsync(id, ct);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "n8n API error during get workflow by id");
                return StatusCode(
                    (int)(ex.StatusCode.GetValueOrDefault(HttpStatusCode.InternalServerError)),
                    Problem(
                        detail: ex.Message,
                        statusCode: (int)(ex.StatusCode.GetValueOrDefault(HttpStatusCode.InternalServerError))
                    )
                );
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Request timed out during get workflow by id");
                return StatusCode(504, Problem(detail: "Request timed out.", statusCode: 504));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during get workflow by id");
                return StatusCode(500, Problem(detail: "An unexpected error occurred.", statusCode: 500));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken ct)
        {
            try
            {
                await _workflowService.DeleteWorkflowAsync(id, ct);
                return NoContent();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "n8n API error during delete workflow");
                return StatusCode(
                    (int)(ex.StatusCode.GetValueOrDefault(HttpStatusCode.InternalServerError)),
                    Problem(
                        detail: ex.Message,
                        statusCode: (int)(ex.StatusCode.GetValueOrDefault(HttpStatusCode.InternalServerError))
                    )
                );
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Request timed out during delete workflow");
                return StatusCode(504, Problem(detail: "Request timed out.", statusCode: 504));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during delete workflow");
                return StatusCode(500, Problem(detail: "An unexpected error occurred.", statusCode: 500));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] WorkflowCreateRequest req, CancellationToken ct)
        {
            try
            {
                var result = await _workflowService.UpdateWorkflowAsync(id, req, ct);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "n8n API error during update workflow");
                return StatusCode(
                    (int)(ex.StatusCode.GetValueOrDefault(HttpStatusCode.InternalServerError)),
                    Problem(
                        detail: ex.Message,
                        statusCode: (int)(ex.StatusCode.GetValueOrDefault(HttpStatusCode.InternalServerError))
                    )
                );
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Request timed out during update workflow");
                return StatusCode(504, Problem(detail: "Request timed out.", statusCode: 504));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Deserialization error during update workflow");
                return BadRequest(Problem(detail: ex.Message, statusCode: 400));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during update workflow");
                return StatusCode(500, Problem(detail: "An unexpected error occurred.", statusCode: 500));
            }
        }

        [HttpPost("{id}/activate")]
        public async Task<IActionResult> Activate(string id, CancellationToken ct)
        {
            try
            {
                await _workflowService.ActivateWorkflowAsync(id, ct);
                return NoContent();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "n8n API error during activate workflow");
                return StatusCode(
                    (int)(ex.StatusCode.GetValueOrDefault(HttpStatusCode.InternalServerError)),
                    Problem(
                        detail: ex.Message,
                        statusCode: (int)(ex.StatusCode.GetValueOrDefault(HttpStatusCode.InternalServerError))
                    )
                );
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Request timed out during activate workflow");
                return StatusCode(504, Problem(detail: "Request timed out.", statusCode: 504));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during activate workflow");
                return StatusCode(500, Problem(detail: "An unexpected error occurred.", statusCode: 500));
            }
        }

        [HttpPost("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(string id, CancellationToken ct)
        {
            try
            {
                await _workflowService.DeactivateWorkflowAsync(id, ct);
                return NoContent();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "n8n API error during deactivate workflow");
                return StatusCode(
                    (int)(ex.StatusCode.GetValueOrDefault(HttpStatusCode.InternalServerError)),
                    Problem(
                        detail: ex.Message,
                        statusCode: (int)(ex.StatusCode.GetValueOrDefault(HttpStatusCode.InternalServerError))
                    )
                );
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Request timed out during deactivate workflow");
                return StatusCode(504, Problem(detail: "Request timed out.", statusCode: 504));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during deactivate workflow");
                return StatusCode(500, Problem(detail: "An unexpected error occurred.", statusCode: 500));
            }
        }

        [HttpPost("{id}/transfer")]
        public async Task<IActionResult> Transfer(string id, [FromBody] WorkflowTransferRequest req, CancellationToken ct)
        {
            try
            {
                await _workflowService.TransferWorkflowAsync(id, req, ct);
                return NoContent();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "n8n API error during transfer workflow");
                return StatusCode(
                    (int)(ex.StatusCode.GetValueOrDefault(HttpStatusCode.InternalServerError)),
                    Problem(
                        detail: ex.Message,
                        statusCode: (int)(ex.StatusCode.GetValueOrDefault(HttpStatusCode.InternalServerError))
                    )
                );
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Request timed out during transfer workflow");
                return StatusCode(504, Problem(detail: "Request timed out.", statusCode: 504));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during transfer workflow");
                return StatusCode(500, Problem(detail: "An unexpected error occurred.", statusCode: 500));
            }
        }

        [HttpPost("credentials/{id}/transfer")]
        public async Task<IActionResult> TransferCredential(string id, [FromBody] CredentialTransferRequest req, CancellationToken ct)
        {
            try
            {
                await _workflowService.TransferCredentialAsync(id, req, ct);
                return NoContent();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "n8n API error during transfer credential");
                return StatusCode(
                    (int)(ex.StatusCode.GetValueOrDefault(HttpStatusCode.InternalServerError)),
                    Problem(
                        detail: ex.Message,
                        statusCode: (int)(ex.StatusCode.GetValueOrDefault(HttpStatusCode.InternalServerError))
                    )
                );
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Request timed out during transfer credential");
                return StatusCode(504, Problem(detail: "Request timed out.", statusCode: 504));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during transfer credential");
                return StatusCode(500, Problem(detail: "An unexpected error occurred.", statusCode: 500));
            }
        }

       
    }
}
