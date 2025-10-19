using Public.DTO.Credentials.Common;

namespace N8N_API.Entities
{
    public class CredentialEntity
    {
        public Guid Id { get; set; }
        public string Provider { get; set; } = default!;
        public DateTime Created { get; set; }
        public DateTime Updated { get; internal set; }
    }
}
