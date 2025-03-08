using System.Collections.Generic;
using System.Threading.Tasks;
using LeadGenerationAPI.Models;

namespace LeadGenerationAPI.Services
{
    public interface ILeadService
    {
        Task<IEnumerable<Lead>> GetLeadsAsync();
        Task<Lead> GetLeadByIdAsync(int id);
        Task<IEnumerable<ScoredLead>> GenerateLeadsAsync(string searchQuery);
        Task<ScoredLead> CreateLeadAsync(Lead lead);
        Task<bool> UpdateLeadAsync(Lead lead);
        Task<bool> DeleteLeadAsync(int id);
    }
} 