using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.DTO.Credentials.Twilio
{
    public sealed class TwilioCredentialData
    {
        public TwilioAuthType AuthType { get; set; } = TwilioAuthType.AuthToken;

        // common
        public required string AccountSid { get; set; }

        // auth-token flow
        public string? AuthToken { get; set; }

        // api-key flow
        public string? ApiKeySid { get; set; }
        public string? ApiKeySecret { get; set; }
    }
}
