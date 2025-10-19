using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.DTO.Credentials.Google
{
    public record GoogleState(string CredentialId, string ClientId, string ClientSecret, string[] Scopes);
}