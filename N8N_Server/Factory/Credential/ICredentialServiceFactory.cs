using Google_OAuth2.Services.Credentials.Interface;
using Public.DTO.Credentials.Common;

namespace Google_OAuth2.Factory.Credential
{
    public interface ICredentialServiceFactory
    {
        ICredentialService GetService(CredentialProvider provider);
    }
}
