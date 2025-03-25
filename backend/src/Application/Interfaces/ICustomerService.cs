using Core.DTOs.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICustomerService
    {
        Task<Guid?> CreateCustomer(CustomerCreateDTO customerCreateDTO);
        Task<CustomerDTO> GetCustomer(Guid customerId);
        Task DeleteCustomer(Guid customerId);
        Task UpdateCustomer(CustomerDTO customerDTO);
    }
}
