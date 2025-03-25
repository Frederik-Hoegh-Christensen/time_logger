using AutoMapper;
using Core.DTOs.Project;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProjectRepository(ApplicationDbContext dbContext, IMapper mapper) : IProjectRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<ProjectRepository> _logger;

        public async Task<Guid?> CreateProjectAsync(ProjectCreateDTO projectDTO)
        {
            try
            {
                var project = _mapper.Map<Project>(projectDTO);
                await _dbContext.Projects.AddAsync(project);
                await _dbContext.SaveChangesAsync();
                return project.Id;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Failed to create project.");
                return null;
            }
        }

        public async Task<ProjectDTO?> GetProjectAsync(Guid projectId)
        {
            try
            {
                var project = await _dbContext.Projects
                    .AsNoTracking()
                    .Include(p => p.Customer) // Ensuring Customer details are included
                    .FirstOrDefaultAsync(p => p.Id == projectId);

                return project is not null ? _mapper.Map<ProjectDTO>(project) : null;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Failed to fetch project.");
                return null;
            }
        }

        public async Task<ICollection<ProjectDTO>> GetProjectsByFreelancerIdAsync(Guid freelancerId)
        {
            try
            {
                var projects = await _dbContext.Projects
                    .AsNoTracking()
                    .Include(p => p.Customer)
                    .Where(p => p.FreelancerId == freelancerId)
                    .ToListAsync();

                return _mapper.Map<List<ProjectDTO>>(projects);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Failed to fetch projects for freelancer {FreelancerId}", freelancerId);
                return [];
            }
        }

        public async Task<bool> UpdateProjectAsync(Guid projectId, ProjectDTO updatedProjectDTO)
        {
            try
            {
                var project = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
                if (project is null)
                {
                    //_logger.LogWarning("Project not found: {ProjectId}", projectId);
                    return false;
                }

                _mapper.Map(updatedProjectDTO, project);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Failed to update project.");
                return false;
            }
        }

        public async Task<bool> DeleteProjectAsync(Guid projectId)
        {
            try
            {
                var project = await _dbContext.Projects.FindAsync(projectId);
                if (project is null)
                {
                    //_logger.LogWarning("Project not found: {ProjectId}", projectId);
                    return false;
                }

                _dbContext.Projects.Remove(project);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Failed to delete project.");
                return false;
            }
        }
    }
}
