using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using Microsoft.Identity.Client;
using System.Text.RegularExpressions;
using System.Linq;

namespace LeadGenerationAPI
{
    public class LinkedInScraper
    {
        private string ExtractName(string fullText)
        {
            if (string.IsNullOrEmpty(fullText)) return string.Empty;
            
            // Get text before first newline
            var parts = fullText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            return parts.Length > 0 ? parts[0].Trim() : string.Empty;
        }

        private string GenerateEmail(string fullName, string companyName)
        {
            try
            {
                // Split full name into parts
                var nameParts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (nameParts.Length < 2) return string.Empty;

                string firstName = nameParts[0].ToLower();
                string lastName = nameParts[^1].ToLower();

                // Process company name for domain
                string domain;
                if (string.IsNullOrWhiteSpace(companyName))
                {
                    domain = "gmail.com";
                }
                else
                {
                    // If company name has spaces, create abbreviation
                    var companyParts = companyName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (companyParts.Length > 1)
                    {
                        // Create abbreviation (e.g., "Microsoft Corporation" -> "mc")
                        domain = string.Join("", companyParts.Select(p => p[0].ToString().ToLower())) + ".com";
                    }
                    else
                    {
                        // Use full company name if it's a single word
                        domain = companyParts[0].ToLower() + ".com";
                    }
                }

                return $"{firstName}.{lastName}@{domain}";
            }
            catch
            {
                return string.Empty;
            }
        }

        private void EnrichLeadDetails(IWebDriver driver, Lead lead)
        {
            try
            {
                // Navigate to profile page
                driver.Navigate().GoToUrl(lead.ProfileUrl);
                System.Threading.Thread.Sleep(2000); // Wait for page to load

                var fullText = driver.FindElement(By.ClassName("text-body-medium")).Text;
                
                // Check if the text contains "at " and split accordingly
                if (fullText.Contains(" at "))
                {
                    var parts = fullText.Split(new[] { " at " }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 2)
                    {
                        lead.JobTitle = parts[0].Trim();
                        lead.CompanyName = parts[1].Trim();
                    }
                    else
                    {
                        lead.JobTitle = fullText;
                    }
                }
                else
                {
                    lead.JobTitle = fullText;
                }

                // Generate email after we have company name
                lead.EmailAddress = GenerateEmail(lead.Name, lead.CompanyName);
            }
            catch (NoSuchElementException)
            {
                // Element not found, keep default empty values
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching profile details for {lead.Name}: {ex.Message}");
            }
        }

        public List<Lead> GetLeads(string searchQuery)
        {
            IWebDriver driver = new ChromeDriver();
            try
            {
                // Step 1: Login to LinkedIn
                driver.Navigate().GoToUrl("https://www.linkedin.com/login");
                driver.FindElement(By.Id("username")).SendKeys("azharuddin.pathan@innovapoint.com");
                driver.FindElement(By.Id("password")).SendKeys("Innova@123");
                driver.FindElement(By.Id("password")).Submit();
                System.Threading.Thread.Sleep(3000);

                // Step 2: Search and collect basic lead information
                driver.Navigate().GoToUrl($"https://www.linkedin.com/search/results/people/?keywords={searchQuery}");
                System.Threading.Thread.Sleep(3000);

                List<Lead> leads = new();
                var profiles = driver.FindElements(By.ClassName("HdksGfPqoWJtcEmYIiclPMfHqDqLecN"));

                // First pass: Collect basic information
                foreach (var profile in profiles)
                {
                    string fullText = profile.Text;
                    string name = ExtractName(fullText);
                    string profileLink = profile.FindElement(By.TagName("a")).GetAttribute("href");

                    leads.Add(new ScoredLead 
                    { 
                        Name = name,
                        ProfileUrl = profileLink,
                        JobTitle = "",
                        CompanyName = "",
                        EmailAddress = "",
                        Connections = 0,
                        IndustryMatch = 0,
                        JobTitleMatch = 0,
                        Score = 0
                    });
                }

                // Step 3: Enrich leads with additional details
                foreach (var lead in leads)
                {
                    EnrichLeadDetails(driver, lead);
                    
                    // Return to search results for next profile
                    driver.Navigate().GoToUrl($"https://www.linkedin.com/search/results/people/?keywords={searchQuery}");
                    System.Threading.Thread.Sleep(1000);
                }

                return leads;
            }
            finally
            {
                driver.Quit();
            }
        }
    }

    public class Lead
    {
        public string Name { get; set; }
        public string ProfileUrl { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string EmailAddress { get; set; }
    }
    
    public class ScoredLead : Lead
    {
        public int IndustryMatch { get; set; }  // 1 if industry matches, else 0
        public int JobTitleMatch { get; set; }  // 1 if job title matches, else 0
        public int Connections { get; set; }    // Estimated number of connections
        public float Score { get; set; }        // AI-generated lead score
    }
}
