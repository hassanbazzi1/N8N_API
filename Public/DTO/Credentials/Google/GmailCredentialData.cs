using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.DTO.Credentials.Google
{
    public sealed class GmailCredentialData
    {
        public required string ClientId { get; init; }
        public required string ClientSecret { get; init; }

        // Gmail modify is the most useful scope (read, send, label, delete)
        // Use all main Gmail scopes by default if not provided
        public string Scope { get; init; } =
            "https://mail.google.com/ " +
            "https://www.googleapis.com/auth/gmail.labels " +
            "https://www.googleapis.com/auth/gmail.send " +
            "https://www.googleapis.com/auth/gmail.compose " +
            "https://www.googleapis.com/auth/gmail.modify " +
            "https://www.googleapis.com/auth/gmail.addons.current.action.compose " +
            "https://www.googleapis.com/auth/gmail.addons.current.message.action " +
            "https://www.googleapis.com/auth/gmail.addons.current.message.readonly";
    }
}
