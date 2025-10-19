namespace N8N_API.Entities
{
    public class WorkflowEntity
    {
        public Guid Id { get; set; }
        public string ExternalId { get; set; } = default!; // n8n id
        public DateTime Created { get; set; }
    }
}
