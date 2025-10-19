using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.DTO.Credentials.Airtable
{
    public sealed class AirtableCredentialData
    {
        // Airtable Personal Access Token OR legacy API key
        public required string ApiKey { get; set; }
    }
}
