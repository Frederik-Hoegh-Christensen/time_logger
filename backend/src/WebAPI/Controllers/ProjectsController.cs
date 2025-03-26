using Application.Interfaces;
using Application.Services;
using Core.DTOs.Project;
using Core.DTOs.TimeRegistration;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ITimeRegistrationService _timeRegistrationService;

        public ProjectsController(IProjectService projectService, ITimeRegistrationService timeRegistrationService  )
        {
            _projectService = projectService;
            _timeRegistrationService = timeRegistrationService;
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetProject(Guid projectId)
        {
            var project = await _projectService.GetProjectAsync(projectId);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        [HttpGet("{projectId}/timeregistrations")]
        public async Task<ActionResult<IList<TimeRegistrationDTO>>> GetTimeRegistrationsForProject(Guid projectId)
        {
            var timeRegistrations = await _timeRegistrationService.GetTimeRegistrationsForProjectAsync(projectId);

            if (timeRegistrations == null || !timeRegistrations.Any())
            {
                return NoContent();
            }

            return Ok(timeRegistrations);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] ProjectCreateDTO project)
        {
            if (project == null)
            {
                return BadRequest("Project cannot be null.");
            }

            var createdProjectId = await _projectService.CreateProjectAsync(project);
            return CreatedAtAction(nameof(GetProject), new { projectId = createdProjectId }, createdProjectId);
        }

        [HttpPut("{projectId}")]
        public async Task<IActionResult> UpdateProject(Guid projectId, [FromBody] ProjectDTO updatedProject)
        {
            if (updatedProject == null)
            {
                return BadRequest("Updated project cannot be null.");
            }

            var existingProject = await _projectService.GetProjectAsync(projectId);

            if (existingProject == null)
            {
                return NotFound();
            }

            await _projectService.UpdateProjectAsync(projectId, updatedProject);

            return NoContent();
        }

        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteProject(Guid projectId)
        {
            var project = await _projectService.GetProjectAsync(projectId);

            if (project == null)
            {
                return NotFound();
            }

            await _projectService.DeleteProjectAsync(projectId);

            return NoContent();
        }
    }
}
