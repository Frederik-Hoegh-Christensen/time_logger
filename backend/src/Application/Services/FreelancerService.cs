using Core.DTOs.Freelancer;
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
    public class FreelancerService(IFreelancerRepository freelancerRepository) : IFreelancerService
    {
        private readonly IFreelancerRepository _freelancerRepository = freelancerRepository;

        public async Task CreateFreelancer(FreelancerCreateDTO freelancerDTO, CancellationToken cancellationToken)
        {
            var id = await _freelancerRepository.CreateFreelancerAsync(freelancerDTO, cancellationToken);
            if (id == null)
            {

            }
            //TODO what here
            return;
        }

        public async Task DeleteFreelancer(Guid freelancerId)
        {
            await _freelancerRepository.DeleteFreelancerAsync(freelancerId);
        }

        public async Task<FreelancerDTO> GetFreelancer(Guid freelancerId)
        {
            var freelancer = await _freelancerRepository.GetFreelancerAsync(freelancerId);
            return freelancer;
        }

        public async Task UpdateFreelancer(Guid freelancerId, FreelancerDTO freelancerDTO)
        {
            await _freelancerRepository.UpdateFreelancerAsync(freelancerId, freelancerDTO);
        }
    }
}
