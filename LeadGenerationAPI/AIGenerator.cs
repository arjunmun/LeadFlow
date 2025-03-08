//using System;
//using System.Threading.Tasks;
//using OpenAI.Chat;
//using OpenAI.Managers;
//using OpenAI.ObjectModels;
//using OpenAI.ObjectModels.RequestModels;

//public class AIGenerator
//{
//    private readonly OpenAIService _openAiService;

//    public AIGenerator()
//    {
//        var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
//        if (string.IsNullOrEmpty(apiKey))
//        {
//            throw new InvalidOperationException("API key not found in environment variables.");
//        }

//        _openAiService = new OpenAIService(new OpenAiOptions
//        {
//            ApiKey = apiKey
//        });
//    }

//    public async Task<string> GenerateMessageAsync(string name, string jobTitle, string company)
//    {
//        try
//        {
//            var response = await _openAiService.ChatCompletion.CreateChatCompletionAsync(new ChatCompletionCreateRequest
//            {
//                Model = Models.Gpt_4,
//                Messages = new[]
//                {
//                    new ChatMessage("system", "You are an AI that writes professional LinkedIn outreach messages."),
//                    new ChatMessage("user", $"Write a LinkedIn message for {name}, {jobTitle} at {company}.")
//                }
//            });

//            if (response.Successful)
//            {
//                return response.Choices[0].Message.Content;
//            }
//            else
//            {
//                Console.WriteLine($"Error: {response.Error?.Message}");
//                return "Error generating message.";
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Exception: {ex.Message}");
//            return "An unexpected error occurred.";
//        }
//    }
//}
