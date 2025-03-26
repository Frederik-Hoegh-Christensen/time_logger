using Application.Interfaces;
using Core.DTOs.Customer;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CustomerService(ICustomerRepository customerRepository) : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;
        public async Task<Guid?> CreateCustomer(CustomerCreateDTO customerCreateDTO)
        {
            var customerId = await _customerRepository.CreateCustomer(customerCreateDTO);
            if (customerId == null)
            {
                return null;
            }
            return customerId;
        }

        public async Task DeleteCustomer(Guid customerId)
        {
            await _customerRepository.DeleteCustomer(customerId);
        }

        public async Task<CustomerDTO?> GetCustomer(Guid customerId)
        {
            return await _customerRepository.GetCustomer(customerId);
        }

        public async Task UpdateCustomer(CustomerDTO customerDTO)
        {
            await _customerRepository.UpdateCustomer(customerDTO);
        }
    }
}
