using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.DTOs.Freelancer
{
    public class FreelancerCreateDTO
    {
        [JsonPropertyName("firstName")]
        public required string FirstName { get; set; }
        [JsonPropertyName("lastName")]
        public required string LastName { get; set; }
        [JsonPropertyName("password")]
        public required string Password { get; set; }
        [JsonPropertyName("email")]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
