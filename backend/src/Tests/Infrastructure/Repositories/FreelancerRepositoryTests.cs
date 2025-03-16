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
    public class FreelancerRepositoryTests : IDisposable
    {
        private readonly FreelancerRepository _sut;
        private readonly ApplicationDbContext _dbContext;

        public FreelancerRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite("Filename=:memory:") 
                .Options;
            _dbContext = new ApplicationDbContext(options);
            _sut = new FreelancerRepository(_dbContext);
            _dbContext.Database.OpenConnection();
            _dbContext.Database.EnsureCreated();
            Seed4FreelancersDb();
        }
        [Fact]
        public void method()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public void Get_WhenFreelancerExists_ShouldReturnFreelancer()
        {
            // Arrange
            var id = _dbContext.Freelancers.First().Id;

            // Act
            var freelancer = _sut.GetFreelancer(id);

            // Assert
            Assert.NotNull(freelancer);
        }

        [Fact]
        public void Get_WhenFreelancerNotExists_ShouldReturnNull()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var freelancer = _sut.GetFreelancer(id);

            // Assert
            Assert.Null(freelancer);
        }

        [Fact]
        public void Delete_WhenIdExist_ShouldDelete()
        {
            // Arrange
            var id = _dbContext.Freelancers.First().Id;

            // Act
            _sut.DeleteFreeLancer(id);

            // Assert
            Assert.Equal(3, _dbContext.Freelancers.Count());
            Assert.Null(_dbContext.Freelancers.Where(f => f.Id == id).FirstOrDefault());
        }


        [Fact]
        public void Delete_WhenIdNotExist_ShouldDoNothing()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            _sut.DeleteFreeLancer(id);

            // Assert
            Assert.Equal(4, _dbContext.Freelancers.Count());
        }


        [Theory]
        [InlineData(null, "Doe", "Password123!", "test@example.com")] // Missing FirstName
        [InlineData("John", null, "Password123!", "test@example.com")] // Missing LastName
        [InlineData("John", "Doe", null, "test@example.com")] // Missing Password
        [InlineData("John", "Doe", "Password123!", null)] // Missing Email
        public void Create_WhenMissingRequiredField_ShouldThrow(string firstName, string lastName, string password, string email)
        {
            var projects = new List<Project>();
            // Arrange
            var freelancer = new Freelancer
            {
                FirstName = firstName,
                LastName = lastName,
                Password = password,
                Email = email,
                Projects = projects
            };

            // Act & Assert
            Assert.Throws<DbUpdateException>(() => _sut.CreateFreeLancer(freelancer));
        }

        [Fact]
        public void Create_WhenFreelancerIsValid_ShouldCreate()
        {
            // Arrange
            var freelancer = new Freelancer { Email = "Rick@Morty.com", FirstName = "Rick", LastName = "Morty", Password = "MyStrongPassw0rd!" };

            // Act
            _sut.CreateFreeLancer(freelancer);

            // Assert
            Assert.Equal(5, _dbContext.Freelancers.Count());
        }





        private void Seed4FreelancersDb()
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
        }
        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}
