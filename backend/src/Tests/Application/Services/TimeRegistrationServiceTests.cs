using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Application.Services;
using Core.DTOs.TimeRegistration;
using Core.Interfaces;
using Application.Interfaces;
using Core.DTOs.Project;

namespace Tests.Application.Services
{
    public class TimeRegistrationServiceTests
    {
        private readonly TimeRegistrationService _timeRegistrationService;
        private readonly Mock<IProjectService> _mockProjectService;
        private readonly Mock<ITimeRegistrationRepository> _mockTimeRegistrationRepository;

        public TimeRegistrationServiceTests()
        {
            _mockProjectService = new Mock<IProjectService>();
            _mockTimeRegistrationRepository = new Mock<ITimeRegistrationRepository>();
            _timeRegistrationService = new TimeRegistrationService(_mockProjectService.Object, _mockTimeRegistrationRepository.Object);
        }

        [Fact]
        public async Task CreateTimeRegistrationAsync_ShouldReturnNull_WhenHoursWorkedIsTooLow()
        {
            //Arrange
            var timeRegistrationDTO = new TimeRegistrationCreateDTO { HoursWorked = 0.4m };

            //Act
            var created = await _timeRegistrationService.CreateTimeRegistrationAsync(timeRegistrationDTO);

            // Assert
            Assert.Null(created);

        }

        [Fact]
        public async Task CreateTimeRegistrationAsync_ShouldNotCreate_WhenProjectIsNull()
        {
            var timeRegistrationDTO = new TimeRegistrationCreateDTO { ProjectId = Guid.NewGuid(), HoursWorked = 1m };
            _mockProjectService.Setup(ps => ps.GetProjectAsync(timeRegistrationDTO.ProjectId)).ReturnsAsync((ProjectDTO?)null);

            var id = await _timeRegistrationService.CreateTimeRegistrationAsync(timeRegistrationDTO);

            Assert.Null(id);
            _mockTimeRegistrationRepository.Verify(repo => repo.CreateTimeRegistrationAsync(It.IsAny<TimeRegistrationCreateDTO>()), Times.Never);
        }

        [Fact]
        public async Task CreateTimeRegistrationAsync_ShouldNotCreate_WhenProjectIsCompleted()
        {
            var timeRegistrationDTO = new TimeRegistrationCreateDTO { ProjectId = Guid.NewGuid(), HoursWorked = 1m };
            _mockProjectService.Setup(ps => ps.GetProjectAsync(timeRegistrationDTO.ProjectId)).ReturnsAsync(new ProjectDTO { IsCompleted = true });

            await _timeRegistrationService.CreateTimeRegistrationAsync(timeRegistrationDTO);

            _mockTimeRegistrationRepository.Verify(repo => repo.CreateTimeRegistrationAsync(It.IsAny<TimeRegistrationCreateDTO>()), Times.Never);
        }

        [Fact]
        public async Task CreateTimeRegistrationAsync_ShouldCallRepository_WhenValid()
        {
            var timeRegistrationDTO = new TimeRegistrationCreateDTO { ProjectId = Guid.NewGuid(), HoursWorked = 1m };
            _mockProjectService.Setup(ps => ps.GetProjectAsync(timeRegistrationDTO.ProjectId)).ReturnsAsync(new ProjectDTO { IsCompleted = false });

            await _timeRegistrationService.CreateTimeRegistrationAsync(timeRegistrationDTO);

            _mockTimeRegistrationRepository.Verify(repo => repo.CreateTimeRegistrationAsync(timeRegistrationDTO), Times.Once);
        }

        [Fact]
        public async Task DeleteTimeRegistrationAsync_ShouldCallRepository()
        {
            var timeRegistrationId = Guid.NewGuid();
            await _timeRegistrationService.DeleteTimeRegistrationAsync(timeRegistrationId);
            _mockTimeRegistrationRepository.Verify(repo => repo.DeleteTimeRegistrationAsync(timeRegistrationId), Times.Once);
        }

        [Fact]
        public async Task GetTimeRegistrationAsync_ShouldReturnTimeRegistration_WhenExists()
        {
            var timeRegistrationId = Guid.NewGuid();
            var expectedDTO = new TimeRegistrationDTO { Id = timeRegistrationId };
            _mockTimeRegistrationRepository.Setup(repo => repo.GetTimeRegistrationAsync(timeRegistrationId)).ReturnsAsync(expectedDTO);

            var result = await _timeRegistrationService.GetTimeRegistrationAsync(timeRegistrationId);

            Assert.NotNull(result);
            Assert.Equal(timeRegistrationId, result.Id);
        }

        [Fact]
        public async Task UpdateTimeRegistrationAsync_ShouldReturnFalse_WhenHoursWorkedIsTooLow()
        {
            //Arrange
            var timeRegistrationId = Guid.NewGuid();
            var updatedDTO = new TimeRegistrationDTO { HoursWorked = 0.4m };

            // Act 
            var updated = await _timeRegistrationService.UpdateTimeRegistrationAsync(timeRegistrationId, updatedDTO);

            // Assert
            Assert.False(updated);
        }

        [Theory]
        [InlineData(0.5)]
        [InlineData(0.6)]
        [InlineData(4)]
        [InlineData(8)]
        public async Task UpdateTimeRegistrationAsync_ShouldCallRepository_WhenValid(decimal hoursWorked)
        {
            var timeRegistrationId = Guid.NewGuid();
            var updatedDTO = new TimeRegistrationDTO { HoursWorked = 0.5m };

            await _timeRegistrationService.UpdateTimeRegistrationAsync(timeRegistrationId, updatedDTO);

            _mockTimeRegistrationRepository.Verify(repo => repo.UpdateTimeRegistrationAsync(timeRegistrationId, updatedDTO), Times.Once);
        }
    }
}
