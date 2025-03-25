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
    public class CustomerController(ICustomerService customerService) : ControllerBase
    {
        private readonly ICustomerService _customerService = customerService;

        [HttpGet("get/{id}")]
        public async Task<ActionResult<CustomerDTO>> GetCustomer(Guid id)
        {
            var customerDTO = await _customerService.GetCustomer(id);

            if (customerDTO == null)
                return NotFound();

            return Ok(customerDTO);
        }

        [HttpPost("create")]
        public async Task<ActionResult<Guid>> CreateCustomer([FromBody] CustomerCreateDTO customerDTO, CancellationToken cancellationToken)
        {
            if (customerDTO == null)
                return BadRequest("Customer data is required");

            var customerId = await _customerService.CreateCustomer(customerDTO);
            return Ok(customerId);
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateCustomer([FromBody] CustomerDTO customerDTO)
        {
            if (customerDTO == null)
                return BadRequest("Customer data is required");

            await _customerService.UpdateCustomer(customerDTO);
            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteCustomer(Guid id)
        {
            await _customerService.DeleteCustomer(id);
            return NoContent();
        }
    }
}
