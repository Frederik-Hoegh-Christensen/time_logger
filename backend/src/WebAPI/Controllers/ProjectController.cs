using Core.DTOs.Project;
using Application.Interfaces;
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
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        // GET api/projects/{projectId}
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

        [HttpGet("freelancer/{freelancerId}")]
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
        public async Task<IActionResult> CreateProject([FromBody] ProjectCreateDTO project)
        {
            if (project == null)
            {
                return BadRequest("Project cannot be null.");
            }

            await _projectService.CreateProjectAsync(project);

            return Ok();
            //return CreatedAtAction(nameof(GetProject), new { projectId = project.Id }, project);
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
