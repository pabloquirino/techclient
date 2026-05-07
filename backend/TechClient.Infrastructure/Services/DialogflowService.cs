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

    public DialogflowService(IConfiguration configuration)
    {
        _projectId = configuration["Dialogflow:ProjectId"]!;
        _agentId = configuration["Dialogflow:AgentId"]!;
        _location = configuration["Dialogflow:Location"]!;

        var externalCredentials = System.Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
        if (string.IsNullOrEmpty(externalCredentials))
        {
            var credentialsPath = configuration["Dialogflow:CredentialsPath"]!;
            System.Environment.SetEnvironmentVariable(
                "GOOGLE_APPLICATION_CREDENTIALS",
                Path.Combine(AppContext.BaseDirectory, credentialsPath)
            );
        }
    }

    public async Task<ChatResponse> SendMessageAsync(ChatRequest request)
{
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
    var queryResult = response.QueryResult;

    var replyText = string.Join(" ", queryResult.ResponseMessages
        .Where(m => m.Text != null)
        .SelectMany(m => m.Text.Text_));

    var match = queryResult.Match;
    var intentName = match?.Intent?.DisplayName;
    var confidence = (float)(match?.Confidence ?? 0f);

    var currentPage = queryResult.CurrentPage?.DisplayName ?? "";
    bool hasActiveFlow = !string.IsNullOrEmpty(currentPage)
        && currentPage != "Start Page"
        && currentPage != "END_SESSION";

    var protectedIntents = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "preciso de suporte",
        "qual status do meu chamado",
        "falar com atendente"
    };

    bool isProtectedIntent = !string.IsNullOrEmpty(intentName)
        && protectedIntents.Contains(intentName);

    bool isFallback = !isProtectedIntent && (
        string.IsNullOrWhiteSpace(replyText)
        || string.IsNullOrWhiteSpace(intentName)
        || intentName.StartsWith("sys.no-match", StringComparison.OrdinalIgnoreCase)
        || intentName.Equals("Default Fallback Intent", StringComparison.OrdinalIgnoreCase)
    );

    return new ChatResponse
    {
        SessionId = request.SessionId,
        Message = request.Message,
        Reply = replyText,
        Intent = intentName,
        IntentConfidence = confidence,
        HasActiveFlow = hasActiveFlow || isProtectedIntent, 
        IsFallback = isFallback
    };
}
}