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
        void CreateTimeRegistration(TimeRegistration timeRegistration);
        void DeleteTimeRegistration(Guid id);
        void UpdateTimeRegistration(Guid id);
        void GetFreelancer(Guid id);
    }
}
