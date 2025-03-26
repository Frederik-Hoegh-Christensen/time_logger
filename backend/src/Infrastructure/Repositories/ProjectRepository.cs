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

        public async Task<Guid?> CreateProjectAsync(ProjectCreateDTO projectDTO)
        {
            try
            {
                var project = _mapper.Map<Project>(projectDTO);
                await _dbContext.Projects.AddAsync(project);
                await _dbContext.SaveChangesAsync();
                return project.Id;
            }
            catch
            {
                return null;
            }
        }

        public async Task<ProjectDTO?> GetProjectAsync(Guid projectId)
        {
            try
            {
                var project = await _dbContext.Projects
                    .AsNoTracking()
                    .Include(p => p.Customer) 
                    .FirstOrDefaultAsync(p => p.Id == projectId);

                return project is not null ? _mapper.Map<ProjectDTO>(project) : null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IList<ProjectDTO>> GetProjectsByFreelancerIdAsync(Guid freelancerId)
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
                    return false;
                }

                _mapper.Map(updatedProjectDTO, project);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
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
                    return false;
                }

                _dbContext.Projects.Remove(project);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
