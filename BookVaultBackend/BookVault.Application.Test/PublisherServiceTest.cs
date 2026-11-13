using BookVault.Application.Services;
using BookVault.Application.Interfaces.Repositories;
using BookVault.Application.Exceptions;
using Moq;
using Xunit;
using System.Security.Cryptography.X509Certificates;
using BookVault.Domain.Entities;
using BookVault.Application.DTOs.Publishers.Responses;
using BookVault.Application.DTOs.Publishers.Requests;

namespace BookVault.Application.Test
{
    public class PublisherServiceTest
    {

        private readonly Mock<IPublisherRepository> _repositoryMock;
        private readonly PublisherService _service;
        private CreatePublisherDto publisherDto;

        public PublisherServiceTest()
        {
            _repositoryMock = new Mock<IPublisherRepository>();
            _service = new PublisherService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsEmptyList_WhenNoPublishersExist()
        {
            // Arrange
            _repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Publisher>());

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }


        [Fact]
        public async Task GetAllAsync_ReturnsListOfPublisherDto()
        {
            // Arrange
            var publishers = new List<Publisher>
            {
                new Publisher { Id = Guid.NewGuid(), Publishername = "Publisher 1", Address = "Address 1", City = "City 1", Website = "Website 1", Email = "Email 1", Phone = "Phone 1", Createdat = DateTime.UtcNow },
                new Publisher { Id = Guid.NewGuid(), Publishername = "Publisher 2", Address = "Address 2", City = "City 2", Website = "Website 2", Email = "Email 2", Phone = "Phone 2", Createdat = DateTime.UtcNow }
            };

            _repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(publishers);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(publishers.Count, result.Count());
        }


        [Fact]
        public async Task GetByIdAsync_NonExistingPublisher_ThrowsNotFoundException()
        {
            // Arrange
            var publisherId = Guid.NewGuid();

            _repositoryMock.Setup(repo => repo.GetByIdAsync(publisherId)).ReturnsAsync((Publisher)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(publisherId));
        }


        [Fact]

        public async Task GetByIdAsync_ExistingPublisher_ReturnsPublisherDetailsDto()
        {
            // Arrange
            var publisherId = Guid.NewGuid();
            var publisher = new Publisher
            {
                Id = publisherId,
                Publishername = "Publisher 1",
                Address = "Address 1",
                City = "City 1",
                Website = "Website 1",
                Email = "Email 1",
                Phone = "Phone 1",
                Createdat = DateTime.UtcNow,
                Authors = new List<Author>
                {
                    new Author { Id = Guid.NewGuid(), Firstname = "Author 1", Lastname = "LastName 1" },
                    new Author { Id = Guid.NewGuid(), Firstname = "Author 2", Lastname = "LastName 2" }
                }
            };
            _repositoryMock.Setup(repo => repo.GetByIdAsync(publisherId)).ReturnsAsync(publisher);
            // Act
            var result = await _service.GetByIdAsync(publisherId);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(publisher.Id, result.Id);
            Assert.Equal(publisher.Publishername, result.Publishername);
            Assert.Equal(publisher.Address, result.Address);
            Assert.Equal(publisher.City, result.City);
            Assert.Equal(publisher.Website, result.Website);
            Assert.Equal(publisher.Email, result.Email);
            Assert.Equal(publisher.Phone, result.Phone);
            Assert.Equal(publisher.Authors.Count, result.Authors.Count());
        }


        //Create Async Test

        [Fact]
        public async Task CreateAsync_ValidPublisherDto_CreatePublisherDto()
        {
            // Arrange
            var publisherDto = new CreatePublisherDto
            {
                Publishername = "Publisher 1",
                Address = "Address 1",
                City = "City 1",
                Website = "Website.com",
                Email = "email@example.com",
                Phone = "Phone 1"
            };

            _repositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Publisher>()))
                .Returns(Task.CompletedTask);


            // Act
            await _service.CreateAsync(publisherDto);

