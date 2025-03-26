using Core.DTOs.TimeRegistration;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeRegistrationsController : ControllerBase
    {
        private readonly ITimeRegistrationService _timeRegistrationService;

        public TimeRegistrationsController(ITimeRegistrationService timeRegistrationService)
        {
            _timeRegistrationService = timeRegistrationService;
        }

        

        [HttpGet("{timeRegistrationId}")]
        public async Task<IActionResult> GetTimeRegistration(Guid timeRegistrationId)
        {
            var timeRegistrationDTO = await _timeRegistrationService.GetTimeRegistrationAsync(timeRegistrationId);

            if (timeRegistrationDTO == null)
            {
                return NotFound();
            }

            return Ok(timeRegistrationDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTimeRegistration([FromBody] TimeRegistrationCreateDTO timeRegistrationCreateDTO)
        {
            if (timeRegistrationCreateDTO == null)
            {
                return BadRequest("Time registration cannot be null.");
            }

            var createdTimeRegistrationId = await _timeRegistrationService.CreateTimeRegistrationAsync(timeRegistrationCreateDTO);

            // Return 201 Created with the location of the new resource
            return CreatedAtAction(nameof(GetTimeRegistration), new { timeRegistrationId = createdTimeRegistrationId }, createdTimeRegistrationId);
        }

        [HttpPut("{timeRegistrationId}")]
        public async Task<IActionResult> UpdateTimeRegistration(Guid timeRegistrationId, [FromBody] TimeRegistrationDTO updatedTimeRegistration)
        {
            if (updatedTimeRegistration == null)
            {
                return BadRequest("Updated time registration cannot be null.");
            }

            var existingTimeRegistration = await _timeRegistrationService.GetTimeRegistrationAsync(timeRegistrationId);

            if (existingTimeRegistration == null)
            {
                return NotFound();
            }

            await _timeRegistrationService.UpdateTimeRegistrationAsync(timeRegistrationId, updatedTimeRegistration);

            return NoContent();
        }

        [HttpDelete("{timeRegistrationId}")]
        public async Task<IActionResult> DeleteTimeRegistration(Guid timeRegistrationId)
        {
            var timeRegistration = await _timeRegistrationService.GetTimeRegistrationAsync(timeRegistrationId);

            if (timeRegistration == null)
            {
                return NotFound();
            }

            await _timeRegistrationService.DeleteTimeRegistrationAsync(timeRegistrationId);

            return NoContent();
        }
    }
}
