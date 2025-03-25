using Core.DTOs.Freelancer;
using Core.DTOs.Project;
using Core.DTOs.TimeRegistration;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IFreelancerService
    {
        Task UpdateFreelancer(Guid freelancerId, FreelancerDTO freelancerDTO);
        Task CreateFreelancer(FreelancerCreateDTO freelancerDTO, CancellationToken cancellationToken);
        Task DeleteFreelancer(Guid freelancerId);
        Task<FreelancerDTO> GetFreelancer(Guid freelancerId);

    }
}
