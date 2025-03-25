using Core.DTOs.Project;
using Core.DTOs.TimeRegistration;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IProjectRepository
    {
        Task<Guid?> CreateProjectAsync(ProjectCreateDTO project);
        Task<bool> DeleteProjectAsync(Guid projectId);
        Task<bool> UpdateProjectAsync(Guid projectId, ProjectDTO updatedProject);
        Task<ProjectDTO?> GetProjectAsync(Guid projectId);
        Task<ICollection<ProjectDTO>> GetProjectsByFreelancerIdAsync(Guid freelancerId);
       

    }
}
