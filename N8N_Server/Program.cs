using Google_OAuth2.Factory.Credential;
using Google_OAuth2.Helpers;
using Google_OAuth2.Services.Credentials.Implementations.Airtable;
using Google_OAuth2.Services.Credentials.Implementations.DeepSeek;
using Google_OAuth2.Services.Credentials.Implementations.Gemini;
using Google_OAuth2.Services.Credentials.Implementations.Google;
using Google_OAuth2.Services.Credentials.Implementations.OpenAi;
using Google_OAuth2.Services.Credentials.Implementations.SendGrid;
using Google_OAuth2.Services.Credentials.Implementations.Telegram;
using Google_OAuth2.Services.Credentials.Implementations.Twilio;
using Google_OAuth2.Services.Workflows;
using Microsoft.EntityFrameworkCore;

using N8N_API.Entities;

using System.Net;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Singleton HttpClient with session cookie for credentials endpoints
builder.Services.AddSingleton<HttpClient>(_ =>
{
    var cookies  = new CookieContainer();
    // ? disable auto?redirect here
    var handler  = new HttpClientHandler
    {
        CookieContainer   = cookies,
        AllowAutoRedirect = false
    };

    var client = new HttpClient(handler)
    {
        BaseAddress = new Uri("https://n8n.srv898423.hstgr.cloud")
    };

    var loginBody = new StringContent(
        "{\"emailOrLdapLoginId\":\"karimgsaikali2@gmail.com\",\"password\":\"Hostinger1801@\"}",
        Encoding.UTF8,
        "application/json");

    client.PostAsync("/rest/login", loginBody).GetAwaiter().GetResult()
          .EnsureSuccessStatusCode();

    return client;          // this client now keeps the cookies & never auto?redirects
});
// Register all credential services using the singleton HttpClient
builder.Services.AddScoped<GoogleOAuth2CredentialService>();
builder.Services.AddScoped<SendGridCredentialService>();
builder.Services.AddScoped<TwilioCredentialService>();
builder.Services.AddScoped<TelegramCredentialService>();
builder.Services.AddScoped<AirtableCredentialService>();
builder.Services.AddScoped<OpenAICredentialService>();
builder.Services.AddScoped<DeepSeekCredentialService>();
builder.Services.AddScoped<GeminiCredentialService>();
builder.Services.AddScoped<ICredentialServiceFactory, CredentialServiceFactory>();

// Register WorkflowService with its own HttpClient (API key only)
builder.Services.AddHttpClient<IWorkflowService, WorkflowService>(client =>
{
    client.BaseAddress = new Uri("https://n8n.srv898423.hstgr.cloud");
    client.DefaultRequestHeaders.Add("X-N8N-API-KEY","eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI0NTQzOTYwNS01NWZkLTQ5Y2QtODY3Zi05ZjZjOTNiYmJiNTMiLCJpc3MiOiJuOG4iLCJhdWQiOiJwdWJsaWMtYXBpIiwiaWF0IjoxNzUzMzUxNDU5LCJleHAiOjE3NTU5MDAwMDB9.AsL_jhO0OG5qJatgb5XrLZQX1KrhLfOZkEMJ1_nxVH0"); client.DefaultRequestHeaders.Accept.Add(new("application/json"));
});

var app = builder.Build();

app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
