using Core.DTOs.Project;
using Application.Interfaces;
using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProjectService(IProjectRepository projectRepository) : IProjectService
    {
        private readonly IProjectRepository _projectRepository = projectRepository;
        public async Task<Guid?> CreateProjectAsync(ProjectCreateDTO projectDTO)
        {
            if (projectDTO.Deadline < DateTime.Today)
            {
                return null;
            }
            var id = await _projectRepository.CreateProjectAsync(projectDTO);
            return id;
        }

        public async Task<bool> DeleteProjectAsync(Guid id)
        {
            var deleted = await _projectRepository.DeleteProjectAsync(id);
            return deleted;
        }

        public async Task<ProjectDTO?> GetProjectAsync(Guid projectId)
        {
            var project  = await _projectRepository.GetProjectAsync(projectId);
            return project;
        }

        public async Task<ICollection<ProjectDTO>> GetProjectsByFreelancerIdAsync(Guid freelancerId)
        {
            var projects = await _projectRepository.GetProjectsByFreelancerIdAsync(freelancerId);
            return projects;
        }

        public async Task<bool> UpdateProjectAsync(Guid projectId, ProjectDTO updatedProject)
        {
            if (updatedProject.Deadline < (DateTime.Today))
            {
                return false;
            }

            var project = await _projectRepository.GetProjectAsync(projectId);
            if (project == null)
            {
                return false;

            }
            var updated = await _projectRepository.UpdateProjectAsync(projectId, updatedProject);
            return updated;
        }
    }
}
