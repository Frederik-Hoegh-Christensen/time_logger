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
        private readonly ILogger<CustomerRepository> _logger;

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
                //_logger.LogError(ex, "Failed to create customer.");
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
                //_logger.LogError(ex, "Failed to fetch customer.");
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
                   // _logger.LogWarning("Customer not found: {CustomerId}", customerDTO.Id);
                    return false;
                }

                _mapper.Map(customerDTO, existingCustomer);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Failed to update customer.");
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
                    //_logger.LogWarning("Customer not found: {CustomerId}", customerId);
                    return false;
                }

                _dbContext.Customers.Remove(customer);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Failed to delete customer.");
                return false;
            }
        }
    }
}
