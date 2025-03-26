using Application.Mappings;
using AutoMapper;
using Core.DTOs.TimeRegistration;
using Core.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Infrastructure.Repositories
{
    public class TimeRegistrationRepositoryTests : IDisposable
    {
        private readonly TimeRegistrationRepository _sut;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public TimeRegistrationRepositoryTests()
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
            _sut = new TimeRegistrationRepository(_dbContext, _mapper);
            _dbContext.Database.OpenConnection();
            _dbContext.Database.EnsureCreated();
            Seed4Projects4Freelancers4TimeRegistrationsDb();
        }

        [Fact]
        public async Task CreateTimeRegistrationAsync_ShouldAddTimeRegistration_WhenTimeRegistrationIsValid()
        {
            // Arrange
            var timeRegistration = new TimeRegistrationCreateDTO
            {
                ProjectId = _dbContext.Projects.First().Id,
                FreelancerId = _dbContext.Freelancers.First().Id,
                WorkDate = DateOnly.FromDateTime(DateTime.Now),
                HoursWorked = 5.0m,
                Description = "Test Task"
            };

            // Act
            var id = await _sut.CreateTimeRegistrationAsync(timeRegistration);
            
            // Assert
            Assert.NotNull(id);
            var createdTimeRegistration = _dbContext.TimeRegistrations.First(tr => tr.Id == id);
            Assert.Equal(timeRegistration.Description, createdTimeRegistration.Description);
            Assert.Equal(timeRegistration.FreelancerId, createdTimeRegistration.FreelancerId);
            Assert.Equal(timeRegistration.WorkDate, createdTimeRegistration.WorkDate);
            Assert.Equal(timeRegistration.HoursWorked, createdTimeRegistration.HoursWorked);
            Assert.Equal(timeRegistration.ProjectId, createdTimeRegistration.ProjectId);
        }

        [Fact]
        public async Task CreateTimeRegistrationAsync_ShouldReturnNull_WhenTimeRegistrationMissingProjectId()
        {
            var freelancerId = _dbContext.Freelancers.First().Id;
            // Arrange
            var timeRegistration = new TimeRegistrationCreateDTO
            {
                FreelancerId = freelancerId,
                WorkDate = DateOnly.FromDateTime(DateTime.Now),
                HoursWorked = 5.0m,
                Description = "Test Task"
            };

            // Act
            var id = await _sut.CreateTimeRegistrationAsync(timeRegistration);

            // Assert
            Assert.Null(id);
            Assert.Equal(4, _dbContext.TimeRegistrations.Count());
        }

        [Fact]
        public async Task CreateTimeRegistrationAsync_ShouldReturnNull_WhenTimeRegistrationMissingFreelancerId()
        {
            var projectId = _dbContext.Projects.First().Id;
            
            // Arrange
            var timeRegistration = new TimeRegistrationCreateDTO
            {
                ProjectId = projectId,
                WorkDate = DateOnly.FromDateTime(DateTime.Now),
                HoursWorked = 5.0m,
                Description = "Test Task"
            };

            // Act
            var id = await _sut.CreateTimeRegistrationAsync(timeRegistration);

            // Assert
            Assert.Null(id);
            Assert.Equal(4, _dbContext.TimeRegistrations.Count());
        }

        [Fact]
        public async Task DeleteTimeRegistrationAsync_ShouldRemoveTimeRegistration_WhenIdIsValid()
        {
            // Arrange
            var timeRegistration = await _dbContext.TimeRegistrations.FirstAsync();
            var timeRegistrationId = timeRegistration.Id;

            // Act
            await _sut.DeleteTimeRegistrationAsync(timeRegistrationId);

            // Assert
            var deletedTimeRegistration = await _dbContext.TimeRegistrations
                .FirstOrDefaultAsync(tr => tr.Id == timeRegistrationId);
            Assert.Null(deletedTimeRegistration);
        }

        [Fact]
        public async Task GetTimeRegistrationAsync_ShouldReturnTimeRegistration_WhenIdIsValid()
        {
            // Arrange
            var timeRegistration = await _dbContext.TimeRegistrations.FirstAsync();
            var timeRegistrationId = timeRegistration.Id;

            // Act
            var result = await _sut.GetTimeRegistrationAsync(timeRegistrationId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(timeRegistrationId, result.Id);
        }

        [Fact]
        public async Task GetTimeRegistrationAsync_ShouldReturnNull_WhenIdIsInvalid()
        {
            // Arrange
            var invalidId = Guid.NewGuid();

            // Act
            var result = await _sut.GetTimeRegistrationAsync(invalidId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateTimeRegistrationAsync_ShouldUpdateTimeRegistration_WhenIdAndUpdatedTimeRegistrationAreValid()
        {
            // Arrange
            var timeRegistration = await _dbContext.TimeRegistrations.FirstAsync();
            var updatedTimeRegistration = new TimeRegistrationDTO
            {
                Id = timeRegistration.Id,
                ProjectId = timeRegistration.ProjectId,
                FreelancerId = timeRegistration.FreelancerId,
                WorkDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
                HoursWorked = 9.0m,
                Description = "Updated task"
            };

            // Act
            await _sut.UpdateTimeRegistrationAsync(timeRegistration.Id, updatedTimeRegistration);

            // Assert
            var updatedEntity = await _dbContext.TimeRegistrations
                .FirstOrDefaultAsync(tr => tr.Id == timeRegistration.Id);
            Assert.NotNull(updatedEntity);
            Assert.Equal(updatedTimeRegistration.Description, updatedEntity.Description);
            Assert.Equal(updatedTimeRegistration.HoursWorked, updatedEntity.HoursWorked);
        }

        [Fact]
        public async Task UpdateTimeRegistrationAsync_ShouldReturnFalse_WhenUpdatedTimeRegistrationIsNull()
        {
            // Arrange
            var timeRegistration = await _dbContext.TimeRegistrations.FirstAsync();

            // Act 
            var updated = await _sut.UpdateTimeRegistrationAsync(timeRegistration.Id, null);

            //Assert
            Assert.False(updated);
        }

        [Fact]
        public async Task UpdateTimeRegistrationAsync_ShouldReturnFalse_WhenTimeRegistrationDoesNotExist()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            var updatedTimeRegistration = new TimeRegistrationDTO
            {
                Id = invalidId,
                ProjectId = Guid.NewGuid(),
                FreelancerId = Guid.NewGuid(),
                WorkDate = DateOnly.FromDateTime(DateTime.Now),
                HoursWorked = 8.0m,
                Description = "Test Update"
            };

            // Act 
            var updated = await _sut.UpdateTimeRegistrationAsync(invalidId, updatedTimeRegistration);

            // Assert
            Assert.False(updated);
        }



        private void Seed4Projects4Freelancers4TimeRegistrationsDb()
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
            _dbContext.Freelancers.AddRange(freelancers);
            _dbContext.SaveChanges();

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


            _dbContext.Projects.AddRange(projects);
            _dbContext.SaveChanges();

            var timeRegistrations = new List<TimeRegistration>
            {
                new TimeRegistration
                {
                    ProjectId = projects[0].Id,
                    FreelancerId = freelancers[0].Id,
                    WorkDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-5)),
                    HoursWorked = 8.5m,
                    Description = "Worked on Project Alpha task A"
                },
                new TimeRegistration
                {
                    ProjectId = projects[1].Id,
                    FreelancerId = freelancers[1].Id,
                    WorkDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-4)),
                    HoursWorked = 7.5m,
                    Description = "Worked on Project Beta task B"
                },
                new TimeRegistration
                {
                    ProjectId = projects[2].Id,
                    FreelancerId = freelancers[2].Id,
                    WorkDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-3)),
                    HoursWorked = 6.0m,
                    Description = "Worked on Project Gamma task C"
                },
                new TimeRegistration
                {
                    ProjectId = projects[3].Id,
                    FreelancerId = freelancers[3].Id,
                    WorkDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-2)),
                    HoursWorked = 5.5m,
                    Description = "Worked on Project Delta task D"
                }
            };
            _dbContext.TimeRegistrations.AddRange(timeRegistrations);
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}
