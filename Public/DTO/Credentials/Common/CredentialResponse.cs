using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.DTO.Credentials.Common
{
    public class CredentialResponse
    {
        public string CredentialId { get; set; }
        public CredentialProvider Provider { get; set; }
        public bool IsAuthorised { get; set; }
        public string? Message { get; set; }
        public string? AuthUrl { get; set; } 
    }
    }
