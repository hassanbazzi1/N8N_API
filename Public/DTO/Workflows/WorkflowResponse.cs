using System.Collections.Generic;

namespace Public.DTO.Workflows
{
    public class WorkflowResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public List<WorkflowNode> Nodes { get; set; }  // <- Change from object to List<WorkflowNode>
        public object Connections { get; set; } // <- Change from object
        public List<string>? Tags { get; set; }
        public string? Description { get; set; }
        public object Settings { get; set; }
    }

}