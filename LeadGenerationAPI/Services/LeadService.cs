using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LeadGenerationAPI.Data;
using LeadGenerationAPI.Models;
using System.Linq;

namespace LeadGenerationAPI.Services
{
    public class LeadService : ILeadService
    {
        private readonly LeadGenerationContext _context;
        private readonly LinkedInScraper _scraper;

        public LeadService(LeadGenerationContext context, LinkedInScraper scraper)
        {
            _context = context;
            _scraper = scraper;
        }

        public async Task<IEnumerable<Lead>> GetLeadsAsync()
        {
            return await _context.Leads.ToListAsync();
        }

        public async Task<Lead> GetLeadByIdAsync(int id)
        {
            return await _context.Leads
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<ScoredLead>> GenerateLeadsAsync(string searchQuery)
        {
            var leads = _scraper.GetLeads(searchQuery, 0);
            return leads.Cast<ScoredLead>();
        }

        public async Task<ScoredLead> CreateLeadAsync(Lead lead)
        {
            if (lead is ScoredLead scoredLead)
            {
                _context.Leads.Add(scoredLead);
                await _context.SaveChangesAsync();
                return scoredLead;
            }
            else
            {
                var newScoredLead = new ScoredLead
                {
                    Name = lead.Name,
                    ProfileUrl = lead.ProfileUrl,
                    JobTitle = lead.JobTitle,
                    CompanyName = lead.CompanyName,
                    EmailAddress = lead.EmailAddress,
                    IndustryMatch = 0,
                    JobTitleMatch = 0,
                    Connections = 0,
                    Score = 0
                };
                _context.Leads.Add(newScoredLead);
                await _context.SaveChangesAsync();
                return newScoredLead;
            }
        }

        public async Task<bool> UpdateLeadAsync(Lead lead)
        {
            if (!(lead is ScoredLead scoredLead))
            {
                return false;
            }

            _context.Entry(scoredLead).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await LeadExists(lead.Id))
                {
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> DeleteLeadAsync(int id)
        {
            var lead = await _context.Leads.FindAsync(id);
            if (lead == null)
            {
                return false;
            }

            _context.Leads.Remove(lead);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> LeadExists(int id)
        {
            return await _context.Leads.AnyAsync(e => e.Id == id);
        }
    }
} 