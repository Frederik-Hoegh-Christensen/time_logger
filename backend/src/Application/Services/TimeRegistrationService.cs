using Core.DTOs.Project;
using Core.DTOs.TimeRegistration;
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
    public class TimeRegistrationService(IProjectService projectService, ITimeRegistrationRepository timeRegistrationRepository) : ITimeRegistrationService
    {
        private readonly ITimeRegistrationRepository _timeRegistrationRepository = timeRegistrationRepository;
        private readonly IProjectService _projectService = projectService;
        public async Task<Guid?> CreateTimeRegistrationAsync(TimeRegistrationCreateDTO timeRegistrationCreateDTO)
        {
            if (timeRegistrationCreateDTO.HoursWorked < 0.5m || timeRegistrationCreateDTO.HoursWorked >= 24m)
            {
                return null;
            }
            var project = await _projectService.GetProjectAsync(timeRegistrationCreateDTO.ProjectId);
            if (project == null || project.IsCompleted)
            {
                return null;
            }

            var id = await _timeRegistrationRepository.CreateTimeRegistrationAsync(timeRegistrationCreateDTO);
            return id;
        }

        public async Task<bool> DeleteTimeRegistrationAsync(Guid timeRegistrationId)
        {
            var deleted = await _timeRegistrationRepository.DeleteTimeRegistrationAsync(timeRegistrationId);
            return deleted;
        }

        public async Task<TimeRegistrationDTO?> GetTimeRegistrationAsync(Guid timeRegistrationId)
        {
            var timeRegistration = await _timeRegistrationRepository.GetTimeRegistrationAsync(timeRegistrationId);
            return timeRegistration;
        }

        public async Task<bool> UpdateTimeRegistrationAsync(Guid timeRegistrationId, TimeRegistrationDTO updatedTimeRegistration)
        {
            if (updatedTimeRegistration.HoursWorked < 0.5m || updatedTimeRegistration.HoursWorked >= 24m)
            {
                return false;
            }
            var updated = await _timeRegistrationRepository.UpdateTimeRegistrationAsync(timeRegistrationId, updatedTimeRegistration);
            return updated;
        }

        public async Task<IList<TimeRegistrationDTO>> GetTimeRegistrationsForProjectAsync(Guid projectId)
        {
            var timeRegistrations = await _timeRegistrationRepository.GetTimeRegistrationsForProjectAsync(projectId);
            return timeRegistrations;
            
        }

        public async Task<IList<TimeRegistrationDTO>> GetTimeRegistrationsByFreelancerIdAndDate(Guid freelancerId, DateOnly date)
        {
            var timeRegistrations = await _timeRegistrationRepository.GetTimeRegistrationsByFreelancerIdAndDate(freelancerId, date);
            return timeRegistrations;
        }

    }
}
