using BookVault.Application.Services;
using BookVault.Application.Interfaces.Repositories;
using BookVault.Application.Exceptions;
using Moq;
using Xunit;
using System.Security.Cryptography.X509Certificates;
using BookVault.Domain.Entities;

namespace BookVault.Application.Test
{
    public class SectionServiceTest
    {
        private readonly Mock<ISectionRepository> _sectionRepositoryMock;
        private readonly SectionService _sectionService;

        public SectionServiceTest()
        {
            _sectionRepositoryMock = new Mock<ISectionRepository>();
            _sectionService = new SectionService(
                _sectionRepositoryMock.Object
                );
        }

        [Fact]
        public async Task GetAllAsync_WhenListIsEmpty_ReturnsEmptyList()
        {
            // Arrange
            _sectionRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Section>());
            // Act
            var result = await _sectionService.GetAllAsync();
            // Assert
            Assert.Empty(result);
        }


        [Fact]
        public async Task GetAllAsync_WhenListIsNotEmpty_ReturnsListOfSectionDtos()
        {
            // Arrange
            var sections = new List<Section>
            {
                new Section { Id = 1, Title = "Section 1" },
                new Section { Id = 2, Title = "Section 2" }
            };

            _sectionRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(sections);
            
            // Act
            var result = await _sectionService.GetAllAsync();
            
            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, dto => dto.Id == 1 && dto.Title == "Section 1");
            Assert.Contains(result, dto => dto.Id == 2 && dto.Title == "Section 2");
        }

    }
}
