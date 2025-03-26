using AutoMapper;
using Core.DTOs.Customer;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class CustomerRepository(ApplicationDbContext dbContext, IMapper mapper) : ICustomerRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;

        public async Task<Guid?> CreateCustomer(CustomerCreateDTO customerCreateDTO)
        {
            try
            {
                var customer = _mapper.Map<Customer>(customerCreateDTO);
                await _dbContext.Customers.AddAsync(customer);
                await _dbContext.SaveChangesAsync();
                return customer.Id;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<CustomerDTO?> GetCustomer(Guid customerId)
        {
            try
            {
                var customer = await _dbContext.Customers
                    .AsNoTracking()
                    .FirstAsync(c => c.Id == customerId);

                return _mapper.Map<CustomerDTO>(customer);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> UpdateCustomer(CustomerDTO customerDTO)
        {
            try
            {
                var existingCustomer = await _dbContext.Customers.FindAsync(customerDTO.Id);
                if (existingCustomer is null)
                {
                    return false;
                }

                _mapper.Map(customerDTO, existingCustomer);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteCustomer(Guid customerId)
        {
            try
            {
                var customer = await _dbContext.Customers.FindAsync(customerId);
                if (customer is null)
                {
                    return false;
                }

                _dbContext.Customers.Remove(customer);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
