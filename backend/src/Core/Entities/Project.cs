﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        [Required]
        public Guid FreelancerId { get; set; }
        public string Name { get; set; }
        public string Client { get; set; }
        public DateTime Deadline { get; set; }
        public Freelancer Freelancer { get; set; }
        public ICollection<TimeRegistration> TimeRegistrations { get; set; }
    }
}
