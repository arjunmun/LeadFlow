using LeadGenerationAPI;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using static LeadScorer;
[Route("api/leads")]
[ApiController]
public class LeadController : ControllerBase
{
    private readonly LinkedInScraper _scraper;
    private readonly LeadScorer _scorer;
    public LeadController()
    {
        _scraper = new LinkedInScraper();  // LinkedIn Scraper
        _scorer = new LeadScorer();        // AI Lead Scorer
    }
    [HttpGet]
    public IActionResult GetLeads([FromQuery] string searchQuery = "CTO Fintech")
    {
        List<Lead> leads = _scraper.GetLeads(searchQuery);
        if (leads == null || leads.Count == 0)
        {
            return NotFound(new { message = "No leads found for the given query." });
        }
        // Score leads by assigning estimated IndustryMatch, JobTitleMatch, Connections
        List<ScoredLead> scoredLeads = leads.Select(lead =>
        {
            var leadData = new LeadData
            {
                IndustryMatch = lead.Name.ToLower().Contains("fintech") ? 1 : 0,  // Approximate industry match
                JobTitleMatch = lead.Name.ToLower().Contains("cto") ? 1 : 0,     // Approximate job title match
                Connections = new System.Random().Next(1, 10)                     // Randomly simulate connection count
            };
            float score = _scorer.ScoreLead(leadData);
            return new ScoredLead
            {
                Name = lead.Name,
                ProfileUrl = lead.ProfileUrl,
                IndustryMatch = leadData.IndustryMatch,
                JobTitleMatch = leadData.JobTitleMatch,
                Connections = leadData.Connections,
                Score = score
            };
        })
        .OrderByDescending(l => l.Score)  // Sort by highest score
        .ToList();
        return Ok(scoredLeads);  // Return leads with scores
    }
}