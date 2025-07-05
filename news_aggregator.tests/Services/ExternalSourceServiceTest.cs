using AutoMapper;
using Moq;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application;
using System.Collections.Generic;
using System.Threading.Tasks;
using news_aggregator.domain.Models.DTOs;
using news_application.Models;
using news_aggregator.shared.CustomException.ExternalSource;
using news_aggregator.tests.Helpers;
using Xunit;

namespace news_aggregator.tests.Services
{
    public class ExternalSourceServiceTest
    {
        private readonly Mock<IExternalSourceRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ExternalSourceService _externalSourceService;

        public ExternalSourceServiceTest()
        {
            _repositoryMock = new Mock<IExternalSourceRepository>();
            _mapperMock = new Mock<IMapper>();
            _externalSourceService = new ExternalSourceService(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllSourcesAsync_WhenCalled_ReturnsMappedDtos()
        {
            var externalSourceEntities = new List<ExternalSource> { ExternalSourceMockDataGenerator.GetExternalSourceEntity() };
            var externalSourceDtos = new List<ExternalSourceDto> { ExternalSourceMockDataGenerator.GetExternalSourceDto() };

            _repositoryMock.Setup(repository => repository.GetAllAsync()).ReturnsAsync(externalSourceEntities);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<ExternalSourceDto>>(externalSourceEntities)).Returns(externalSourceDtos);

            var result = await _externalSourceService.GetAllSourcesAsync();

            Assert.Single(result);
        }

        [Fact]
        public async Task GetSourceByIdAsync_WithValidId_ReturnsDto()
        {
            var externalSourceEntity = ExternalSourceMockDataGenerator.GetExternalSourceEntity();
            var externalSourceDto = ExternalSourceMockDataGenerator.GetExternalSourceDto();

            _repositoryMock.Setup(repository => repository.GetByIdAsync(externalSourceEntity.ExternalSourceId)).ReturnsAsync(externalSourceEntity);
            _mapperMock.Setup(mapper => mapper.Map<ExternalSourceDto>(externalSourceEntity)).Returns(externalSourceDto);

            var result = await _externalSourceService.GetSourceByIdAsync(externalSourceEntity.ExternalSourceId);

            Assert.Equal(externalSourceDto.ExternalSourceId, result.ExternalSourceId);
        }

        [Fact]
        public async Task GetSourceByIdAsync_WithInvalidId_ThrowsNotFoundException()
        {
            _repositoryMock.Setup(repository => repository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((ExternalSource)null!);

            await Assert.ThrowsAsync<ExternalSourceNotFoundException>(() =>
                _externalSourceService.GetSourceByIdAsync(999));
        }

        [Fact]
        public async Task AddExternalSourceApi_WithValidDto_CallsAddOnce()
        {
            var createExternalSourceDto = ExternalSourceMockDataGenerator.GetCreateExternalSourceDto();

            _repositoryMock.Setup(repository => repository.AddAsync(It.IsAny<ExternalSource>())).Returns(Task.CompletedTask);

            await _externalSourceService.AddExternalSourceApi(createExternalSourceDto);

            _repositoryMock.Verify(repository => repository.AddAsync(It.IsAny<ExternalSource>()), Times.Once);
        }

        [Fact]
        public async Task UpdateExternalSourceAsync_WithValidId_ReturnsTrue()
        {
            var externalSourceId = 1;
            var existingExternalSource = ExternalSourceMockDataGenerator.GetExternalSourceEntity(externalSourceId);
            var updatedDto = ExternalSourceMockDataGenerator.GetExternalSourceDto(externalSourceId, "UpdatedName");

            _repositoryMock.Setup(repository => repository.GetByIdAsync(externalSourceId)).ReturnsAsync(existingExternalSource);
            _repositoryMock.Setup(repository => repository.UpdateAsync(existingExternalSource)).Returns(Task.CompletedTask);

            var result = await _externalSourceService.UpdateExternalSourceAsync(externalSourceId, updatedDto);

            Assert.True(result);
        }

        [Fact]
        public async Task UpdateExternalSourceAsync_WithInvalidId_ThrowsNotFoundException()
        {
            _repositoryMock.Setup(repository => repository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((ExternalSource)null!);

            await Assert.ThrowsAsync<ExternalSourceNotFoundException>(() =>
                _externalSourceService.UpdateExternalSourceAsync(999, ExternalSourceMockDataGenerator.GetExternalSourceDto()));
        }

        [Fact]
        public async Task UpdatePartialAsync_WithValidId_UpdatesAndReturnsTrue()
        {
            var externalSourceId = 2;
            var existingExternalSource = ExternalSourceMockDataGenerator.GetExternalSourceEntity(externalSourceId);
            var updateDto = ExternalSourceMockDataGenerator.GetUpdateExternalSourceDto();

            _repositoryMock.Setup(repository => repository.GetByIdAsync(externalSourceId)).ReturnsAsync(existingExternalSource);
            _repositoryMock.Setup(repository => repository.UpdateAsync(existingExternalSource)).Returns(Task.CompletedTask);

            var result = await _externalSourceService.UpdatePartialAsync(externalSourceId, updateDto);

            Assert.True(result);
        }

        [Fact]
        public async Task UpdatePartialAsync_WithInvalidId_ThrowsNotFoundException()
        {
            _repositoryMock.Setup(repository => repository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((ExternalSource)null!);

            await Assert.ThrowsAsync<ExternalSourceNotFoundException>(() =>
                _externalSourceService.UpdatePartialAsync(999, ExternalSourceMockDataGenerator.GetUpdateExternalSourceDto()));
        }
    }
}
