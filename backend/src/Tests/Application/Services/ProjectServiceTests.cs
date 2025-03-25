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
        private readonly IProjectService _projectService;
        private readonly Mock<IProjectRepository> _mockProjectRepository;

        public ProjectServiceTests()
        {
            _mockProjectRepository = new Mock<IProjectRepository>();
            _projectService = new ProjectService(_mockProjectRepository.Object);
        }

        [Fact]
        public async Task CreateProjectAsync_ShouldThrowArgumentException_WhenDeadlineIsInThePast()
        {
            var projectDTO = new ProjectCreateDTO { Deadline = DateTime.UtcNow.AddDays(-1) };

            await Assert.ThrowsAsync<ArgumentException>(() => _projectService.CreateProjectAsync(projectDTO));
        }

        [Fact]
        public async Task CreateProjectAsync_ShouldReturnProjectId_WhenValid()
        {
            var projectDTO = new ProjectCreateDTO { Deadline = DateTime.UtcNow.AddDays(10) };
            var expectedId = Guid.NewGuid();
            _mockProjectRepository.Setup(repo => repo.CreateProjectAsync(projectDTO)).ReturnsAsync(expectedId);

            var result = await _projectService.CreateProjectAsync(projectDTO);

            Assert.Equal(expectedId, result);
        }

        [Fact]
        public async Task DeleteProjectAsync_ShouldReturnTrue_WhenDeletionSucceeds()
        {
            var projectId = Guid.NewGuid();
            _mockProjectRepository.Setup(repo => repo.DeleteProjectAsync(projectId)).ReturnsAsync(true);

            var result = await _projectService.DeleteProjectAsync(projectId);

            Assert.True(result);
        }

        [Fact]
        public async Task GetProjectAsync_ShouldReturnProject_WhenExists()
        {
            var projectId = Guid.NewGuid();
            var projectDTO = new ProjectDTO { Id = projectId };
            _mockProjectRepository.Setup(repo => repo.GetProjectAsync(projectId)).ReturnsAsync(projectDTO);

            var result = await _projectService.GetProjectAsync(projectId);

            Assert.NotNull(result);
            Assert.Equal(projectId, result.Id);
        }

        [Fact]
        public async Task GetProjectsByFreelancerIdAsync_ShouldReturnListOfProjects()
        {
            var freelancerId = Guid.NewGuid();
            var projects = new List<ProjectDTO> { new ProjectDTO(), new ProjectDTO() };
            _mockProjectRepository.Setup(repo => repo.GetProjectsByFreelancerIdAsync(freelancerId)).ReturnsAsync(projects);

            var result = await _projectService.GetProjectsByFreelancerIdAsync(freelancerId);

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task UpdateProjectAsync_ShouldThrowArgumentException_WhenDeadlineIsInThePast()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var updatedProject = new ProjectDTO { Deadline = DateTime.UtcNow.AddDays(-1) };

            //Act
            var updated = await _projectService.UpdateProjectAsync(projectId, updatedProject);
            // Assert
            Assert.False(updated);
            
        
        }

        [Fact]
        public async Task UpdateProjectAsync_ShouldThrowArgumentException_WhenProjectDoesNotExist()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var updatedProject = new ProjectDTO { Deadline = DateTime.UtcNow.AddDays(10) };
            _mockProjectRepository.Setup(repo => repo.GetProjectAsync(projectId)).ReturnsAsync((ProjectDTO?)null);

            // Act
            var updated = await _projectService.UpdateProjectAsync(projectId, updatedProject);

            // Assert
            Assert.False(updated);
        }

        [Fact]
        public async Task UpdateProjectAsync_ShouldCallRepositoryUpdate_WhenValid()
        {
            var projectId = Guid.NewGuid();
            var updatedProject = new ProjectDTO { Deadline = DateTime.UtcNow.AddDays(10) };
            _mockProjectRepository.Setup(repo => repo.GetProjectAsync(projectId)).ReturnsAsync(updatedProject);

            await _projectService.UpdateProjectAsync(projectId, updatedProject);

            _mockProjectRepository.Verify(repo => repo.UpdateProjectAsync(projectId, updatedProject), Times.Once);
        }
    }
}
