using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Public.DTO.Workflows
{
    public class WorkflowNode
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = default!;

        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("type")]
        public string Type { get; set; } = default!;

        [JsonPropertyName("typeVersion")]
        public int TypeVersion { get; set; }

        [JsonPropertyName("parameters")]
        public object Parameters { get; set; } = new { };

        [JsonPropertyName("position")]
        public double[] Position { get; set; } = default!;

        [JsonPropertyName("credentials")]
        public object? Credentials { get; set; }
    }
}
    