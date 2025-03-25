using AutoMapper;
using Core.DTOs.Freelancer;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Infrastructure.Repositories
{
    public class FreelancerRepository(ApplicationDbContext dbContext, IMapper mapper) : IFreelancerRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<FreelancerRepository> _logger;

        public async Task<Guid?> CreateFreelancerAsync(FreelancerCreateDTO freelancerDTO, CancellationToken cancellationToken)
        {
            try
            {
                var freelancer = _mapper.Map<Freelancer>(freelancerDTO);
                await _dbContext.Freelancers.AddAsync(freelancer, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return freelancer.Id;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Failed to create freelancer.");
                return null;
            }
        }

        public async Task<FreelancerDTO?> GetFreelancerAsync(Guid id)
        {
            try
            {
                var freelancer = await _dbContext.Freelancers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(f => f.Id == id);

                return freelancer is not null ? _mapper.Map<FreelancerDTO>(freelancer) : null;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Failed to fetch freelancer.");
                return null;
            }
        }

        public async Task<bool> UpdateFreelancerAsync(Guid id, FreelancerDTO updatedFreelancer)
        {
            try
            {
                var freelancer = await _dbContext.Freelancers.FindAsync(id);
                if (freelancer is null)
                {
                    //_logger.LogWarning("Freelancer not found: {FreelancerId}", id);
                    return false;
                }

                _mapper.Map(updatedFreelancer, freelancer);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Failed to update freelancer.");
                return false;
            }
        }

        public async Task<bool> DeleteFreelancerAsync(Guid id)
        {
            try
            {
                var freelancer = await _dbContext.Freelancers.FindAsync(id);
                if (freelancer is null)
                {
                    //_logger.LogWarning("Freelancer not found: {FreelancerId}", id);
                    return false;
                }

                _dbContext.Freelancers.Remove(freelancer);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Failed to delete freelancer.");
                return false;
            }
        }
    }
}
