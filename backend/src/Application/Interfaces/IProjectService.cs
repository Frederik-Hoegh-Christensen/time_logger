using Core.DTOs.Project;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProjectService
    {
        Task<Guid?> CreateProjectAsync(ProjectCreateDTO projectDTO);
        Task<bool> DeleteProjectAsync(Guid id);
        Task<bool> UpdateProjectAsync(Guid id, ProjectDTO updatedProjectDTO);
        Task<ProjectDTO?> GetProjectAsync(Guid projectId);
        Task<IList<ProjectDTO>> GetProjectsByFreelancerIdAsync(Guid freelancerId);
    }
}
