using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.DTOs.Project
{
    public class ProjectCreateDTO
    {
        [JsonPropertyName("freelancerId")]
        public Guid FreelancerId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("deadline")]
        public DateTime Deadline { get; set; }
        [JsonPropertyName("customerId")]
        public Guid CustomerId { get; set; }
        [JsonPropertyName("isCompleted")]
        public bool IsCompleted { get; set; }
    }
}
