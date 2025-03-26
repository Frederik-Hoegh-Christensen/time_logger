using Application.Mappings;
using AutoMapper;
using Core.DTOs.Project;
using Core.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Sdk;


namespace Tests.Infrastructure.Repositories
{
    public class ProjectRepositoryTests : IDisposable
    {
        private readonly ProjectRepository _sut;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProjectRepositoryTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // Add your AutoMapper profiles here (e.g. mapping between DTOs and entities)
                cfg.AddProfile(new MappingProfile());
            });
            _mapper = config.CreateMapper();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite("Filename=:memory:")
                .Options;
            _dbContext = new ApplicationDbContext(options);
            _sut = new ProjectRepository(_dbContext, _mapper);
            _dbContext.Database.OpenConnection(); 
            _dbContext.Database.EnsureCreated();
            Seed4ProjectsAnd4FreelancersDb();
        }


        [Fact]
        public async Task Delete_WhenIdDoesNotExist_ShouldDoNothing()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            await _sut.DeleteProjectAsync(id);

            // Assert
            Assert.Equal(4, _dbContext.Projects.Count());
        }

        [Fact]
        public async Task Delete_WhenIdExists_ShouldDelete()
        {
            // Arrange
            var project = await _dbContext.Projects.FirstOrDefaultAsync();
            var id = project.Id;

            // Act
            await _sut.DeleteProjectAsync(id);

            // Assert
            Assert.Equal(3, _dbContext.Projects.Count());
            Assert.Null(await _dbContext.Projects.Where(p => p.Id == id).FirstOrDefaultAsync());
        }

        [Fact]
        public async Task Create_WhenProjectIsValid_ShouldCreate()
        {
            // Arrange
            var freelancer = await _dbContext.Freelancers.FirstOrDefaultAsync();
            var customer = await _dbContext.Customers.FirstOrDefaultAsync();

            var project = new ProjectCreateDTO
            {
                FreelancerId = freelancer.Id,
                Name = "Project Beta",
                CustomerId = customer.Id,
                Deadline = DateTime.Now.AddMonths(3),
            };

            // Act
            var projectId = await _sut.CreateProjectAsync(project);
            var createdProject = _dbContext.Projects.First(p => p.Id == projectId);
            // Assert
            Assert.NotNull(createdProject);
            Assert.Equal(createdProject.FreelancerId, freelancer.Id);
            Assert.Equal(createdProject.Name, project.Name);
            Assert.Equal(createdProject.CustomerId, project.CustomerId);
            Assert.Equal(createdProject.Deadline, project.Deadline);
        }

        [Fact]
        public async Task Create_WhenProjectDoesNotHaveFreelancerId_ShouldReturnNull()
        {
            var customer = await _dbContext.Customers.FirstAsync();

            // Arrange
            var project = new ProjectCreateDTO
            {
                Name = "Project Morty",
                CustomerId = customer.Id,
                Deadline = DateTime.Now.AddMonths(3),
            };

            // Act
            var created =  await _sut.CreateProjectAsync(project);
            Assert.Null(created);
        }

        [Fact]
        public async Task Create_WhenProjectIsNull_ShouldReturnNull()
        {
            // Arrange
            var project = new ProjectCreateDTO();

            // Act
            var created = await _sut.CreateProjectAsync(project);

            //Assert
            Assert.Null(created);
            Assert.Equal(4, _dbContext.Projects.Count());
        }

        [Fact]
        public async Task GetProject_WhenProjectExists_ReturnsProject()
        {
            // Arrange
            var freelancerId = (await _dbContext.Freelancers.FirstAsync()).Id;
            var project = await _dbContext.Projects
                .Where(p => p.FreelancerId == freelancerId)
                .FirstOrDefaultAsync();

            // Act
            var result = await _sut.GetProjectAsync(project.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(project.Id, result?.Id);
            Assert.Equal(freelancerId, result?.FreelancerId);
        }

        [Fact]
        public async Task GetProject_WhenProjectDoesNotExist_ReturnsNull()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var freelancerId = Guid.NewGuid();

            // Act
            var result = await _sut.GetProjectAsync(projectId);

            // Assert
            Assert.Null(result);
        }


        [Fact]
        public async Task GetProjectsByFreelancerId_WhenFreelancerHasProjects_ReturnsProjects()
        {
            // Arrange
            var freelancerId = (await _dbContext.Freelancers.FirstAsync()).Id;

            // Act
            var result = await _sut.GetProjectsByFreelancerIdAsync(freelancerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
            Assert.All(result, p => Assert.Equal(freelancerId, p.FreelancerId));
        }

        [Fact]
        public async Task GetProjectsByFreelancerId_WhenFreelancerHasNoProjects_ReturnsEmptyList()
        {
            // Arrange
            var freelancerId = Guid.NewGuid();

            // Act
            var result = await _sut.GetProjectsByFreelancerIdAsync(freelancerId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task UpdateProject_WhenProjectExists_ShouldUpdateFields()
        {
            // Arrange
            var customer = await _dbContext.Customers.FirstAsync();
            var project = await _dbContext.Projects.FirstAsync();
            var updatedProjectDTO = new ProjectDTO
            {
                Id = project.Id,
                FreelancerId = project.FreelancerId,
                Name = "Updated Name",
                CustomerId = customer.Id,
                CustomerName = customer.Name,
                Deadline = DateTime.Now.AddMonths(6)
            };

            // Act
            var updated = await _sut.UpdateProjectAsync(project.Id, updatedProjectDTO);
            var updatedProject = await _dbContext.Projects.FirstAsync(p => p.Id == project.Id);
            // Assert
            Assert.True(updated);
            Assert.Equal(updatedProject.Name, updatedProjectDTO.Name);
            Assert.Equal(updatedProject.CustomerId, updatedProjectDTO.CustomerId);
            Assert.Equal(updatedProject.Deadline, updatedProjectDTO.Deadline);
        }

        [Fact]
        public async Task UpdateProject_WhenProjectDoesNotExist_ShouldDoNothing()
        {
            // Arrange
            var customer = await _dbContext.Customers.FirstAsync();
            var nonExistentId = Guid.NewGuid();
            var updatedProject = new ProjectDTO
            {
                Name = "Non-Existent Update",
                CustomerId = customer.Id,
                Deadline = DateTime.Now.AddMonths(6)
            };

            // Act
            await _sut.UpdateProjectAsync(nonExistentId, updatedProject);

            // Assert
            Assert.Equal(4, _dbContext.Projects.Count());
        }

        private void Seed4ProjectsAnd4FreelancersDb()
        {
            var freelancers = new List<Freelancer>
            {
                new Freelancer
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    Password = "Password123",
                    Projects = new List<Project>() // You can add some sample projects here if needed
                },
                new Freelancer
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    Password = "Password456",
                    Projects = new List<Project>() // Add some sample projects here if needed
                },
                new Freelancer
                {
                    FirstName = "Alice",
                    LastName = "Johnson",
                    Email = "alice.johnson@example.com",
                    Password = "Password789",
                    Projects = new List<Project>() // Add some sample projects here if needed
                },
                new Freelancer
                {
                    FirstName = "Bob",
                    LastName = "Williams",
                    Email = "bob.williams@example.com",
                    Password = "Password101",
                    Projects = new List<Project>() // Add some sample projects here if needed
                }
            };
            var customers = new List<Customer>
            {
                new Customer
                {
                    Name = "CustomerOne",
                    Email = "CustomerOne@gmail.com"
                },
                new Customer
                {
                    Name = "CustomerTwo",
                    Email = "CustomerTwo@gmail.com"
                },
                new Customer
                {
                    Name = "CustomerThree",
                    Email = "CustomerThree@gmail.com"
                },
                new Customer
                {
                    Name = "CustomerFour",
                    Email = "CustomerFour@gmail.com"
                },
            };
            _dbContext.Customers.AddRange(customers);
            _dbContext.SaveChanges();
            var projects = new List<Project>
        {
            new Project
            {
                FreelancerId = freelancers[0].Id,
                Name = "Project Alpha",
                CustomerId = customers[0].Id,
                Deadline = DateTime.Now.AddMonths(2),
                Freelancer = freelancers[0]
            },
            new Project
            {
                FreelancerId = freelancers[1].Id,
                Name = "Project Beta",
                CustomerId = customers[1].Id,
                Deadline = DateTime.Now.AddMonths(3),
                Freelancer = freelancers[1]
            },
            new Project
            {
                FreelancerId = freelancers[2].Id,
                Name = "Project Gamma",
                CustomerId = customers[2].Id,
                Deadline = DateTime.Now.AddMonths(4),
                Freelancer = freelancers[2]
            },
            new Project
            {
                FreelancerId = freelancers[3].Id,
                Name = "Project Delta",
                CustomerId = customers[3].Id,
                Deadline = DateTime.Now.AddMonths(5),
                Freelancer = freelancers[3] // This links the freelancer to the project
            }
        };

            _dbContext.Freelancers.AddRange(freelancers);
            _dbContext.Projects.AddRange(projects);
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}
