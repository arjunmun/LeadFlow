using Microsoft.AspNetCore.Mvc;

namespace LeadGenerationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AiGeneratorController : ControllerBase
    {
        private readonly AIGenerator _aiGenerator;
        private readonly LinkedInScraper _linkedInScraper;

        public AiGeneratorController(AIGenerator aiGenerator, LinkedInScraper linkedInScraper)
        {
            _aiGenerator = aiGenerator;
            _linkedInScraper = linkedInScraper;
        }

        [HttpGet("GetOutreachMessage")]
        public ActionResult GetOutreachMessage(string name, string jobTitle, string company)
        {
            return Ok(_aiGenerator.GenerateMessageAsync(name, jobTitle, company).Result);
        }

        [HttpPost("SendOutreachEmail")]
        public async Task<ActionResult> SendOutreachEmail(string toEmail, string name, string jobTitle, string company)
        {
            try
            {
                toEmail = string.IsNullOrEmpty(toEmail) ? "suresh.dhadhi@sfhawk.com": toEmail;
                await _aiGenerator.SendOutreachEmailAsync(toEmail, name, jobTitle, company);
                return Ok("Email sent successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to send email: {ex.Message}");
            }
        }

        //[HttpGet("LinkedInScrapper")]
        //public ActionResult LinkedInScrapper(string query)
        //{
        //    return Ok(_linkedInScraper.GetLeads(query));
        //}
    }
}
