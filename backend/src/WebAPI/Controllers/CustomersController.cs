using Application.Interfaces;
using Core.DTOs.Customer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController(ICustomerService customerService) : ControllerBase
    {
        private readonly ICustomerService _customerService = customerService;

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDTO>> GetCustomer(Guid id)
        {
            var customerDTO = await _customerService.GetCustomer(id);

            if (customerDTO == null)
                return NotFound();

            return Ok(customerDTO);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateCustomer([FromBody] CustomerCreateDTO customerDTO)
        {
            if (customerDTO == null)
                return BadRequest("Customer data is required");

            var customerId = await _customerService.CreateCustomer(customerDTO);

            return CreatedAtAction(nameof(GetCustomer), new { id = customerId }, customerId);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCustomer(Guid id, [FromBody] CustomerDTO customerDTO)
        {
            if (customerDTO == null)
                return BadRequest("Customer data is required");

            if (id != customerDTO.Id) 
                return BadRequest("Mismatched customer ID");

            await _customerService.UpdateCustomer(customerDTO);
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCustomer(Guid id)
        {
            await _customerService.DeleteCustomer(id);
            return NoContent();
        }
    }
}
