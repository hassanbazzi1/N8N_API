using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.DTO.Credentials.LLM.Gemini
{
    public class GeminiCredentialData
    {
        public string ApiKey { get; set; }
        public string Host { get; set; } = "https://generativelanguage.googleapis.com";
    }
}
