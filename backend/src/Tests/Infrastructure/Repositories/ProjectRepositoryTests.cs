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

        public ProjectRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite("Filename=:memory:")  // SQLite enforces foreign keys
                .Options;
            _dbContext = new ApplicationDbContext(options);
            _sut = new ProjectRepository(_dbContext);
            _dbContext.Database.OpenConnection(); 
            _dbContext.Database.EnsureCreated();
            Seed4ProjectsAnd4FreelancersDb();
        }

        [Fact]
        public void method()
        {
            // Arrange

            // Act

            // Assert
        }
        [Fact]
        public void Delete_WhenIdDoesNotExist_ShouldDoNothing()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            _sut.DeleteProject(id);

            // Assert
            Assert.Equal(4, _dbContext.Projects.Count());
        }

        [Fact]
        public void Delete_WhenIdExist_ShouldDelete()
        {
            // Arrange
            var project = _dbContext.Projects.FirstOrDefault();
            var id = project.Id;

            // Act
            _sut.DeleteProject(id);

            // Assert
            Assert.Equal(3, _dbContext.Projects.Count());
        }

        [Fact]
        public void Create_WhenProjectIsValid_ShouldCreate()
        {
            // Arrange
            var freelancer = _dbContext.Freelancers.FirstOrDefault();
            var project = new Project
            {
                FreelancerId = freelancer.Id,
                Name = "Project Beta",
                Client = "Client B",
                Deadline = DateTime.Now.AddMonths(3),
                Freelancer = freelancer
            };

            // Act
            _sut.CreateProject(project);

            // Assert
            Assert.Equal(5, _dbContext.Projects.Count());
        }
        [Fact]
        public void Create_WhenProjectDoesNotHaveFreelancerId_ShouldThrowDbUpdateException()
        {
            // Arrange
            var project = new Project
            {
                Name = "Project Morty",
                Client = "Client XX",
                Deadline = DateTime.Now.AddMonths(3),
            };

            // Act/Assert
            Assert.Throws<DbUpdateException>(() => _sut.CreateProject(project));

        }

        [Fact]
        public void Create_WhenProjectIsNull_ShouldThrowDbUpdateException()
        {
            var freelancer = _dbContext.Freelancers.FirstOrDefault();
            // Arrange
            var project = new Project();

            // Act/Assert
            Assert.Throws<DbUpdateException>(() => _sut.CreateProject(project));
        }

        [Fact]
        public void GetProject_WhenProjectExists_ReturnsProject()
        {
            // Arrange
            var freelancerId = _dbContext.Freelancers.FirstOrDefault().Id;
            var project = _dbContext.Projects.Where(p => p.FreelancerId == freelancerId).FirstOrDefault();

            // Act
            var result = _sut.GetProject(project.Id, freelancerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(project.Id, result?.Id);
            Assert.Equal(freelancerId, result?.FreelancerId);
        }

        [Fact]
        public void GetProject_WhenProjectDoesNotExist_ReturnsNull()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var freelancerId = Guid.NewGuid();

            // Act
            var result = _sut.GetProject(projectId, freelancerId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetProject_WhenFreelancerIdDoesNotMatch_ReturnsNull()
        {
            // Arrange
            var project = _dbContext.Projects.FirstOrDefault();

            // Act
            var result = _sut.GetProject(project.Id, Guid.NewGuid()); // Mismatched FreelancerId

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetProjectsByFreelancerId_WhenFreelancerHasProjects_ReturnsProjects()
        {
            // Arrange
            var freelancerId = _dbContext.Freelancers.FirstOrDefault().Id;
            var projects = new List<Project>
            {
                new Project { FreelancerId = freelancerId, Name = "Project 1", Client = "Client A", Deadline = DateTime.Now.AddMonths(1) },
                new Project { FreelancerId = freelancerId, Name = "Project 2", Client = "Client B", Deadline = DateTime.Now.AddMonths(2) }
            };

            _dbContext.Projects.AddRange(projects);
            _dbContext.SaveChanges();

            // Act
            var result = _sut.GetProjectsByFreelancerId(freelancerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.All(result, p => Assert.Equal(freelancerId, p.FreelancerId));
        }

        [Fact]
        public void GetProjectsByFreelancerId_WhenFreelancerHasNoProjects_ReturnsEmptyList()
        {
            // Arrange
            var freelancerId = Guid.NewGuid();

            // Act
            var result = _sut.GetProjectsByFreelancerId(freelancerId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetProjectsByFreelancerId_WhenMultipleFreelancersExist_ReturnsOnlyMatchingProjects()
        {
            // Arrange
            var freelancerId = _dbContext.Freelancers.FirstOrDefault().Id;
            var newProjects = new List<Project>
            {
                new Project(){Name = "abc", Client = "qwerty", Deadline = DateTime.Now, FreelancerId = freelancerId},
                new Project(){Name = "cba", Client = "ytrewq", Deadline = DateTime.Now, FreelancerId = freelancerId}
            };
            _dbContext.Projects.AddRange(newProjects);
            _dbContext.SaveChanges();

            // Act
            var result = _sut.GetProjectsByFreelancerId(freelancerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.All(result, p => Assert.Equal(freelancerId, p.FreelancerId));
        }

        private void Seed4ProjectsAnd4FreelancersDb()
        {
            var freelancers = new List<Freelancer>
            {
                new Freelancer
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    Password = "Password123",
                    Projects = new List<Project>() // You can add some sample projects here if needed
                },
                new Freelancer
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    Password = "Password456",
                    Projects = new List<Project>() // Add some sample projects here if needed
                },
                new Freelancer
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Alice",
                    LastName = "Johnson",
                    Email = "alice.johnson@example.com",
                    Password = "Password789",
                    Projects = new List<Project>() // Add some sample projects here if needed
                },
                new Freelancer
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Bob",
                    LastName = "Williams",
                    Email = "bob.williams@example.com",
                    Password = "Password101",
                    Projects = new List<Project>() // Add some sample projects here if needed
                }
            };
            var projects = new List<Project>
        {
            new Project
            {
                Id = Guid.NewGuid(),
                FreelancerId = freelancers[0].Id,
                Name = "Project Alpha",
                Client = "Client A",
                Deadline = DateTime.Now.AddMonths(2),
                Freelancer = freelancers[0] 
            },
            new Project
            {
                Id = Guid.NewGuid(),
                FreelancerId = freelancers[1].Id,
                Name = "Project Beta",
                Client = "Client B",
                Deadline = DateTime.Now.AddMonths(3),
                Freelancer = freelancers[1] 
            },
            new Project
            {
                Id = Guid.NewGuid(),
                FreelancerId = freelancers[2].Id,
                Name = "Project Gamma",
                Client = "Client C",
                Deadline = DateTime.Now.AddMonths(4),
                Freelancer = freelancers[2] 
            },
            new Project
            {
                Id = Guid.NewGuid(),
                FreelancerId = freelancers[3].Id,
                Name = "Project Delta",
                Client = "Client D",
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
