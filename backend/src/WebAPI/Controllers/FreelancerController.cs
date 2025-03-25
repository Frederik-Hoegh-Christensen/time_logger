using Core.DTOs.Freelancer;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FreelancerController(IFreelancerService freelancerService) : ControllerBase
    {
        private readonly IFreelancerService _freelancerService = freelancerService;

        [HttpGet("get/{id}")]
        public async Task<ActionResult<FreelancerDTO>> GetFreelancer(Guid id)
        {
            var freelancerDTO = await _freelancerService.GetFreelancer(id);

            if (freelancerDTO == null)
                return NotFound();

            return Ok(freelancerDTO);
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateFreelancer([FromBody] FreelancerCreateDTO freelancerDTO, CancellationToken cancellationToken  )
        {
            if (freelancerDTO == null)
            {
                return BadRequest("Freelancer data is required");
            }

            await _freelancerService.CreateFreelancer(freelancerDTO, cancellationToken);
            //TODO find retur type?
            return Ok();
        }

        [HttpPut("update/{id}")]
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
            return NoContent(); // 204 No Content, successful update with no body
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteFreelancer(Guid id)
        {
            var freelancer = await _freelancerService.GetFreelancer(id);
            if (freelancer == null)
            {
                return NotFound();
            }

            await _freelancerService.DeleteFreelancer(id);
            return NoContent(); // 204 No Content, successful deletion
        }

    }
}
