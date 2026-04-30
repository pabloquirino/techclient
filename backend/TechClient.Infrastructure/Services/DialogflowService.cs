using Google.Cloud.Dialogflow.Cx.V3;
using Microsoft.Extensions.Configuration;
using TechClient.Domain.Interfaces;
using TechClient.Domain.Models;

namespace TechClient.Infrastructure.Services;

public class DialogflowService : IChatService
{
    private readonly string _projectId;
    private readonly string _agentId;
    private readonly string _location;
    private readonly string _credentialsPath;

    public DialogflowService(IConfiguration configuration)
    {
        _projectId = configuration["Dialogflow:ProjectId"]!;
        _agentId = configuration["Dialogflow:AgentId"]!;
        _location = configuration["Dialogflow:Location"]!;
        _credentialsPath = configuration["Dialogflow:CredentialsPath"]!;
    }

    public async Task<ChatResponse> SendMessageAsync(ChatRequest request)
    {
        System.Environment.SetEnvironmentVariable(
            "GOOGLE_APPLICATION_CREDENTIALS",
            Path.Combine(AppContext.BaseDirectory, _credentialsPath)
        );

        var sessionName = SessionName.FromProjectLocationAgentSession(
            _projectId, _location, _agentId, request.SessionId
        );

        var clientBuilder = new SessionsClientBuilder
        {
            Endpoint = $"{_location}-dialogflow.googleapis.com"
        };

        var client = await clientBuilder.BuildAsync();

        var detectRequest = new DetectIntentRequest
        {
            Session = sessionName.ToString(),
            QueryInput = new QueryInput
            {
                Text = new TextInput { Text = request.Message },
                LanguageCode = "pt-BR"
            }
        };

        var response = await client.DetectIntentAsync(detectRequest);

        var replyText = string.Join(" ", response.QueryResult.ResponseMessages
            .Where(m => m.Text != null)
            .SelectMany(m => m.Text.Text_));

        var intentName = response.QueryResult.Match?.Intent?.DisplayName;
        var isFallback = string.IsNullOrWhiteSpace(replyText) ||
                         intentName == null ||
                         intentName.Contains("sys.no-match");

        return new ChatResponse
        {
            SessionId = request.SessionId,
            Message = request.Message,
            Reply = replyText,
            Intent = intentName,
            IsFallback = isFallback
        };
    }
}