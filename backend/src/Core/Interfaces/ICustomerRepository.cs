using Core.DTOs.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Guid?> CreateCustomer(CustomerCreateDTO customerCreateDTO);
        Task<CustomerDTO?> GetCustomer(Guid CustomerId);
        Task<bool> DeleteCustomer(Guid CustomerId);
        Task<bool> UpdateCustomer(CustomerDTO customerDTO);
    }
}
