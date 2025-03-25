using Core.DTOs.TimeRegistration;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ITimeRegistrationRepository
    {
        Task<Guid?> CreateTimeRegistrationAsync(TimeRegistrationCreateDTO timeRegistration);
        Task<bool> DeleteTimeRegistrationAsync(Guid id);
        Task<bool> UpdateTimeRegistrationAsync(Guid id, TimeRegistrationDTO updatedTimeRegistration);
        Task<TimeRegistrationDTO?> GetTimeRegistrationAsync(Guid id);
        Task <IList<TimeRegistrationDTO>> GetTimeRegistrationsForProjectAsync(Guid projectId);
        Task<IList<TimeRegistrationDTO>> GetTimeRegistrationsByFreelancerIdAndDate(Guid freelancerId, DateOnly date);
    }
}
