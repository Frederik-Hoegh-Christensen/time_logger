using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Customer
{
    public class CustomerCreateDTO
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
}
