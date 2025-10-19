using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.DTO.Credentials.Telegram
{
    public sealed class TelegramCredentialData
    {
        // Bot token from BotFather: 123456789:AA...
        public required string AccessToken { get; set; }

        // Optional override (self‑hosted Telegram compat); leave null to use default.
        public string? BaseUrl { get; set; }
    }
}
