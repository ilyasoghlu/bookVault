using BookVault.Application.Services;
using BookVault.Application.Interfaces.Repositories;
using BookVault.Application.Exceptions;
using BookVault.Application.Interfaces.Services;
using BookVault.Domain.Entities;
using Moq;
using Xunit;
using BookVault.Application.DTOs.Authors.Requests;
using BookVault.Application.DTOs.Publishers.Responses;



namespace BookVault.Application.Test
{
    public class AuthorServiceTest
    {
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<IPublisherRepository> _publisherRepositoryMock;
        private readonly AuthorService _authorService;

        public AuthorServiceTest()
        {
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _publisherRepositoryMock = new Mock<IPublisherRepository>();
            _authorService = new AuthorService(
                _authorRepositoryMock.Object,
                _publisherRepositoryMock.Object);
        }


        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfAuthorDto()
        {
            // Arrange
            var authors = new List<Author>
            {
                new Author
                {
                    Id = Guid.NewGuid(),
                    Firstname = "John",
                    Lastname = "Doe",
                    Birthdate = new DateOnly(1980, 1, 1),
                    Deathdate = null,
                    Nationality = "American",
                    Biography = "Some biography",
                    Createdat = DateTime.UtcNow,
                    Updatedat = DateTime.UtcNow,
                    Publisherid = Guid.NewGuid()
                },
                new Author
                {
                    Id = Guid.NewGuid(),
                    Firstname = "Jane",
                    Lastname = "Smith",
                    Birthdate = new DateOnly(1990, 2, 2),
                    Deathdate = null,
                    Nationality = "British",
                    Biography = "Another biography",
                    Createdat = DateTime.UtcNow,
                    Updatedat = DateTime.UtcNow,
                    Publisherid = Guid.NewGuid()
                }
            };
            _authorRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(authors);
            // Act
            var result = await _authorService.GetAllAsync();
            // Assert
            Assert.NotNull(result);
            Assert.Equal(authors.Count, result.Count());
        }



        [Fact]

        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoAuthorsExist()
        {
            // Arrange
            _authorRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Author>());
            // Act
            var result = await _authorService.GetAllAsync();
            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            //Verify
            _authorRepositoryMock.Verify(
                repo => repo.GetAllAsync(),
                Times.Once);
        }


