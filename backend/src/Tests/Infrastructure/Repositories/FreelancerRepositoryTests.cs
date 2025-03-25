using Application.Mappings;
using AutoMapper;
using Core.DTOs.Freelancer;
using Core.Entities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Infrastructure.Repositories
{
    public class FreelancerRepositoryTests : IDisposable
    {
        private readonly FreelancerRepository _sut;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public FreelancerRepositoryTests()
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
            _sut = new FreelancerRepository(_dbContext, _mapper);
            _dbContext.Database.OpenConnection();
            _dbContext.Database.EnsureCreated();
            Seed4FreelancersDbAsync().Wait();
        }

        [Fact]
        public async Task Get_WhenFreelancerExists_ShouldReturnFreelancer()
        {
            // Arrange
            var id = _dbContext.Freelancers.First().Id;

            // Act
            var freelancer = await _sut.GetFreelancerAsync(id);

            // Assert
            Assert.NotNull(freelancer);
        }

        [Fact]
        public async Task Get_WhenFreelancerNotExists_ShouldReturnNull()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var freelancer = await _sut.GetFreelancerAsync(id);

            // Assert
            Assert.Null(freelancer);
        }

        [Fact]
        public async Task Delete_WhenIdExist_ShouldReturnTrue()
        {
            // Arrange
            var id = _dbContext.Freelancers.First().Id;

            // Act
            var deleted = await _sut.DeleteFreelancerAsync(id);

            // Assert
            Assert.True(deleted);
            Assert.Null(await _dbContext.Freelancers.Where(f => f.Id == id).FirstOrDefaultAsync());
        }

        [Fact]
        public async Task Delete_WhenIdNotExist_ShouldReturnFalse()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var deleted = await _sut.DeleteFreelancerAsync(id);

            //
            Assert.False(deleted);
            Assert.Equal(4, await _dbContext.Freelancers.CountAsync());
        }

        [Theory]
        [InlineData(null, "Doe", "Password123!", "test@example.com")] // Missing FirstName
        [InlineData("John", null, "Password123!", "test@example.com")] // Missing LastName
        [InlineData("John", "Doe", null, "test@example.com")] // Missing Password
        [InlineData("John", "Doe", "Password123!", null)] // Missing Email
        public async Task Create_WhenMissingRequiredField_ShouldReturnNull(string firstName, string lastName, string password, string email)
        {
            var projects = new List<Project>();
            // Arrange
            var freelancer = new FreelancerCreateDTO
            {
                FirstName = firstName,
                LastName = lastName,
                Password = password,
                Email = email,
            };

            // Act
            var created = await _sut.CreateFreelancerAsync(freelancer, new CancellationToken());
            // Assert
            Assert.Null(created);
        }

        [Fact]
        public async Task Create_WhenFreelancerIsValid_ShouldCreateAndReturnId()
        {
            // Arrange
            var freelancer = new FreelancerCreateDTO { Email = "Rick@Morty.com", FirstName = "Rick", LastName = "Morty", Password = "MyStrongPassw0rd!" };

            // Act
            var freelancerId = await _sut.CreateFreelancerAsync(freelancer, new CancellationToken());
            var createdFreelancer = _dbContext.Freelancers.First(f => f.Id == freelancerId);
            // Assert
            Assert.NotNull(createdFreelancer);
            Assert.Equal(createdFreelancer.Email, freelancer.Email);
            Assert.Equal(createdFreelancer.FirstName, freelancer.FirstName);
            Assert.Equal(createdFreelancer.LastName, freelancer.LastName);
        }

        private async Task Seed4FreelancersDbAsync()
        {
            var freelancers = new List<Freelancer>
            {
                new Freelancer
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    Password = "Password123",
                    Projects = new List<Project>()
                },
                new Freelancer
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    Password = "Password456",
                    Projects = new List<Project>()
                },
                new Freelancer
                {
                    FirstName = "Alice",
                    LastName = "Johnson",
                    Email = "alice.johnson@example.com",
                    Password = "Password789",
                    Projects = new List<Project>()
                },
                new Freelancer
                {
                    FirstName = "Bob",
                    LastName = "Williams",
                    Email = "bob.williams@example.com",
                    Password = "Password101",
                    Projects = new List<Project>()
                }
            };
            await _dbContext.Freelancers.AddRangeAsync(freelancers);
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}
