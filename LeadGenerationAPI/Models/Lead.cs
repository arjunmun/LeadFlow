using System;

namespace LeadGenerationAPI.Models
{
    public class Lead
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProfileUrl { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class ScoredLead : Lead
    {
        public int IndustryMatch { get; set; }  // 1 if industry matches, else 0
        public int JobTitleMatch { get; set; }  // 1 if job title matches, else 0
        public int Connections { get; set; }    // Estimated number of connections
        public float Score { get; set; }        // AI-generated lead score
    }
} 