namespace Public.DTO.Workflows
{
    public class WorkflowUpdateRequest
    {
        public string Name { get; set; }                // required
        public List<WorkflowNode> Nodes { get; set; }         // required, as array
        public object Connections { get; set; }         // required
        public object Settings { get; set; } = new();   // required (even if empty)
    }
}
