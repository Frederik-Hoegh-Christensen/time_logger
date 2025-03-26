using Application.Interfaces;
using Application.Services;
using Core.DTOs.Freelancer;
using Core.DTOs.TimeRegistration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FreelancersController(IFreelancerService freelancerService, IProjectService projectService, ITimeRegistrationService timeRegistrationService) : ControllerBase
    {
        private readonly IFreelancerService _freelancerService = freelancerService;
        private readonly IProjectService _projectService = projectService;
        private readonly ITimeRegistrationService _timeRegistrationService = timeRegistrationService;

        [HttpGet("{id}")]
        public async Task<ActionResult<FreelancerDTO>> GetFreelancer(Guid id)
        {
            var freelancerDTO = await _freelancerService.GetFreelancer(id);

            if (freelancerDTO == null)
                return NotFound();

            return Ok(freelancerDTO);
        }

        [HttpGet("{freelancerId}/timeregistrations")]
        public async Task<ActionResult<IList<TimeRegistrationDTO>>> GetTimeRegistrationsByFreelancerIdAndDate(Guid freelancerId, [FromQuery] DateOnly date)
        {
            var timeRegistrations = await _timeRegistrationService.GetTimeRegistrationsByFreelancerIdAndDate(freelancerId, date);

            if (timeRegistrations == null || !timeRegistrations.Any())
            {
                return NotFound("No time registrations found for the specified freelancer and date.");
            }

            return Ok(timeRegistrations);
        }

        [HttpGet("{freelancerId}/projects")]
        public async Task<IActionResult> GetProjectsByFreelancerId(Guid freelancerId)
        {
            var projects = await _projectService.GetProjectsByFreelancerIdAsync(freelancerId);

            if (projects == null || projects.Count == 0)
            {
                return NotFound();
            }

            return Ok(projects);
        }

        [HttpPost]
        public async Task<ActionResult> CreateFreelancer([FromBody] FreelancerCreateDTO freelancerDTO, CancellationToken cancellationToken)
        {
            if (freelancerDTO == null)
            {
                return BadRequest("Freelancer data is required");
            }

            var createdFreelancerId = await _freelancerService.CreateFreelancer(freelancerDTO, cancellationToken);

            return CreatedAtAction(nameof(GetFreelancer), new { id = createdFreelancerId });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateFreelancer(Guid id, [FromBody] FreelancerDTO freelancerDTO)
        {
            if (freelancerDTO == null)
            {
                return BadRequest("Freelancer data is required");
            }

            var existingFreelancer = await _freelancerService.GetFreelancer(id);
            if (existingFreelancer == null)
            {
                return NotFound();
            }

            await _freelancerService.UpdateFreelancer(id, freelancerDTO);
            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFreelancer(Guid id)
        {
            var freelancer = await _freelancerService.GetFreelancer(id);
            if (freelancer == null)
            {
                return NotFound();
            }

            await _freelancerService.DeleteFreelancer(id);
            return NoContent();
        }

    }
}
