# Lead Generation API

A .NET 8.0 Web API that helps generate personalized outreach emails and manage lead generation processes. The API integrates with AI for message generation and includes LinkedIn lead scraping capabilities.

## Features

- AI-powered personalized email message generation
- LinkedIn lead scraping functionality
- Email sending capabilities via SMTP
- Calendly integration for scheduling meetings
- RESTful API endpoints for easy integration

## Prerequisites

- .NET 8.0 SDK or later
- Gmail account (for email sending) or Office 365 account
- Calendly account (optional, for scheduling features)

## API Endpoints

### AI Message Generation
- `GET /AIGenerator/GetOutreachMessage`
  - Parameters:
    - `name`: Recipient's name
    - `jobTitle`: Recipient's job title
    - `company`: Recipient's company name

### Email Sending
- `POST /AIGenerator/SendOutreachEmail`
  - Parameters:
    - `toEmail`: Recipient's email address
    - `name`: Recipient's name
    - `jobTitle`: Recipient's job title
    - `company`: Recipient's company name

### LinkedIn Scraping
- `GET /AIGenerator/LinkedInScrapper`
  - Parameters:
    - `query`: Search query for LinkedIn leads

### Calendly Integration
- `GET /Calendly/GetEventTypes`
  - Retrieves available event types
- `GET /Calendly/GetEventType/{eventTypeId}`
  - Gets details for a specific event type
- `GET /Calendly/GenerateSchedulingLink/{eventTypeId}`
  - Generates a scheduling link for an event type
- `GET /Calendly/ScheduleMeeting`
  - Parameters:
    - `email`: Participant's email
    - `eventTypeId`: Event type ID

## Getting Started

1. Clone the repository
2. Update the configuration in `appsettings.json`
3. Run the following commands:
   ```bash
   dotnet restore
   dotnet build
   dotnet run
   ```

## Project Structure

- `AIGenerator.cs`: Handles AI-powered message generation
- `EmailService.cs`: Manages email sending functionality
- `CalendlyService.cs`: Handles Calendly integration
- `Controllers/`: Contains API endpoints
  - `AIGeneratorController.cs`: Main API controller
  - `CalendlyController.cs`: Calendly integration endpoints

## Dependencies

- Microsoft.AspNetCore.OpenApi
- Swashbuckle.AspNetCore
- System.Net.Mail
- Newtonsoft.Json

## Security Notes

- Store sensitive information (API keys, passwords) in secure configuration management
- Use environment variables or secure secret management in production
- Implement proper authentication and authorization for API endpoints

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details. 
