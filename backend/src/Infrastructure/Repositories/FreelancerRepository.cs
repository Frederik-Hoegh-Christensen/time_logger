using Core.Entities;
using Core.Interfaces;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class FreelancerRepository(ApplicationDbContext dbContext) : IFreelancerRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        public void CreateFreeLancer(Freelancer freelancer)
        {
            _dbContext.Freelancers.Add(freelancer);
            _dbContext.SaveChanges();
        }

        public void DeleteFreeLancer(Guid id)
        {
            var freelancer = _dbContext.Freelancers.Where(f => f.Id == id).FirstOrDefault();
            if (freelancer != null)
            {
                _dbContext.Remove(freelancer);
                _dbContext.SaveChanges();
            }

        }

        public Freelancer? GetFreelancer(Guid id)
        {
            var freelancer = _dbContext.Freelancers.Where(f => f.Id == id).FirstOrDefault();
            if (freelancer != null)
            {
                return freelancer;
            }
            else
            {
                return null;
            }
        }

        public void UpdateFreeLancer(Guid id, Freelancer updatedFreelancer)
        {
            var freelancer = _dbContext.Freelancers.FirstOrDefault(f => f.Id == id);

            if (freelancer != null)
            {
                freelancer.FirstName = updatedFreelancer.FirstName;
                freelancer.LastName = updatedFreelancer.LastName;
                freelancer.Email = updatedFreelancer.Email;
                freelancer.Password = updatedFreelancer.Password;

                _dbContext.Freelancers.Update(freelancer);
                _dbContext.SaveChanges();
            }   
        }
    }
}
