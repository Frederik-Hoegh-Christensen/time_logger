using Core.DTOs.TimeRegistration;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITimeRegistrationService
    {
        Task<Guid?> CreateTimeRegistrationAsync(TimeRegistrationCreateDTO timeRegistration);
        Task<bool> DeleteTimeRegistrationAsync(Guid timeRegistrationId);
        Task<bool> UpdateTimeRegistrationAsync(Guid timeRegistrationId, TimeRegistrationDTO updatedTimeRegistration);
        Task<TimeRegistrationDTO?> GetTimeRegistrationAsync(Guid timeRegistrationId);
        Task<IList<TimeRegistrationDTO>> GetTimeRegistrationsForProjectAsync(Guid projectId);
        Task<IList<TimeRegistrationDTO>> GetTimeRegistrationsByFreelancerIdAndDate(Guid freelancerId, DateOnly date);
    }
}
