using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.DTO.Credentials.LLM.OpenAI
{
    public class OpenAICredentialData
    {
        public string ApiKey { get; set; }
        public string? OrganizationId { get; set; }
        public string? Url { get; set; }
    }
}
