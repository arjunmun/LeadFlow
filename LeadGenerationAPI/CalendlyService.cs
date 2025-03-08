using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class CalendlyService
{
    private static readonly HttpClient httpClient = new();
    private readonly string apiKey;

    public CalendlyService(string apiKey)
    {
        this.apiKey = apiKey;
    }

    // Method to retrieve event types
    public async Task<string> GetEventTypes()
    {
        string apiUrl = "https://api.calendly.com/event_types";
        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var response = await httpClient.GetAsync(apiUrl);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    // Method to retrieve a specific event type
    public async Task<string> GetEventType(string eventTypeId)
    {
        string apiUrl = $"https://api.calendly.com/event_types/{eventTypeId}";
        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var response = await httpClient.GetAsync(apiUrl);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    // Method to generate a scheduling link for an event type
    public async Task<string> GenerateSchedulingLink(string eventTypeId)
    {
        // Generate a scheduling link by retrieving the event type and extracting the link
        var eventTypeResponse = await GetEventType(eventTypeId);
        var eventTypeData = JsonConvert.DeserializeObject<EventTypeResponse>(eventTypeResponse);
        return eventTypeData.scheduling_url;
    }

    // Helper class for deserializing event type response
    private class EventTypeResponse
    {
        public string scheduling_url { get; set; }
    }

    // Example method to schedule a meeting by sharing the scheduling link
    public async Task<string> ScheduleMeeting(string email, string eventTypeId)
    {
        // Generate the scheduling link
        var schedulingLink = await GenerateSchedulingLink(eventTypeId);

        // Share the scheduling link with participants
        // For demonstration purposes, just return the link
        return $"Please schedule a meeting using this link: {schedulingLink}";
    }
}
