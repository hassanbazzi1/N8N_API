using Google_OAuth2.Services.Credentials.Implementations.Airtable;
using Google_OAuth2.Services.Credentials.Implementations.DeepSeek;
using Google_OAuth2.Services.Credentials.Implementations.Gemini;
using Google_OAuth2.Services.Credentials.Implementations.Google;
using Google_OAuth2.Services.Credentials.Implementations.OpenAi;
using Google_OAuth2.Services.Credentials.Implementations.SendGrid;
using Google_OAuth2.Services.Credentials.Implementations.Telegram;
using Google_OAuth2.Services.Credentials.Implementations.Twilio;
using Google_OAuth2.Services.Credentials.Interface;
using Public.DTO.Credentials.Common;
using Public.DTO.Credentials.LLM.DeepSeek;

namespace Google_OAuth2.Factory.Credential
{
    public class CredentialServiceFactory : ICredentialServiceFactory
    {
        private readonly GoogleOAuth2CredentialService _google;
        private readonly SendGridCredentialService _sendGrid;
        private readonly TwilioCredentialService _twilio;
        private readonly TelegramCredentialService _telegram;
        private readonly AirtableCredentialService _airtable;
        private readonly DeepSeekCredentialService _deepSeek;
        private readonly GeminiCredentialService _gemini;
        private readonly OpenAICredentialService _openai;


        public CredentialServiceFactory(
            GoogleOAuth2CredentialService google,
            SendGridCredentialService sendGrid,
            TwilioCredentialService twilio,
            TelegramCredentialService telegram,
            AirtableCredentialService airtable,
            DeepSeekCredentialService deepSeek,
            GeminiCredentialService gemini,
            OpenAICredentialService openai
)
        {
            _google = google;
            _sendGrid = sendGrid;
            _twilio = twilio;
            _telegram = telegram;
            _airtable = airtable;
            _deepSeek = deepSeek;
            _gemini = gemini;
            _openai = openai;
        }

        public ICredentialService GetService(CredentialProvider provider) => provider switch
        {
            // All Google OAuth2 variants use the same service
           
            CredentialProvider.googleDriveOAuth2Api => _google,
            CredentialProvider.googleSheetsOAuth2Api => _google,
            CredentialProvider.googleCalendarOAuth2Api => _google,
            CredentialProvider.openAiApi => _openai,
            CredentialProvider.deepSeekApi => _deepSeek,
            CredentialProvider.googlePalmApi => _gemini,
            CredentialProvider.SendGridApi => _sendGrid,
            CredentialProvider.TwilioApi => _twilio,
            CredentialProvider.TelegramApi => _telegram,
            CredentialProvider.AirtableApi => _airtable,

            _ => throw new NotImplementedException()
        };
    }
}
