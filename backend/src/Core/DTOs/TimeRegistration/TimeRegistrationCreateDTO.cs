using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.TimeRegistration
{
    public class TimeRegistrationCreateDTO
    {
        public Guid ProjectId { get; set; }
        public Guid FreelancerId { get; set; }
        public DateOnly WorkDate { get; set; }
        public decimal HoursWorked { get; set; }
        public string Description { get; set; }
    }
}
