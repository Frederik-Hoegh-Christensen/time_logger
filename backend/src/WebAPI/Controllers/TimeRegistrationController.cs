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
    public class TimeRegistrationController : ControllerBase
    {
        private readonly ITimeRegistrationService _timeRegistrationService;

        public TimeRegistrationController(ITimeRegistrationService timeRegistrationService)
        {
            _timeRegistrationService = timeRegistrationService;
        }

        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<IList<TimeRegistrationDTO>>> GetTimeRegistrationsForProject(Guid projectId)
        {
            // Retrieve the time registrations for the project using the service
            var timeRegistrations = await _timeRegistrationService.GetTimeRegistrationsForProjectAsync(projectId);

            // Check if there are any time registrations returned
            if (timeRegistrations == null || !timeRegistrations.Any())
            {
                // Return a NotFound response if no time registrations are found
                return NotFound("No time registrations found for the specified project.");
            }

            // Return the time registrations as a response
            return Ok(timeRegistrations);
        }

        [HttpGet("freelancer/{freelancerId}/date/{date}")]
        public async Task<ActionResult<IList<TimeRegistrationDTO>>> GetTimeRegistrationsByFreelancerIdAndDate(Guid freelancerId, DateOnly date)
        {
            // Retrieve the time registrations for the project using the service
            var timeRegistrations = await _timeRegistrationService.GetTimeRegistrationsByFreelancerIdAndDate(freelancerId, date);

            // Check if there are any time registrations returned
            if (timeRegistrations == null || !timeRegistrations.Any())
            {
                // Return a NotFound response if no time registrations are found
                return NotFound("No time registrations found for the specified project.");
            }

            // Return the time registrations as a response
            return Ok(timeRegistrations);
        }

        // GET api/timeregistrations/{timeRegistrationId}
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

        // POST api/timeregistrations
        [HttpPost]
        public async Task<IActionResult> CreateTimeRegistration([FromBody] TimeRegistrationCreateDTO timeRegistrationCreateDTO)
        {
            if (timeRegistrationCreateDTO == null)
            {
                return BadRequest("Time registration cannot be null.");
            }

            await _timeRegistrationService.CreateTimeRegistrationAsync(timeRegistrationCreateDTO);

            return Ok();
            //return CreatedAtAction(nameof(GetTimeRegistration), new { timeRegistrationId = timeRegistrationCreateDTO.Id }, timeRegistrationCreateDTO);
        }

        // PUT api/timeregistrations/{timeRegistrationId}
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

        // DELETE api/timeregistrations/{timeRegistrationId}
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
