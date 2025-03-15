using Core.Entities;
using Core.Interfaces;
using Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProjectRepository(ApplicationDbContext dbContext) : IProjectRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        public void CreateProject(Project project)
        {
            _dbContext.Projects.Add(project);
            _dbContext.SaveChanges();
        }

        public void DeleteProject(Guid id)
        {
            var project = _dbContext.Projects.Where(p => p.Id == id).FirstOrDefault();
            if (project != null)
            {
                _dbContext.Projects.Remove(project);
                _dbContext.SaveChanges();
            }
        }

        public Project? GetProject(Guid projectId, Guid FreelancerId)
        {
            var project = _dbContext.Projects.Where(p => p.Id == projectId && p.FreelancerId == FreelancerId).FirstOrDefault();

            if (project != null)
            {
                return project;

            }
            return null;

        }

        public ICollection<Project> GetProjectsByFreelancerId(Guid freelancerId)
        {
            var projects = _dbContext.Projects.Where(p => p.FreelancerId == freelancerId).ToList();
            if (projects != null)
            {
                return projects;
            }
            else
            {
                return [];
            }
        }

        public void UpdateProject(Guid id, Project updatedProject)
        {
            var project = _dbContext.Projects.FirstOrDefault(p => p.Id == id);

            if (project != null)
            {
                project.Name = updatedProject.Name;
                project.Client = updatedProject.Client;
                project.Deadline = updatedProject.Deadline;
                project.Client = updatedProject.Client;

                _dbContext.SaveChanges();
            }

            
        }
    }
}
