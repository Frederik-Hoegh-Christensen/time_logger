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
        void CreateProject(Project project);
        void DeleteProject(Guid id);
        void UpdateProject(Guid id, Project updatedProject);
        Project GetProject(Guid projectId, Guid FreelancerId);
        ICollection<Project> GetProjectsByFreelancerId(Guid freelancerId);

    }
}
