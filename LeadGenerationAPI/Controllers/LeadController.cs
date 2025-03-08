using LeadGenerationAPI;
using LeadGenerationAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class LeadController : ControllerBase
{
    private readonly ILeadService _leadService;

    public LeadController(ILeadService leadService)
    {
        _leadService = leadService;
    }

    [HttpGet]
    public async Task<IActionResult> GetLeads()
    {
        // TODO: Get actual user ID from authentication
        var leads = await _leadService.GetLeadsAsync();
        return Ok(leads);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLead(int id)
    {
        var lead = await _leadService.GetLeadByIdAsync(id);
        if (lead == null)
        {
            return NotFound();
        }
        return Ok(lead);
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateLeads([FromBody] GenerateLeadsRequest request)
    {
        if (string.IsNullOrEmpty(request.SearchQuery))
        {
            return BadRequest("Search query is required");
        }

        // TODO: Get actual user ID from authentication
        int userId = 1; // Temporary default value
        var leads = await _leadService.GenerateLeadsAsync(request.SearchQuery);
        return Ok(leads);
    }
}

public class GenerateLeadsRequest
{
    public string SearchQuery { get; set; }
}