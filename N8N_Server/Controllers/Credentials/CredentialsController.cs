using Google_OAuth2.Factory.Credential;
using Microsoft.AspNetCore.Mvc;
using N8N_API.Entities;

using Public.DTO.Credentials.Common;

[ApiController]
[Route("api/credentials")]
public class CredentialsController : ControllerBase
{
    private readonly ICredentialServiceFactory _factory;
 

    public CredentialsController(
        ICredentialServiceFactory factory
       )
    {
        _factory = factory;
       
    }

    // Create
    [HttpPost]
    public async Task<ActionResult<CredentialResponse>> Create([FromBody] CredentialCreateRequest req, CancellationToken ct)
    {
        var service = _factory.GetService(req.Provider);
        var result = await service.CreateAsync(req, ct);

 

        return Ok(result);
    }

    // Update
    [HttpPatch("{provider}/{credentialId}")]
    public async Task<ActionResult<CredentialResponse>> Update(
        CredentialProvider provider, string credentialId,
        [FromBody] CredentialCreateRequest req, CancellationToken ct)
    {
        var service = _factory.GetService(provider);
        var result = await service.UpdateAsync(credentialId, req, ct);

     

        return Ok(result);
    }
    // Get all credentials from n8n
    [HttpGet]
    public async Task<ActionResult<IEnumerable<N8nCredentialDto>>> GetAll([FromQuery] CredentialProvider provider,CancellationToken ct)
    {
  
        var service = _factory.GetService(provider);
        var creds = await service.GetAllAsync(ct);
        return Ok(creds);
    }

    // Get credential by ID from n8n
    [HttpGet("{credentialId}")]
    public async Task<ActionResult<N8nCredentialDto>> GetById(string credentialId, [FromQuery] CredentialProvider provider, CancellationToken ct)
    {
        var service = _factory.GetService(provider);
        var cred = await service.GetByIdAsync(credentialId, ct);
        if (cred == null)
            return NotFound();
        return Ok(cred);
    }



    // Delete from n8n and local DB
    [HttpDelete("{provider}/{credentialId}")]
    public async Task<IActionResult> Delete(
        CredentialProvider provider, string credentialId, CancellationToken ct)
    {
        var service = _factory.GetService(provider);
        await service.DeleteAsync(credentialId, ct);

        return Ok();
    }

    [HttpGet("{provider}/{credentialId}/consent-url")]
    public async Task<ActionResult<string>> GetConsentUrl(
           CredentialProvider provider, string credentialId, CancellationToken ct)
    {
        var service = _factory.GetService(provider);
        var url = await service.GetConsentUrlAsync(credentialId, ct);

        if (string.IsNullOrWhiteSpace(url))
            return NotFound("No consent URL for this credential.");

        return Ok(url);
    }
}
