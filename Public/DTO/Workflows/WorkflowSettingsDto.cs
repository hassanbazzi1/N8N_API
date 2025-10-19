using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Public.DTO.Workflows
{
    public class WorkflowSettingsDto
    {
        [JsonPropertyName("saveExecutionProgress")]
        public bool? SaveExecutionProgress { get; set; }
    }
}
