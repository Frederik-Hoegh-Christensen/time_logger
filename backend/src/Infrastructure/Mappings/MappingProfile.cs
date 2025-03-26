using Core.DTOs.Customer;
using Core.DTOs.Freelancer;
using Core.DTOs.Project;
using Core.DTOs.TimeRegistration;
using AutoMapper;
using Core.Entities;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Freelancer, FreelancerDTO>().ReverseMap();
            CreateMap<Freelancer, FreelancerCreateDTO>().ReverseMap();

            CreateMap<Project, ProjectDTO>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))

                .ReverseMap()
                .ForMember(dest => dest.Customer, opt => opt.Ignore());
            CreateMap<Project, ProjectCreateDTO>().ReverseMap();

            CreateMap<TimeRegistrationDTO, TimeRegistration>().ReverseMap()
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project != null ? src.Project.Name : ""));
            CreateMap<TimeRegistration, TimeRegistrationCreateDTO>().ReverseMap();

            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<Customer, CustomerCreateDTO>().ReverseMap();
        }
    }
}
