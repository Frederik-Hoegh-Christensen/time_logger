using AutoMapper;
using Core.DTOs.TimeRegistration;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TimeRegistrationRepository(ApplicationDbContext dbContext, IMapper mapper) : ITimeRegistrationRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;

        public async Task<Guid?> CreateTimeRegistrationAsync(TimeRegistrationCreateDTO timeRegistrationDTO)
        {
            try
            {
                var timeRegistration = _mapper.Map<TimeRegistration>(timeRegistrationDTO);
                if (timeRegistration == null)
                    return null;

                await _dbContext.TimeRegistrations.AddAsync(timeRegistration);
                await _dbContext.SaveChangesAsync();
                return timeRegistration.Id;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeleteTimeRegistrationAsync(Guid id)
        {
            try
            {
                var timeRegistration = await _dbContext.TimeRegistrations.FirstOrDefaultAsync(tr => tr.Id == id);
                if (timeRegistration == null)
                    return false;

                _dbContext.TimeRegistrations.Remove(timeRegistration);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting time registration: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateTimeRegistrationAsync(Guid id, TimeRegistrationDTO updatedTimeRegistration)
        {
            if (updatedTimeRegistration == null)
                return false;

            try
            {
                var existingTimeRegistration = await _dbContext.TimeRegistrations.FirstOrDefaultAsync(tr => tr.Id == id);
                if (existingTimeRegistration == null)
                {
                    return false;
                }
                   
                existingTimeRegistration.HoursWorked = updatedTimeRegistration.HoursWorked;
                existingTimeRegistration.Description = updatedTimeRegistration.Description;

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating time registration: {ex.Message}");
                return false;
            }
        }

        public async Task<TimeRegistrationDTO?> GetTimeRegistrationAsync(Guid id)
        {
            try
            {
                var timeRegistration = await _dbContext.TimeRegistrations.FirstOrDefaultAsync(tr => tr.Id == id);
                return _mapper.Map<TimeRegistrationDTO>(timeRegistration);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IList<TimeRegistrationDTO>> GetTimeRegistrationsForProjectAsync(Guid projectId)
        {
            try
            {
                var timeRegistrations = await _dbContext.TimeRegistrations
                    .Where(tr => tr.ProjectId == projectId)
                    .ToListAsync();

                return _mapper.Map<List<TimeRegistrationDTO>>(timeRegistrations) ?? new List<TimeRegistrationDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching time registrations for project: {ex.Message}");
                return new List<TimeRegistrationDTO>();
            }
        }

        public async Task<IList<TimeRegistrationDTO>> GetTimeRegistrationsByFreelancerIdAndDate(Guid freelancerId, DateOnly date)
        {
            try
            {
                var timeRegistrations = await _dbContext.TimeRegistrations
                    .Where(tr => tr.FreelancerId == freelancerId && tr.WorkDate == date)
                    .Include(tr => tr.Project)
                    .ToListAsync();

                return _mapper.Map<List<TimeRegistrationDTO>>(timeRegistrations) ?? new List<TimeRegistrationDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching time registrations by freelancer and date: {ex.Message}");
                return new List<TimeRegistrationDTO>();
            }
        }
    }
}
