using Public.DTO.Credentials.Airtable;
using Public.DTO.Credentials.Google;
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
    public class CredentialUpdateRequest
    {
        public GoogleCredentialData? googleOAuth2Api { get; set; }
        public GoogleCredentialData? googleDriveOAuth2Api { get; set; }
        public GoogleCredentialData? googleCalendarOAuth2Api { get; set; }
        public GoogleCredentialData? googleSheetsOAuth2Api { get; set; }

        public TelegramCredentialData? telegramApi { get; set; }
        public SendGridCredentialData? sendGridApi { get; set; }
        public TwilioCredentialData? twilioApi { get; set; }
        public AirtableCredentialData? airtableApi { get; set; }
    }
}
