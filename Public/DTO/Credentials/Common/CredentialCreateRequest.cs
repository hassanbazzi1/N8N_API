using Public.DTO.Credentials.Airtable;
using Public.DTO.Credentials.Google;
using Public.DTO.Credentials.LLM.DeepSeek;
using Public.DTO.Credentials.LLM.Gemini;
using Public.DTO.Credentials.LLM.OpenAI;
using Public.DTO.Credentials.SendGrid;
using Public.DTO.Credentials.Telegram;
using Public.DTO.Credentials.Twilio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.DTO.Credentials.Common
{
    public class CredentialCreateRequest
    {
        //Common 
        public string Name { get; set; }
        public CredentialProvider Provider { get; set; }

        // OpenAI Data
        public OpenAICredentialData? OpenAIData { get; set; }
        //DeepSeek Data
        public DeepSeekCredentialData? DeepSeekData { get; set; }
        //Gemini Data
        public GeminiCredentialData? GeminiData { get; set; }
        //Google Oauth Data
        public  GoogleCredentialData? GoogleData { get; set; }
        //Gmail
        public GmailCredentialData? GmailData { get; set; }
        //Twilio Data
        public TwilioCredentialData? TwilioData { get; set; }
        //SendGrid Data
        public SendGridCredentialData? SendGridData { get; set; }
        //Telegram Data
        public TelegramCredentialData? TelegramData { get; set; }
        public AirtableCredentialData? AirtableData { get; set; }
    }
}
