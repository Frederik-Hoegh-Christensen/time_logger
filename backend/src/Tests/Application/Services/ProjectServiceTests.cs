using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Application.Services;
using Core.DTOs.Project;
using Core.Interfaces;
using Application.Interfaces;

namespace Tests.Application.Services
{
    public class ProjectServiceTests
    {
        private readonly IProjectService _sut;
        private readonly Mock<IProjectRepository> _mockProjectRepository;

        public ProjectServiceTests()
        {
            _mockProjectRepository = new Mock<IProjectRepository>();
            _sut = new ProjectService(_mockProjectRepository.Object);
        }

        [Fact]
        public async Task CreateProjectAsync_ShouldReturnNull_WhenDeadlineIsInThePast()
        {
            // Arrange
            var projectDTO = new ProjectCreateDTO { Deadline = DateTime.Today.AddDays(-1) };

            // Act
            var id = await _sut.CreateProjectAsync(projectDTO);

            // Assert
            Assert.Null(id);
        }

        [Fact]
        public async Task CreateProjectAsync_ShouldReturnProjectId_WhenValid()
        {
            // Arragne
            var projectDTO = new ProjectCreateDTO { Deadline = DateTime.Today.AddDays(10) };
            var expectedId = Guid.NewGuid();
            _mockProjectRepository.Setup(repo => repo.CreateProjectAsync(projectDTO)).ReturnsAsync(expectedId);

            // Act
            var result = await _sut.CreateProjectAsync(projectDTO);

            // Assert
            Assert.Equal(expectedId, result);
        }

        [Fact]
        public async Task DeleteProjectAsync_ShouldReturnTrue_WhenDeletionSucceeds()
        {
            // Arragne
            var projectId = Guid.NewGuid();
            _mockProjectRepository.Setup(repo => repo.DeleteProjectAsync(projectId)).ReturnsAsync(true);

            //Act
            var result = await _sut.DeleteProjectAsync(projectId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetProjectAsync_ShouldReturnProject_WhenExists()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var projectDTO = new ProjectDTO { Id = projectId };
            _mockProjectRepository.Setup(repo => repo.GetProjectAsync(projectId)).ReturnsAsync(projectDTO);

            // Act
            var result = await _sut.GetProjectAsync(projectId);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(projectId, result.Id);
        }

        [Fact]
        public async Task GetProjectsByFreelancerIdAsync_ShouldReturnListOfProjects()
        {
            //Arrange
            var freelancerId = Guid.NewGuid();
            var projects = new List<ProjectDTO> { new ProjectDTO(), new ProjectDTO() };
            _mockProjectRepository.Setup(repo => repo.GetProjectsByFreelancerIdAsync(freelancerId)).ReturnsAsync(projects);

            //Act
            var result = await _sut.GetProjectsByFreelancerIdAsync(freelancerId);

            //Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task UpdateProjectAsync_ShouldReturnFalse_WhenDeadlineIsInThePast()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var updatedProject = new ProjectDTO { Deadline = DateTime.UtcNow.AddDays(-1) };

            //Act
            var updated = await _sut.UpdateProjectAsync(projectId, updatedProject);
            // Assert
            Assert.False(updated);
            
        
        }

        [Fact]
        public async Task UpdateProjectAsync_ShouldReturnFalse_WhenProjectDoesNotExist()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var updatedProject = new ProjectDTO { Deadline = DateTime.UtcNow.AddDays(10) };
            _mockProjectRepository.Setup(repo => repo.GetProjectAsync(projectId)).ReturnsAsync((ProjectDTO?)null);

            // Act
            var updated = await _sut.UpdateProjectAsync(projectId, updatedProject);

            // Assert
            Assert.False(updated);
        }

        [Fact]
        public async Task UpdateProjectAsync_ShouldCallRepositoryUpdate_WhenValid()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var updatedProject = new ProjectDTO { Deadline = DateTime.UtcNow.AddDays(10) };
            _mockProjectRepository.Setup(repo => repo.GetProjectAsync(projectId)).ReturnsAsync(updatedProject);

            // Act
            await _sut.UpdateProjectAsync(projectId, updatedProject);

            // Assert
            _mockProjectRepository.Verify(repo => repo.UpdateProjectAsync(projectId, updatedProject), Times.Once);
        }
    }
}