        [Fact]
        public async Task GetByIdAsync_ShouldReturnAuthorDetailsDto_WhenAuthorExists()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var author = new Author
            {
                Id = authorId,
                Firstname = "John",
                Lastname = "Doe",
                Birthdate = new DateOnly(1980, 1, 1),
                Deathdate = null,
                Nationality = "American",
                Biography = "Some biography",
                Createdat = DateTime.UtcNow,
                Updatedat = DateTime.UtcNow,
                Publisherid = Guid.NewGuid()
            };
            _authorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(authorId))
                .ReturnsAsync(author);

            // Act
            var result = await _authorService.GetByIdAsync(authorId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(author.Id, result.Id);
            Assert.Equal(author.Firstname, result.Firstname);
            Assert.Equal(author.Lastname, result.Lastname);

            //Verify 

            _authorRepositoryMock.Verify(
                repo => repo.GetByIdAsync(authorId),
                Times.Once);
        }


        [Fact]
        public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenAuthorDoesNotExist()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            _authorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(authorId))
                .ReturnsAsync((Author)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _authorService.GetByIdAsync(authorId));

            //Verify

            _authorRepositoryMock.Verify(
                repo => repo.GetByIdAsync(authorId),
                Times.Once);
        }

        // Create Async Tests

        [Fact]

        public async Task CreateAsync_ShouldReturnAuthorId_WhenAuthorIsCreatedSuccessfully()
        {
            // Arrange

            var createAuthorDto = new CreateAuthorDto
            {
                Firstname = "John",
                Lastname = "Doe",
                Birthdate = new DateOnly(1980, 1, 1),
                Deathdate = null,
                Nationality = "American",
                Biography = "Some biography",

            };
            _authorRepositoryMock
                .Setup(repo => repo.GetByNameAsync(createAuthorDto.Firstname.Trim(), createAuthorDto.Lastname.Trim()))
                .ReturnsAsync((Author)null);


            // Act
            var result = await _authorService.CreateAsync(createAuthorDto);
            // Assert
            Assert.NotEqual(Guid.Empty, result);
            // Verify
            _authorRepositoryMock.Verify(
                repo => repo.AddAsync(It.IsAny<Author>()),
                Times.Once);
            _authorRepositoryMock.Verify(
                repo => repo.SaveAsync(),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenAuthorAlreadyExists_ShouldThrowConflictException()
        {
            // Arrange
            var createAuthorDto = new CreateAuthorDto
            {
                Firstname = "John",
                Lastname = "Doe",
                Birthdate = new DateOnly(1980, 1, 1),
                Deathdate = null,
                Nationality = "American",
                Biography = "Some biography",
            };
            var existingAuthor = new Author
            {
                Id = Guid.NewGuid(),
                Firstname = createAuthorDto.Firstname,
                Lastname = createAuthorDto.Lastname,
                Birthdate = createAuthorDto.Birthdate,
                Deathdate = createAuthorDto.Deathdate,
                Nationality = createAuthorDto.Nationality,
                Biography = createAuthorDto.Biography,
            };

            _authorRepositoryMock
                .Setup(repo => repo.GetByNameAsync(createAuthorDto.Firstname.Trim(), createAuthorDto.Lastname.Trim()))
                 .ReturnsAsync(existingAuthor);

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(() => _authorService.CreateAsync(createAuthorDto));
            // Verify
            _authorRepositoryMock.Verify(
                repo => repo.AddAsync(It.IsAny<Author>()),
                Times.Never);

            _authorRepositoryMock.Verify(
                    repo => repo.SaveAsync(),
                    Times.Never);
        }

        [Fact]
        public async Task CreateAsync_WhenFirstnameIsEmpty_ThrowsBadRequestException()
        {
            // Arrange
            var createAuthorDto = new CreateAuthorDto
            {
                Firstname = "",
                Lastname = "Doe",
                Birthdate = new DateOnly(1980, 1, 1),
                Deathdate = null,
                Nationality = "American",
                Biography = "Some biography",
            };
            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _authorService.CreateAsync(createAuthorDto));

            // Verify
            _authorRepositoryMock.Verify(
                repo => repo.AddAsync(It.IsAny<Author>()),
                Times.Never);
            _authorRepositoryMock.Verify(
                repo => repo.SaveAsync(),
                Times.Never);

        }

        [Fact]
        public async Task CreateAsync_WhenLastnameIsEmpty_ThrowsBadRequestException()
        {
            // Arrange
            var createAuthorDto = new CreateAuthorDto
            {
                Firstname = "John",
                Lastname = "",
                Birthdate = new DateOnly(1980, 1, 1),
                Deathdate = null,
                Nationality = "American",
                Biography = "Some biography",
            };
            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _authorService.CreateAsync(createAuthorDto));
            // Verify
            _authorRepositoryMock.Verify(
                repo => repo.AddAsync(It.IsAny<Author>()),
                Times.Never);
            _authorRepositoryMock.Verify(
                repo => repo.SaveAsync(),
                Times.Never);
        }

        [Fact]
        public async Task CreateAsync_WhenNationalityIsEmpty_ThrowsBadRequestException()
        {
            // Arrange
            var createAuthorDto = new CreateAuthorDto
            {
                Firstname = "John",
                Lastname = "Doe",
                Birthdate = new DateOnly(1980, 1, 1),
                Deathdate = null,
                Nationality = "",
                Biography = "Some biography",
            };
            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _authorService.CreateAsync(createAuthorDto));
            // Verify
            _authorRepositoryMock.Verify(
                repo => repo.AddAsync(It.IsAny<Author>()),
                Times.Never);
            _authorRepositoryMock.Verify(
                repo => repo.SaveAsync(),
                Times.Never);
        }

        [Fact]
        public async Task CreateAsync_WhenBiographyIsEmpty_ThrowsBadRequestException()
        {
            // Arrange
            var createAuthorDto = new CreateAuthorDto
            {
                Firstname = "John",
                Lastname = "Doe",
                Birthdate = new DateOnly(1980, 1, 1),
                Deathdate = null,
                Nationality = "American",
                Biography = "",
            };
            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _authorService.CreateAsync(createAuthorDto));
            // Verify
            _authorRepositoryMock.Verify(
                repo => repo.AddAsync(It.IsAny<Author>()),
                Times.Never);
            _authorRepositoryMock.Verify(
                repo => repo.SaveAsync(),
                Times.Never);
        }

        [Fact]
        public async Task CreateAsync_WhenBirthDateInFuture_ThrowsBadRequestExpection()
        {
            // Arrange
            var createAuthorDto = new CreateAuthorDto
            {
                Firstname = "John",
                Lastname = "Doe",
                Birthdate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), // Future date
                Deathdate = null,
                Nationality = "American",
                Biography = "Some biography",
            };
            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _authorService.CreateAsync(createAuthorDto));
            // Verify
            _authorRepositoryMock.Verify(
                repo => repo.AddAsync(It.IsAny<Author>()),
                Times.Never);
            _authorRepositoryMock.Verify(
                repo => repo.SaveAsync(),
                Times.Never);
        }

        [Fact]
        public async Task CreateAsync_WhenDeathDateBeforeBirthDate_ThrowsBadRequestExpection()
        {
            // Arrange
            var createAuthorDto = new CreateAuthorDto
            {
                Firstname = "John",
                Lastname = "Doe",
                Birthdate = new DateOnly(1980, 1, 1),
                Deathdate = new DateOnly(1970, 1, 1), // Death date before birth date
                Nationality = "American",
                Biography = "Some biography",
            };
            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _authorService.CreateAsync(createAuthorDto));
            // Verify
            _authorRepositoryMock.Verify(
                repo => repo.AddAsync(It.IsAny<Author>()),
                Times.Never);
            _authorRepositoryMock.Verify(
                repo => repo.SaveAsync(),
                Times.Never);
        }

        [Fact]

        public async Task CreateAsync_WhenDeathDateInFuture_ThrowsBadRequestExpection()
        {
            // Arrange
            var createAuthorDto = new CreateAuthorDto
            {
                Firstname = "John",
                Lastname = "Doe",
                Birthdate = new DateOnly(1980, 1, 1),
                Deathdate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), // Future date
                Nationality = "American",
                Biography = "Some biography",
            };
            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _authorService.CreateAsync(createAuthorDto));
            // Verify
            _authorRepositoryMock.Verify(
                repo => repo.AddAsync(It.IsAny<Author>()),
                Times.Never);
            _authorRepositoryMock.Verify(
                repo => repo.SaveAsync(),
                Times.Never);
        }


        // Update Async Tests

        [Fact]
        public async Task UpdateAsync_ShouldUpdateAuthor_WhenAuthorExists()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var existingAuthor = new Author
            {
                Id = authorId,
                Firstname = "John",
                Lastname = "Doe",
                Birthdate = new DateOnly(1980, 1, 1),
                Deathdate = null,
                Nationality = "American",
                Biography = "Some biography",
                Createdat = DateTime.UtcNow,
                Updatedat = DateTime.UtcNow,
                Publisherid = Guid.NewGuid()
            };
            var updateAuthorDto = new UpdateAuthorDto
            {
                Firstname = "Jane",
                Lastname = "Smith",
                Birthdate = new DateOnly(1990, 2, 2),
                Deathdate = null,
                Nationality = "British",
                Biography = "Updated biography"
            };

            _authorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(authorId))
                .ReturnsAsync(existingAuthor);
            // Act
            await _authorService.UpdateAsync(authorId, updateAuthorDto);
            // Assert
            Assert.Equal(updateAuthorDto.Firstname, existingAuthor.Firstname);
            Assert.Equal(updateAuthorDto.Lastname, existingAuthor.Lastname);
            Assert.Equal(updateAuthorDto.Birthdate, existingAuthor.Birthdate);
            Assert.Equal(updateAuthorDto.Deathdate, existingAuthor.Deathdate);
            Assert.Equal(updateAuthorDto.Nationality, existingAuthor.Nationality);
            Assert.Equal(updateAuthorDto.Biography, existingAuthor.Biography);


            // Verify
            _authorRepositoryMock.Verify(
                repo => repo.SaveAsync(),
                Times.Once);
        }


        [Fact]
        public async Task UpdateAsync_WhenAuthorDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var updateAuthorDto = new UpdateAuthorDto
            {
                Firstname = "Jane",
                Lastname = "Smith",
                Birthdate = new DateOnly(1990, 2, 2),
                Deathdate = null,
                Nationality = "British",
                Biography = "Updated biography"
            };

            _authorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(authorId))
                .ReturnsAsync((Author)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _authorService.UpdateAsync(authorId, updateAuthorDto));

            // Verify
            _authorRepositoryMock.Verify(
                repo => repo.SaveAsync(),
                Times.Never);
        }

        [Fact]

        public async Task UpdateAsync_WhenFirstnameIsEmpty_ThrowsBadRequestException()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var existingAuthor = new Author
            {
                Id = authorId,
                Firstname = "John",
                Lastname = "Doe",
                Birthdate = new DateOnly(1980, 1, 1),
                Deathdate = null,
                Nationality = "American",
                Biography = "Some biography",
                Createdat = DateTime.UtcNow,
                Updatedat = DateTime.UtcNow,
                Publisherid = Guid.NewGuid()
            };
            var updateAuthorDto = new UpdateAuthorDto
            {
                Firstname = "",
                Lastname = "Smith",
                Birthdate = new DateOnly(1990, 2, 2),
                Deathdate = null,
                Nationality = "British",
                Biography = "Updated biography"
            };
            _authorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(authorId))
                .ReturnsAsync(existingAuthor);
            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _authorService.UpdateAsync(authorId, updateAuthorDto));
            // Verify
            _authorRepositoryMock.Verify(
                repo => repo.SaveAsync(),
                Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WhenLastnameIsEmpty_ThrowsBadRequestException()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var existingAuthor = new Author
            {
                Id = authorId,
                Firstname = "John",
                Lastname = "Doe",
                Birthdate = new DateOnly(1980, 1, 1),
                Deathdate = null,
                Nationality = "American",
                Biography = "Some biography",
                Createdat = DateTime.UtcNow,
                Updatedat = DateTime.UtcNow,
                Publisherid = Guid.NewGuid()
            };
            var updateAuthorDto = new UpdateAuthorDto
            {
                Firstname = "Jane",
                Lastname = "",
                Birthdate = new DateOnly(1990, 2, 2),
                Deathdate = null,
                Nationality = "British",
                Biography = "Updated biography"
            };
            _authorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(authorId))
                .ReturnsAsync(existingAuthor);
            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _authorService.UpdateAsync(authorId, updateAuthorDto));
            // Verify
            _authorRepositoryMock.Verify(
                repo => repo.SaveAsync(),
                Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WhenBirthDateInFuture_ThrowsBadRequestException()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var existingAuthor = new Author
            {
                Id = authorId,
                Firstname = "John",
                Lastname = "Doe",
                Birthdate = new DateOnly(1980, 1, 1),
                Deathdate = null,
                Nationality = "American",
                Biography = "Some biography",
                Createdat = DateTime.UtcNow,
                Updatedat = DateTime.UtcNow,
                Publisherid = Guid.NewGuid()
            };
            var updateAuthorDto = new UpdateAuthorDto
            {
                Firstname = "Jane",
                Lastname = "Smith",
                Birthdate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), // Future date
                Deathdate = null,
                Nationality = "British",
                Biography = "Updated biography"
            };
            _authorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(authorId))
                .ReturnsAsync(existingAuthor);
            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _authorService.UpdateAsync(authorId, updateAuthorDto));
            // Verify
            _authorRepositoryMock.Verify(
                repo => repo.SaveAsync(),
                Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WhenDeathDateBeforeBirthDate_ThrowsBadRequestException()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var existingAuthor = new Author
            {
                Id = authorId,
                Firstname = "John",
                Lastname = "Doe",
                Birthdate = new DateOnly(1980, 1, 1),
                Deathdate = null,
                Nationality = "American",
                Biography = "Some biography",
                Createdat = DateTime.UtcNow,
                Updatedat = DateTime.UtcNow,
                Publisherid = Guid.NewGuid()
            };
            var updateAuthorDto = new UpdateAuthorDto
            {
                Firstname = "Jane",
                Lastname = "Smith",
                Birthdate = new DateOnly(1990, 2, 2),
                Deathdate = new DateOnly(1980, 1, 1), // Death date before birth date
                Nationality = "British",
                Biography = "Updated biography"
            };
            _authorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(authorId))
                .ReturnsAsync(existingAuthor);
            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _authorService.UpdateAsync(authorId, updateAuthorDto));
            // Verify
            _authorRepositoryMock.Verify(
                repo => repo.SaveAsync(),
                Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WhenDeathDateInFuture_ThrowsBadRequestException()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var existingAuthor = new Author
            {
                Id = authorId,
                Firstname = "John",
                Lastname = "Doe",
                Birthdate = new DateOnly(1980, 1, 1),
                Deathdate = null,
                Nationality = "American",
                Biography = "Some biography",
                Createdat = DateTime.UtcNow,
                Updatedat = DateTime.UtcNow,
                Publisherid = Guid.NewGuid()
            };
            var updateAuthorDto = new UpdateAuthorDto
            {
                Firstname = "Jane",
                Lastname = "Smith",
                Birthdate = new DateOnly(1990, 2, 2),
                Deathdate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), // Future date
                Nationality = "British",
                Biography = "Updated biography"
            };
            _authorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(authorId))
                .ReturnsAsync(existingAuthor);
            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _authorService.UpdateAsync(authorId, updateAuthorDto));
            // Verify
            _authorRepositoryMock.Verify(
                repo => repo.SaveAsync(),
                Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WhenAuthorWithSameNameExists_ThrowsConflictException()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var existingAuthor = new Author
            {
                Id = authorId,
                Firstname = "John",
                Lastname = "Doe",
                Birthdate = new DateOnly(1980, 1, 1),
                Deathdate = null,
                Nationality = "American",
                Biography = "Some biography",
                Createdat = DateTime.UtcNow,
                Updatedat = DateTime.UtcNow,
                Publisherid = Guid.NewGuid()
            };
            var updateAuthorDto = new UpdateAuthorDto
            {
                Firstname = "Jane",
                Lastname = "Smith",
                Birthdate = new DateOnly(1990, 2, 2),
                Deathdate = null,
                Nationality = "British",
                Biography = "Updated biography"
            };
            var existingAuthorWithSameName = new Author
            {
                Id = Guid.NewGuid(),
                Firstname = updateAuthorDto.Firstname,
                Lastname = updateAuthorDto.Lastname,
            };
            _authorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(authorId))
                .ReturnsAsync(existingAuthor);
            _authorRepositoryMock
                .Setup(repo => repo.GetByNameAsync(updateAuthorDto.Firstname.Trim(), updateAuthorDto.Lastname.Trim()))
                .ReturnsAsync(existingAuthorWithSameName);
            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(() => _authorService.UpdateAsync(authorId, updateAuthorDto));
            // Verify
            _authorRepositoryMock.Verify(
                repo => repo.SaveAsync(),
                Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WhenNationalityIsEmpty_ThrowsBadRequestException()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var existingAuthor = new Author
            {
                Id = authorId,
                Firstname = "John",
                Lastname = "Doe",
                Birthdate = new DateOnly(1980, 1, 1),
                Deathdate = null,
                Nationality = "American",
                Biography = "Some biography",
                Createdat = DateTime.UtcNow,
                Updatedat = DateTime.UtcNow,
                Publisherid = Guid.NewGuid()
            };
            var updateAuthorDto = new UpdateAuthorDto
            {
                Firstname = "Jane",
                Lastname = "Smith",
                Birthdate = new DateOnly(1990, 2, 2),
                Deathdate = null,
                Nationality = "",
                Biography = "Updated biography"
            };
            _authorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(authorId))
                .ReturnsAsync(existingAuthor);
            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _authorService.UpdateAsync(authorId, updateAuthorDto));
            // Verify
            _authorRepositoryMock.Verify(
                repo => repo.SaveAsync(),
                Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WhenBiographyIsEmpty_ThrowsBadRequestException()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var existingAuthor = new Author
            {
                Id = authorId,
                Firstname = "John",
                Lastname = "Doe",
                Birthdate = new DateOnly(1980, 1, 1),
                Deathdate = null,
                Nationality = "American",
                Biography = "Some biography",
                Createdat = DateTime.UtcNow,
                Updatedat = DateTime.UtcNow,
                Publisherid = Guid.NewGuid()
            };
            var updateAuthorDto = new UpdateAuthorDto
            {
                Firstname = "Jane",
                Lastname = "Smith",
                Birthdate = new DateOnly(1990, 2, 2),
                Deathdate = null,
                Nationality = "British",
                Biography = ""
            };
            _authorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(authorId))
                .ReturnsAsync(existingAuthor);
            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() => _authorService.UpdateAsync(authorId, updateAuthorDto));
            // Verify
            _authorRepositoryMock.Verify(
                repo => repo.SaveAsync(),
                Times.Never);
        }


        // Delete Async Tests

        [Fact]
        public async Task DeleteAsync_ShouldDeleteAuthor_WhenAuthorExists()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var existingAuthor = new Author
            {
                Id = authorId,
                Firstname = "John",
                Lastname = "Doe",
                Birthdate = new DateOnly(1980, 1, 1),
                Deathdate = null,
                Nationality = "American",
                Biography = "Some biography",
                Createdat = DateTime.UtcNow,
                Updatedat = DateTime.UtcNow,
                Publisherid = Guid.NewGuid()
            };
            _authorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(authorId))
                .ReturnsAsync(existingAuthor);
            // Act
            await _authorService.DeleteAsync(authorId);
            // Assert
            _authorRepositoryMock.Verify(
                repo => repo.Delete(existingAuthor),
                Times.Once);
            _authorRepositoryMock.Verify(
                repo => repo.SaveAsync(),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenAuthorDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            _authorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(authorId))
                .ReturnsAsync((Author)null);
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _authorService.DeleteAsync(authorId));
            // Verify
            _authorRepositoryMock.Verify(
                repo => repo.Delete(It.IsAny<Author>()),
                Times.Never);
            _authorRepositoryMock.Verify(
                repo => repo.SaveAsync(),
                Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_WhenBooksExistForAuthor_ThrowsConflictException()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var existingAuthor = new Author
            {
                Id = authorId,
                Firstname = "John",
                Lastname = "Doe",
                Birthdate = new DateOnly(1980, 1, 1),
                Deathdate = null,
                Nationality = "American",
                Biography = "Some biography",
                Createdat = DateTime.UtcNow,
                Updatedat = DateTime.UtcNow,
                Publisherid = Guid.NewGuid(),
                Books = new List<Book>
                {
                    new Book
                    {
                        Id = Guid.NewGuid(),
                        Title = "Sample Book",
                        Authorid = authorId,
                        Publisherid = Guid.NewGuid(),
                        Createdat = DateTime.UtcNow,
                        Updatedat = DateTime.UtcNow
                    }
                }
            };

            _authorRepositoryMock
                .Setup(repo => repo.GetByIdAsync(authorId))
                .ReturnsAsync(existingAuthor);


            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(() => _authorService.DeleteAsync(authorId));

            // Verify
            _authorRepositoryMock.Verify(
                repo => repo.Delete(It.IsAny<Author>()),
                Times.Never);
            _authorRepositoryMock.Verify(
                repo => repo.SaveAsync(),
                Times.Never);
        }
    }
}
