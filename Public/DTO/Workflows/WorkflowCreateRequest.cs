using System.Text.Json.Serialization;

namespace Public.DTO.Workflows;

public class WorkflowCreateRequest
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;

    [JsonPropertyName("nodes")]
    public List<WorkflowNode> Nodes { get; set; } = new();

    [JsonPropertyName("connections")]
    public Dictionary<string, object> Connections { get; set; } = new();

    [JsonPropertyName("settings")]
    public WorkflowSettingsDto? Settings { get; set; }

}   