            // Assert
            _repositoryMock.Verify(repo => repo.AddAsync(It.Is<Publisher>(p =>
                p.Publishername == publisherDto.Publishername &&
                p.Address == publisherDto.Address &&
                p.City == publisherDto.City &&
                p.Website == publisherDto.Website &&
                p.Email == publisherDto.Email &&
                p.Phone == publisherDto.Phone)), Times.Once);

            // Verify

            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }


        [Fact]
        public async Task CreateAsync_InvalidPublisherDto_ThrowsBadRequestException()
        {
            // Arrange
            var publisherDto = new CreatePublisherDto
            {
                Publishername = "", // Invalid: empty name
                Address = "Address 1",
                City = "City 1",
                Website = "Website.com",
                Email = "email@example.com",
                Phone = "Phone 1"
            };

            _repositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Publisher>()))
                .Returns(Task.CompletedTask);


            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _service.CreateAsync(publisherDto));

            //Verify
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Publisher>()), Times.Never);
            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ExistingPublisher_ThrowsConflictException()
        {
            // Arrange
            var publisherDto = new CreatePublisherDto
            {
                Publishername = "Publisher 1",
                Address = "Address 1",
                City = "City 1",
                Website = "Website.com",
                Email = "email@example.com",
                Phone = "Phone 1"
            };


            _repositoryMock
                .Setup(repo => repo.GetByEmailAsync(publisherDto.Email))
                .ReturnsAsync(new Publisher { Id = Guid.NewGuid(), Publishername = "Existing Publisher" });

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(() => _service.CreateAsync(publisherDto));

            // Verify
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Publisher>()), Times.Never);
            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Never);
        }


        [Fact]
        public async Task CreateAsync_ExistingWebsite_ThrowsConflictException()
        {
            // Arrange
            var publisherDto = new CreatePublisherDto
            {
                Publishername = "Publisher 1",
                Address = "Address 1",
                City = "City 1",
                Website = "Website.com",
                Email = "email@example.com"
            };

            _repositoryMock
                .Setup(repo => repo.GetByWebsiteAsync(publisherDto.Website))
                .ReturnsAsync(new Publisher { Id = Guid.NewGuid(), Publishername = "Existing Publisher" });

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(() => _service.CreateAsync(publisherDto));

            // Verify
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Publisher>()), Times.Never);
            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Never);


        }


        // Update Async Test

        [Fact]

        public async Task UpdateAsync_ValidPublisherDto_UpdatesPublisher()
        {
            // Arrange
            var publisherId = Guid.NewGuid();
            var existingPublisher = new Publisher
            {
                Id = publisherId,
                Publishername = "Old Publisher",
                Address = "Old Address",
                City = "Old City",
                Website = "OldWebsite.com",
                Email = "oldemail@example.com"

            };

            _repositoryMock
                .Setup(repo => repo.GetByIdAsync(publisherId))
                .ReturnsAsync(existingPublisher);

            // Act
            var updateDto = new UpdatePublisherDto
            {
                Publishername = "New Publisher",
                Address = "New Address",
                City = "New City",
                Website = "NewWebsite.com",
                Email = "newemail@example.com"
            };
            await _service.UpdateAsync(publisherId, updateDto);

            // Assert
            _repositoryMock.Verify(repo => repo.Update(It.Is<Publisher>(p =>
                p.Id == publisherId &&
                p.Publishername == updateDto.Publishername &&
                p.Address == updateDto.Address &&
                p.City == updateDto.City &&
                p.Website == updateDto.Website &&
                p.Email == updateDto.Email)), Times.Once);

            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);


        }

        [Fact]

        public async Task UpdateAsync_NonExistingPublisher_ThrowsNotFoundException()
        {
            // Arrange
            var publisherId = Guid.NewGuid();
            _repositoryMock
                .Setup(repo => repo.GetByIdAsync(publisherId))
                .ReturnsAsync((Publisher)null);

            // Act & Assert
            var updateDto = new UpdatePublisherDto
            {
                Publishername = "New Publisher",
                Address = "New Address",
                City = "New City",
                Website = "NewWebsite.com",
                Email = "newemail@example.com"
            };
            await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateAsync(publisherId, updateDto));

            // Verify
            _repositoryMock.Verify(repo => repo.Update(It.IsAny<Publisher>()), Times.Never);
            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Never);

        }


        [Fact]
        public async Task UpdateAsync_ExistingPublisherWithSameEmail_ThrowsConflictException()
        {
            // Arrange
            var publisherId = Guid.NewGuid();
            var existingPublisher = new Publisher
            {
                Id = publisherId,
                Publishername = "Old Publisher",
                Address = "Old Address",
                City = "Old City",
                Website = "OldWebsite.com",
                Email = "oldemail@example.com"
            };

            var anotherPublisher = new Publisher
            {
                Id = Guid.NewGuid(),
                Publishername = "Another Publisher",
                Address = "Another Address",
                City = "Another City",
                Website = "AnotherWebsite.com",
                Email = "existing@example.com"
            };

            _repositoryMock
                .Setup(repo => repo.GetByIdAsync(publisherId))
                .ReturnsAsync(existingPublisher);

            _repositoryMock
                .Setup(repo => repo.GetByEmailAsync(anotherPublisher.Email))
                .ReturnsAsync(anotherPublisher);

            // Act
            var updateDto = new UpdatePublisherDto
            {
                Publishername = "New Publisher",
                Address = "New Address",
                City = "New City",
                Website = "NewWebsite.com",
                Email = anotherPublisher.Email // Same email as existing publisher
            };
            // Assert
            await Assert.ThrowsAsync<ConflictException>(() => _service.UpdateAsync(publisherId, updateDto));


            // Verify
            _repositoryMock.Verify(repo => repo.Update(It.IsAny<Publisher>()), Times.Never);
            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Never);


        }

        [Fact]
        public async Task UpdateAsync_ExistingPublisherWithSameWebsite_ThrowsConflictException()
        {
            // Arrange
            var publisherId = Guid.NewGuid();
            var existingPublisher = new Publisher
            {
                Id = publisherId,
                Publishername = "Old Publisher",
                Address = "Old Address",
                City = "Old City",
                Website = "OldWebsite.com",
                Email = "oldemail@example.com"
            };

            var anotherPublisher = new Publisher
            {
                Id = Guid.NewGuid(),
                Publishername = "Another Publisher",
                Address = "Another Address",
                City = "Another City",
                Website = "ExistingWebsite.com",
                Email = "anotheremail@example.com"
            };

            _repositoryMock
                .Setup(repo => repo.GetByIdAsync(publisherId))
                .ReturnsAsync(existingPublisher);

            _repositoryMock
                .Setup(repo => repo.GetByWebsiteAsync(anotherPublisher.Website))
                .ReturnsAsync(anotherPublisher);

            // Act
            var updateDto = new UpdatePublisherDto
            {
                Publishername = "New Publisher",
                Address = "New Address",
                City = "New City",
                Website = anotherPublisher.Website, // Same website as existing publisher
                Email = "newemail@example.com"
            };

            // Assert
            await Assert.ThrowsAsync<ConflictException>(() => _service.UpdateAsync(publisherId, updateDto));

            // Verify
            _repositoryMock.Verify(repo => repo.Update(It.IsAny<Publisher>()), Times.Never);
            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_InvalidPublisherDto_ThrowsBadRequestException()
        {
            // Arrange
            var publisherId = Guid.NewGuid();
            var existingPublisher = new Publisher
            {
                Id = publisherId,
                Publishername = "Old Publisher",
                Address = "Old Address",
                City = "Old City",
                Website = "OldWebsite.com",
                Email = "oldemail@example.com"
            };

            _repositoryMock
                .Setup(repo => repo.GetByIdAsync(publisherId))
                .ReturnsAsync(existingPublisher);


            // Act
            var updateDto = new UpdatePublisherDto
            {
                Publishername = "", // Invalid: empty name
                Address = "New Address",
                City = "New City",
                Website = "NewWebsite.com",
                Email = "newemail@example.com"
            };

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _service.UpdateAsync(publisherId, updateDto));

            // Verify
            _repositoryMock.Verify(repo => repo.Update(It.IsAny<Publisher>()), Times.Never);
            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_InvalidPublisherDtoWithEmptyEmail_ThrowsBadRequestException()
        {
            // Arrange
            var publisherId = Guid.NewGuid();
            var existingPublisher = new Publisher
            {
                Id = publisherId,
                Publishername = "Old Publisher",
                Address = "Old Address",
                City = "Old City",
                Website = "OldWebsite.com",
                Email = "oldemail@example.com"
            };
            _repositoryMock
                .Setup(repo => repo.GetByIdAsync(publisherId))
                .ReturnsAsync(existingPublisher);


            _repositoryMock
                .Setup(repo => repo.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((Publisher?)null);

            // Act
            var updateDto = new UpdatePublisherDto
            {
                Publishername = "New Publisher",
                Address = "New Address",
                City = "New City",
                Website = "NewWebsite.com",
                Email = "" // Invalid: empty email
            };

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _service.UpdateAsync(publisherId, updateDto));


            // Verify

            _repositoryMock.Verify(repo => repo.Update(It.IsAny<Publisher>()), Times.Never);
            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Never);
        }


        // Delete Async Test

        [Fact]
        public async Task DeleteAsync_ExistingPublisher_DeletesPublisher()
        {
            // Arrange
            var publisherId = Guid.NewGuid();
            var existingPublisher = new Publisher
            {
                Id = publisherId,
                Publishername = "Publisher to Delete",
                Address = "Address",
                City = "City",
                Website = "Website.com",
                Email = "existing@example.com"
            };

            _repositoryMock
                .Setup(repo => repo.GetByIdAsync(publisherId))
                .ReturnsAsync(existingPublisher);

            // Act
            await _service.DeleteAsync(publisherId);

            // Verify
            _repositoryMock.Verify(repo => repo.Delete(existingPublisher), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]

        public async Task DeleteAsync_NonExistingPublisher_ThrowsNotFoundException()
        {
            // Arrange
            var publisherId = Guid.NewGuid();
            _repositoryMock
                .Setup(repo => repo.GetByIdAsync(publisherId))
                .ReturnsAsync((Publisher)null);
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAsync(publisherId));
            // Verify
            _repositoryMock.Verify(repo => repo.Delete(It.IsAny<Publisher>()), Times.Never);
            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ExistingPublisherWithAuthors_ConflictException()
        {
            // Arrange
            var publisherId = Guid.NewGuid();
            var existingPublisher = new Publisher
            {
                Id = publisherId,
                Publishername = "Publisher to Delete",
                Address = "Address",
                City = "City",
                Website = "Website.com",
                Email = "existing@example.com",
                Authors = new List<Author>
    {
                new Author(),
                new Author()
    },
                Books = new List<Book>()
            };




            _repositoryMock
                .Setup(repo => repo.GetByIdAsync(publisherId))
                .ReturnsAsync(existingPublisher);




            // Act
            await Assert.ThrowsAsync<ConflictException>(
                () => _service.DeleteAsync(publisherId));

            // Verify
            _repositoryMock.Verify(repo => repo.Delete(existingPublisher), Times.Never);

            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Never);
        }


        [Fact]
        public async Task DeleteAsync_ExistingPublisherWithBooks_ConflictException()
        {
            // Arrange
            var publisherId = Guid.NewGuid();
            var existingPublisher = new Publisher
            {
                Id = publisherId,
                Publishername = "Publisher to Delete",
                Address = "Address",
                City = "City",
                Website = "Website.com",
                Email = "existing@example.com",
                Authors = new List<Author>(),
                Books = new List<Book>
                {
                    new Book(),
                    new Book()
                }
            };

            _repositoryMock
                .Setup(repo => repo.GetByIdAsync(publisherId))
                .ReturnsAsync(existingPublisher);

            // Act
            await Assert.ThrowsAsync<ConflictException>(
                () => _service.DeleteAsync(publisherId));

            // Verify
            _repositoryMock.Verify(repo => repo.Delete(existingPublisher), Times.Never);
            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Never);
        }
    }
}
