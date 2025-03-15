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
        void CreateFreeLancer(Freelancer freelancer);
        void DeleteFreeLancer(Guid id);
        void UpdateFreeLancer(Guid id);
        void GetFreelancer(Guid id);
    }
}
