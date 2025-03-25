using Core.DTOs.Freelancer;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IFreelancerRepository
    {
        Task<Guid?> CreateFreelancerAsync(FreelancerCreateDTO freelancer, CancellationToken cancellationToken);
        Task<bool> DeleteFreelancerAsync(Guid id);
        Task<bool> UpdateFreelancerAsync(Guid id, FreelancerDTO updatedFreelancer);
        Task<FreelancerDTO?> GetFreelancerAsync(Guid id);
    }
}
