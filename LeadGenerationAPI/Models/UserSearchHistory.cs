using System;

namespace LeadGenerationAPI.Models
{
    public class UserSearchHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SearchQuery { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation property
        public virtual User User { get; set; }
    }
} 