using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

public class AIGenerator
{
    private readonly HttpClient _httpClient;
    private readonly string geminiModel = "";
    private readonly string _apiKey;
    private readonly EmailService _emailService;

    public AIGenerator(HttpClient httpClient, IConfiguration configuration, EmailService emailService)
    {
        _httpClient = httpClient;
        _apiKey = configuration["OpenAI:ApiKey"] ?? "";
        geminiModel =
            $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_apiKey}";
        _emailService = emailService;
    }

    public async Task<string> GenerateMessageAsync(string name, string jobTitle, string company)
    {
        try
        {
            string myName = "John Doe";
            string myCompany = "ABC Corp";
            string myJobTitle = "Marketing lead";

            string prompt = string.Empty;

            if (string.IsNullOrEmpty(company))
            {
                prompt = $"Write a professional ready-to-send email outreach email to {name}. My name is {myName} and I work in {myCompany} as {myJobTitle}, Keep it concise, friendly, and engaging, and do not use any placeholder";
            }
            else
            {
                prompt = $"Write a professional ready-to-send email outreach email to {name}, who is a {jobTitle} at {company}. My name is {myName} and I work in {myCompany} as {myJobTitle}, Keep it concise, friendly, and engaging, and do not use any placeholder";
            }

            var requestData = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = prompt } } }
                }
            };

            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = _httpClient.PostAsync(geminiModel, content).Result;
            string result = await response.Content.ReadAsStringAsync();

            // Parse the JSON response to extract the text
            var jsonResponse = JsonNode.Parse(result);
            string generatedText = jsonResponse?["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString() ?? "No response received";

            return generatedText;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            return "An unexpected error occurred.";
        }
    }

    public async Task SendOutreachEmailAsync(string toEmail, string name, string jobTitle, string company)
    {
        string emailBody = await GenerateMessageAsync(name, jobTitle, company);
        string subject = $"Connecting with {name} from {company}";
        await _emailService.SendEmailAsync(toEmail, subject, emailBody);
    }
}