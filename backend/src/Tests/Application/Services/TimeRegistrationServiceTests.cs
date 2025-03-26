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
        private readonly TimeRegistrationService _sut;
        private readonly Mock<IProjectService> _mockProjectService;
        private readonly Mock<ITimeRegistrationRepository> _mockTimeRegistrationRepository;

        public TimeRegistrationServiceTests()
        {
            _mockProjectService = new Mock<IProjectService>();
            _mockTimeRegistrationRepository = new Mock<ITimeRegistrationRepository>();
            _sut = new TimeRegistrationService(_mockProjectService.Object, _mockTimeRegistrationRepository.Object);
        }

        [Fact]
        public async Task CreateTimeRegistrationAsync_ShouldReturnNull_WhenHoursWorkedIsTooLow()
        {
            //Arrange
            var timeRegistrationDTO = new TimeRegistrationCreateDTO { HoursWorked = 0.4m };

            //Act
            var created = await _sut.CreateTimeRegistrationAsync(timeRegistrationDTO);

            // Assert
            Assert.Null(created);

        }

        [Fact]
        public async Task CreateTimeRegistrationAsync_ShouldNotCreate_WhenProjectIsNull()
        {
            //Arrange
            var timeRegistrationDTO = new TimeRegistrationCreateDTO { ProjectId = Guid.NewGuid(), HoursWorked = 1m };
            _mockProjectService.Setup(ps => ps.GetProjectAsync(timeRegistrationDTO.ProjectId)).ReturnsAsync((ProjectDTO?)null);

            // Act
            var id = await _sut.CreateTimeRegistrationAsync(timeRegistrationDTO);

            // Assert
            Assert.Null(id);
            _mockTimeRegistrationRepository.Verify(repo => repo.CreateTimeRegistrationAsync(It.IsAny<TimeRegistrationCreateDTO>()), Times.Never);
        }

        [Fact]
        public async Task CreateTimeRegistrationAsync_ShouldNotCreate_WhenProjectIsCompleted()
        {
            // Arrange
            var timeRegistrationDTO = new TimeRegistrationCreateDTO { ProjectId = Guid.NewGuid(), HoursWorked = 1m };
            _mockProjectService.Setup(ps => ps.GetProjectAsync(timeRegistrationDTO.ProjectId)).ReturnsAsync(new ProjectDTO { IsCompleted = true });


            // Act
            await _sut.CreateTimeRegistrationAsync(timeRegistrationDTO);

            // Assert
            _mockTimeRegistrationRepository.Verify(repo => repo.CreateTimeRegistrationAsync(It.IsAny<TimeRegistrationCreateDTO>()), Times.Never);
        }

        [Fact]
        public async Task CreateTimeRegistrationAsync_ShouldCallRepository_WhenValid()
        {
            // Arrange
            var timeRegistrationDTO = new TimeRegistrationCreateDTO { ProjectId = Guid.NewGuid(), HoursWorked = 1m };
            _mockProjectService.Setup(ps => ps.GetProjectAsync(timeRegistrationDTO.ProjectId)).ReturnsAsync(new ProjectDTO { IsCompleted = false });

            // Act
            await _sut.CreateTimeRegistrationAsync(timeRegistrationDTO);

            // Assert
            _mockTimeRegistrationRepository.Verify(repo => repo.CreateTimeRegistrationAsync(timeRegistrationDTO), Times.Once);
        }

        [Fact]
        public async Task DeleteTimeRegistrationAsync_ShouldCallRepository()
        {
            // Arrange
            var timeRegistrationId = Guid.NewGuid();

            // Act
            await _sut.DeleteTimeRegistrationAsync(timeRegistrationId);

            // Assert
            _mockTimeRegistrationRepository.Verify(repo => repo.DeleteTimeRegistrationAsync(timeRegistrationId), Times.Once);
        }

        [Fact]
        public async Task GetTimeRegistrationAsync_ShouldReturnTimeRegistration_WhenExists()
        {
            // Arrange
            var timeRegistrationId = Guid.NewGuid();
            var expectedDTO = new TimeRegistrationDTO { Id = timeRegistrationId };
            _mockTimeRegistrationRepository.Setup(repo => repo.GetTimeRegistrationAsync(timeRegistrationId)).ReturnsAsync(expectedDTO);

            // Act
            var result = await _sut.GetTimeRegistrationAsync(timeRegistrationId);


            // Assert
            Assert.NotNull(result);
            Assert.Equal(timeRegistrationId, result.Id);
        }

        [Theory]
        [InlineData(0.4)]
        [InlineData(0.3)]
        [InlineData(0.2)]
        [InlineData(0.0)]
        public async Task UpdateTimeRegistrationAsync_ShouldReturnFalse_WhenHoursWorkedIsTooLow(decimal hoursWorked)
        {
            //Arrange
            var timeRegistrationId = Guid.NewGuid();
            var updatedDTO = new TimeRegistrationDTO { HoursWorked = hoursWorked };

            // Act 
            var updated = await _sut.UpdateTimeRegistrationAsync(timeRegistrationId, updatedDTO);

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
            // Arrange
            var timeRegistrationId = Guid.NewGuid();
            var updatedDTO = new TimeRegistrationDTO { HoursWorked = hoursWorked };

            // Act
            await _sut.UpdateTimeRegistrationAsync(timeRegistrationId, updatedDTO);

            // Assert
            _mockTimeRegistrationRepository.Verify(repo => repo.UpdateTimeRegistrationAsync(timeRegistrationId, updatedDTO), Times.Once);
        }
    }
}
