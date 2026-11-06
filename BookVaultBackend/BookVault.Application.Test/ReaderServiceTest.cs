using BookVault.Application.Services;
using BookVault.Application.Interfaces.Repositories;
using BookVault.Application.Exceptions;
using Moq;
using Xunit;
using BookVault.Application.Interfaces.Services;
using BookVault.Domain.Entities;
using BookVault.Application.DTOs.Reader.Requests;
using BookVault.Application.DTOs.Auth;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace BookVault.Application.Test
{
    public class ReaderServiceTest
    {

        private readonly Mock<IReaderRepository> _readerRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly Mock<IRuleService> _ruleServiceMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly ReaderService _readerService;
        public ReaderServiceTest()
        {
            _readerRepositoryMock = new Mock<IReaderRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _authServiceMock = new Mock<IAuthService>();
            _ruleServiceMock = new Mock<IRuleService>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _readerService = new ReaderService(
                _readerRepositoryMock.Object,
                _userRepositoryMock.Object,
                _authServiceMock.Object,
                _ruleServiceMock.Object,
                _currentUserServiceMock.Object
            );
        }

        [Fact]

        public async Task GetAllAsync_ShouldReturnAllReaders()
        {
            // Arrange
            var readers = new List<Reader>
            {
                new Reader { Id = 1, Firstname = "John", Lastname = "Doe" },
                new Reader { Id = 2, Firstname = "Jane", Lastname = "Smith" }
            };

            _readerRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(readers);

            // Act 
            var result = await _readerService.GetAllAsync();

            // Assert
            Assert.Equal(readers.Count, result.Count());
            Assert.Contains(result, r => r.Firstname == "John" && r.Lastname == "Doe");
            Assert.Contains(result, r => r.Firstname == "Jane" && r.Lastname == "Smith");


            // Verify
            _readerRepositoryMock.Verify(
                repo => repo.GetAllAsync(),
                Times.Once);
        }

        [Fact]

        public async Task GetAllAsync_WhenReadersListEmpty_ReturnsEmptyList()
        {
            // Arrange
            _readerRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Reader>());

            // Act
            var result = await _readerService.GetAllAsync();

            // Assert
            Assert.Empty(result);

            // Verify
            _readerRepositoryMock.Verify(
                repo => repo.GetAllAsync(),
                Times.Once);
        }


        [Fact]

        public async Task GetByIdAsync_ShouldReturnReaderDetails_WhenReaderExists()
        {
            // Arrange

            var readerId = 1;
            var reader = new Reader
            {
                Id = readerId,
                Firstname = "John",
                Lastname = "Doe",
                Email = "john.doe@example.com",
                Phone = "1234567890",
                Dateofbirth = new DateOnly(1991, 1, 1),
                Expirationdate = new DateOnly(2025, 1, 1),
                Createdat = DateTime.UtcNow,
                Updatedat = DateTime.UtcNow,
                Createdby = "admin",
                Updatedby = "admin",

                Borrowings = new List<Borrowing>()
            };

            _readerRepositoryMock
                .Setup(repo => repo.GetByIdAsync(readerId))
                .ReturnsAsync(reader);

            //Act 

            var result = await _readerService.GetByIdAsync(readerId);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(reader.Id, result.Id);
            Assert.Equal(reader.Firstname, result.Firstname);
            Assert.Equal(reader.Lastname, result.Lastname);
            Assert.Equal(reader.Email, result.Email);
            Assert.Equal(reader.Phone, result.Phone);
            Assert.Equal(reader.Dateofbirth, result.Dateofbirth);
            Assert.Equal(reader.Expirationdate, result.Expirationdate);
            Assert.Equal(reader.Createdat, result.Createdat);
            Assert.Equal(reader.Updatedat, result.Updatedat);
            Assert.Equal(reader.Createdby, result.Createdby);
            Assert.Equal(reader.Updatedby, result.Updatedby);
            Assert.Empty(result.Borrowings);

            // Verify
            _readerRepositoryMock.Verify(
                repo => repo.GetByIdAsync(readerId),
                Times.Once);
        }

        [Fact]

        public async Task GetByIdAsync_WhenReaderDoesNotExist_ReturnsNull()
        {
            // Arrange
            var readerId = 1;

            _readerRepositoryMock
                .Setup(repo => repo.GetByIdAsync(readerId))
                .ReturnsAsync((BookVault.Domain.Entities.Reader?)null);

            // Act
            var result = await _readerService.GetByIdAsync(readerId);

            // Assert
            Assert.Null(result);

            // Verify
            _readerRepositoryMock.Verify(
                repo => repo.GetByIdAsync(readerId),
                Times.Once);

        }

        // CreateAsync Tests

        [Fact]
        public async Task CreateAsync_WhenAuthenticatedUserNotFound_ThrowUnauthorizedAccessExpection()
        {
            // Arrange


            _currentUserServiceMock
                .Setup(c => c.Username)
                .Returns((string?)null);

            var dto = new CreateReaderDto();
            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _readerService.CreateAsync(dto));

        }

        [Fact]
        public async Task CreateAsync_WhenReaderFirstNameIsRequired_ThrowBadrequestExpection()
        {
            // Arrange
            _currentUserServiceMock
                .Setup(c => c.Username)
                .Returns("testuser");

            var dto = new CreateReaderDto
            {
                Firstname = "   ", // Invalid first name
                Lastname = "Doe",
                Email = "john.doe@example.com"
            };
            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => _readerService.CreateAsync(dto));
        }

        [Fact]
        public async Task CreateAsync_WhenReaderLastNameIsRequired_ThrowBadrequestExpection()
        {
            // Arrange

            _currentUserServiceMock
               .Setup(c => c.Username)
               .Returns("testuser");

            var dto = new CreateReaderDto
            {
                Firstname = "John",
                Lastname = " ", // Invalid last name
                Email = "john.doe@example.com"
            };

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => _readerService.CreateAsync(dto));
        }


        [Fact]

        public async Task CreateAsync_WhenReaderEmailIsRequired_ThrowBadrequestExpection()
        {
            // Arrange
            _currentUserServiceMock
               .Setup(c => c.Username)
               .Returns("testuser");
            var dto = new CreateReaderDto
            {
                Firstname = "John",
                Lastname = "Doe",
                Email = "   " // Invalid email
            };
            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => _readerService.CreateAsync(dto));
        }

        [Fact]

        public async Task CreateAsync_WhenUsernameIsRequired_ThrowBadrequestExpection()
        {
            // Arrange
            _currentUserServiceMock
               .Setup(c => c.Username)
               .Returns("testuser");
            var dto = new CreateReaderDto
            {
                Firstname = "John",
                Lastname = "Doe",
                Email = "john.doe@example.com",
                Username = "   " // Invalid username
            };
            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => _readerService.CreateAsync(dto));
        }

        [Fact]

        public async Task CreateAsync_WhenPasswordIsRequired_ThrowBadrequestExpection()
        {
            // Arrange
            _currentUserServiceMock
               .Setup(c => c.Username)
               .Returns("testuser");
            var dto = new CreateReaderDto
            {
                Firstname = "John",
                Lastname = "Doe",
                Email = "john.doe@example.com",
                Username = "johndoe",
                Password = "   " // Invalid password
            };
            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => _readerService.CreateAsync(dto));
        }

        [Fact]
        public async Task CreateAsync_WhenDateOfBirthIsInTheFuture_ThrowBadrequestExpection()
        {
            // Arrange
            _currentUserServiceMock
               .Setup(c => c.Username)
               .Returns("testuser");
            var dto = new CreateReaderDto
            {
                Firstname = "John",
                Lastname = "Doe",
                Email = "john.doe@example.com",
                Username = "johndoe",
                Password = "password123",
                Dateofbirth = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)) // Future date
            };
            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => _readerService.CreateAsync(dto));
        }

        [Fact]
        public async Task CreateAsync_WhenExpirationDateIsInThePast_ThrowBadrequestExpection()
        {
            // Arrange
            _currentUserServiceMock
               .Setup(c => c.Username)
               .Returns("testuser");
            var dto = new CreateReaderDto
            {
                Firstname = "John",
                Lastname = "Doe",
                Email = "john.doe@example.com",
                Username = "johndoe",
                Password = "password123",
                Expirationdate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)) // Past date
            };
            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => _readerService.CreateAsync(dto));
        }

        [Fact]

        public async Task CreateAsync_WhenReaderEmailAlreadyExists_ThrowsConflictExpection()
        {
            // Arrange

            _currentUserServiceMock
               .Setup(c => c.Username)
               .Returns("testuser");

            var dto = new CreateReaderDto
            {
                Firstname = "John",
                Lastname = "Doe",
                Email = "john.doe@example.com",
                Username = "johndoe",
                Password = "password123",
                Dateofbirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25)), // Valid date
                Expirationdate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1)) // Valid date
            };

            var existingReader = new BookVault.Domain.Entities.Reader
            {
                Id = 1,
                Firstname = "John",
                Lastname = "Doe",
                Email = "john.doe@example.com"
            };


            _readerRepositoryMock
                .Setup(r => r.GetByReaderEmailAsync("john.doe@example.com"))
                .ReturnsAsync(existingReader);

            // Act & Assert

            await Assert.ThrowsAsync<ConflictException>(
                () => _readerService.CreateAsync(dto));

            // Verify
            _readerRepositoryMock.Verify(
                r => r.GetByReaderEmailAsync("john.doe@example.com"),
                Times.Once);

            _authServiceMock.Verify(
                a => a.RegisterAsync(It.IsAny<RegisterRequest>()),
                Times.Never);
        }


        [Fact]
        public async Task CreateAsync_WhenAllFieldsAreValid_ShouldCreateReaderSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _currentUserServiceMock
               .Setup(c => c.Username)
               .Returns("testuser");

            _authServiceMock
                .Setup(a => a.RegisterAsync(It.IsAny<RegisterRequest>()))
                .ReturnsAsync(userId);


            var dto = new CreateReaderDto
            {
                Firstname = "John",
                Lastname = "Doe",
                Email = "john.doe@example.com",
                Username = "johndoe",
                Password = "password123",
                Dateofbirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25)), // Valid date
                Expirationdate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1)) // Valid date
            };
            // Act
            var result = await _readerService.CreateAsync(dto);
            // Assert
            Assert.NotNull(result);

            //Verify 

            _ruleServiceMock.Verify(
                r => r.AssignPermissionAsync(
                    userId, "BOOK_READ", null, null),
                Times.Once);

            _ruleServiceMock.Verify(
                r => r.AssignPermissionAsync(
                    userId, "AUTHOR_READ", null, null),
                Times.Once);

            _ruleServiceMock.Verify(
                r => r.AssignPermissionAsync(
                    userId, "PUBLISHER_READ", null, null),
                Times.Once);
        }


        // UpdateAsync Tests

        [Fact]

        public async Task UpdateAsync_WhenReaderDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var readerId = 1;

            _readerRepositoryMock
                .Setup(repo => repo.GetByIdAsync(readerId))
                .ReturnsAsync((BookVault.Domain.Entities.Reader?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(()
                => _readerService.UpdateAsync(readerId, new UpdateReaderDto()));

            // Verify
            _readerRepositoryMock.Verify(
                repo => repo.GetByIdAsync(readerId),
                Times.Once);
        }


        [Fact]
        public async Task UpdateAsync_WhenReaderFirstNameIsRequired_ThrowsBadRequestException()
        {
            // Arrange
            var readerId = 1;
            var existingReader = new BookVault.Domain.Entities.Reader
            {
                Id = readerId,
                Firstname = "John",
                Lastname = "Doe",
                Email = "john.doe@example.com"
            };

            _readerRepositoryMock
                .Setup(repo => repo.GetByIdAsync(readerId))
                .ReturnsAsync(existingReader);


            // Act & Assert

            await Assert.ThrowsAsync<BadRequestException>(
                () => _readerService.UpdateAsync(
                    readerId,
                    new UpdateReaderDto
                    {
                        Firstname = "   ", // Invalid first name
                        Lastname = "Doe",
                        Email = "john.doe@example.com"

                    }));

            // Verify
            _readerRepositoryMock.Verify(
                repo => repo.GetByIdAsync(readerId),
                Times.Once);
        }


        [Fact]
        public async Task UpdateAsync_WhenReaderLastNameIsRequired_ThrowsBadRequestException()
        {
            // Arrange
            var readerId = 1;
            var existingReader = new BookVault.Domain.Entities.Reader
            {
                Id = readerId,
                Firstname = "John",
                Lastname = "Doe",
                Email = "john.doe@example.com"
            };

            _readerRepositoryMock
                .Setup(repo => repo.GetByIdAsync(readerId))
                .ReturnsAsync(existingReader);


            // Act & Assert

            await Assert.ThrowsAsync<BadRequestException>(
                () => _readerService.UpdateAsync(
                    readerId,
                    new UpdateReaderDto
                    {
                        Firstname = "John",
                        Lastname = "   ", // Invalid last name
                        Email = "john.doe@example.com"

                    }));

            // Verify
            _readerRepositoryMock.Verify(
                repo => repo.GetByIdAsync(readerId),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenReaderEmailIsRequired_ThrowsBadRequestException()
        {
            // Arrange
            var readerId = 1;
            var existingReader = new BookVault.Domain.Entities.Reader
            {
                Id = readerId,
                Firstname = "John",
                Lastname = "Doe",
                Email = "john.doe@example.com"
            };

            _readerRepositoryMock
                .Setup(repo => repo.GetByIdAsync(readerId))
                .ReturnsAsync(existingReader);


            // Act & Assert

            await Assert.ThrowsAsync<BadRequestException>(
                () => _readerService.UpdateAsync(
                    readerId,
                    new UpdateReaderDto
                    {
                        Firstname = "John",
                        Lastname = "Doe",
                        Email = "   " // Invalid email

                    }));

            // Verify
            _readerRepositoryMock.Verify(
                repo => repo.GetByIdAsync(readerId),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenReaderBirthdayCannotBeInFuture_ThrowsBadRequestException()
        {
            // Arrange
            var readerId = 1;
            var existingReader = new BookVault.Domain.Entities.Reader
            {
                Id = readerId,
                Firstname = "John",
                Lastname = "Doe",
                Email = "john.doe@example.com",
                Dateofbirth = new DateOnly(1990, 1, 1)
            };

            _readerRepositoryMock
                .Setup(repo => repo.GetByIdAsync(readerId))
                .ReturnsAsync(existingReader);


            // Act & Assert

            await Assert.ThrowsAsync<BadRequestException>(
                () => _readerService.UpdateAsync(
                    readerId,
                    new UpdateReaderDto
                    {
                        Firstname = "John",
                        Lastname = "Doe",
                        Email = "john.doe@example.com",
                        Dateofbirth = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)) // Future date

                    }));

            // Verify
            _readerRepositoryMock.Verify(
                repo => repo.GetByIdAsync(readerId),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenExpirationdateIsInThePast_ThrowsBadRequestException()
        {
            // Arrange
            var readerId = 1;
            var existingReader = new BookVault.Domain.Entities.Reader
            {
                Id = readerId,
                Firstname = "John",
                Lastname = "Doe",
                Email = "john.doe@example.com",
                Dateofbirth = new DateOnly(1990, 1, 1),
                Expirationdate = new DateOnly(2025, 1, 1)
            };

            _readerRepositoryMock
                .Setup(repo => repo.GetByIdAsync(readerId))
                .ReturnsAsync(existingReader);


            // Act & Assert

            await Assert.ThrowsAsync<BadRequestException>(
                () => _readerService.UpdateAsync(
                    readerId,
                    new UpdateReaderDto
                    {
                        Firstname = "John",
                        Lastname = "Doe",
                        Email = "john.doe@example.com",
                        Dateofbirth = new DateOnly(1990, 1, 1),
                        Expirationdate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)) // Past date

                    }));

            // Verify
            _readerRepositoryMock.Verify(
                repo => repo.GetByIdAsync(readerId),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenAuthenticationFails_ThrowsUnauthorizedException()
        {
            // Arrange
            var readerId = 1;
            var existingReader = new BookVault.Domain.Entities.Reader
            {
                Id = readerId,
                Firstname = "John",
                Lastname = "Doe",
                Email = "john.doe@example.com",
                Dateofbirth = new DateOnly(1990, 1, 1),
                Expirationdate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1)),
                UserId = Guid.NewGuid()
            };

            _readerRepositoryMock
                .Setup(repo => repo.GetByIdAsync(readerId))
                .ReturnsAsync(existingReader);

            _currentUserServiceMock
                .Setup(current => current.Username)
                .Returns((string?)null);


            // Act & Assert

            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _readerService.UpdateAsync(
                    readerId,
                    new UpdateReaderDto
                    {
                        Firstname = "John",
                        Lastname = "Doe",
                        Email = "john.doe@example.com",
                        Dateofbirth = new DateOnly(1990, 1, 1),
                        Expirationdate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1))
                    }));

            // Verify
            _readerRepositoryMock.Verify(
                repo => repo.GetByIdAsync(readerId),
                Times.Once);
        }



        [Fact]
        public async Task UpdateAsync_WhenEmailAlreadyExists_ThrowsUnauthorizedException()
        {
            // Arrange
            var readerId = 1;

            var existingReader = new BookVault.Domain.Entities.Reader
            {
                Id = readerId,
                Firstname = "John",
                Lastname = "Doe",
                Email = "john.doe@example.com",
                Dateofbirth = new DateOnly(1990, 1, 1),
                Expirationdate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1)),
                UserId = Guid.NewGuid()
            };

            var anotherReader = new BookVault.Domain.Entities.Reader
            {
                Id = 2,
                Firstname = "Jane",
                Lastname = "Smith",
                Email = "jane.smith@example.com"
            };


            _readerRepositoryMock
                .Setup(repo => repo.GetByIdAsync(readerId))
                .ReturnsAsync(existingReader);

            _currentUserServiceMock
                .Setup(current => current.Username)
                .Returns("testuser");

            _readerRepositoryMock
                .Setup(repo => repo.GetByReaderEmailAsync("john.doe@example.com"))
                .ReturnsAsync(anotherReader);



            // Act & Assert

            await Assert.ThrowsAsync<ConflictException>(
                () => _readerService.UpdateAsync(
                    readerId,
                    new UpdateReaderDto
                    {
                        Firstname = "John",
                        Lastname = "Doe",
                        Email = "john.doe@example.com",
                        Dateofbirth = new DateOnly(1990, 1, 1),
                        Expirationdate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1))
                    }));

            // Verify
            _readerRepositoryMock.Verify(
                repo => repo.GetByIdAsync(readerId),
                Times.Once);
        }

        // Delete Async Tests


        [Fact]
        public async Task DeleteAsync_WhenReaderDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var readerId = 1;

            _readerRepositoryMock
                .Setup(repo => repo.GetByIdAsync(readerId))
                .ReturnsAsync((BookVault.Domain.Entities.Reader?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(()
                => _readerService.DeleteAsync(readerId));

            // Verify
            _readerRepositoryMock.Verify(
                repo => repo.GetByIdAsync(readerId),
                Times.Once);


        }

        [Fact]
        public async Task DeleteAsync_WhenReaderExists_ShouldDeleteReaderSuccessfully()
        {
            // Arrange
            var readerId = 1;
            var userId = Guid.NewGuid();

            var existingReader = new BookVault.Domain.Entities.Reader
            {
                Id = readerId,
                Firstname = "John",
                Lastname = "Doe",
                Email = "john.doe@example.com",
                UserId = userId

            };

            _readerRepositoryMock
                .Setup(repo => repo.GetByIdAsync(readerId))
                .ReturnsAsync(existingReader);


            _userRepositoryMock
               .Setup(repo => repo.GetByIdAsync(userId))
               .ReturnsAsync(new User { Id = userId });

            // Act
            await _readerService.DeleteAsync(readerId);
            // Assert



            _readerRepositoryMock.Verify(
                repo => repo.Delete(existingReader),
                Times.Once);
        }

        [Fact]

        public async Task DeleteAsync_WhenReaderUserAcoountDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var readerId = 1;
            var userId = Guid.NewGuid();
            var existingReader = new BookVault.Domain.Entities.Reader
            {
                Id = readerId,
                Firstname = "John",
                Lastname = "Doe",
                Email = "john.doe@example.com",
                UserId = userId
            };


            _readerRepositoryMock
                .Setup(repo => repo.GetByIdAsync(readerId))
                .ReturnsAsync(existingReader);

            _userRepositoryMock
                .Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync((User?)null);

            //Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(()
                => _readerService.DeleteAsync(readerId));

            // Verify
            _readerRepositoryMock.Verify(
                repo => repo.GetByIdAsync(readerId),
                Times.Once);
            _userRepositoryMock.Verify(
                repo => repo.GetByIdAsync(userId),
                Times.Once);

        }



        [Fact]

        public async Task DeleteAsync_WhenReaderHasBorrowings_ShouldSoftDeleteUser()
        {
            // Arrange
            var readerId = 1;
            var userId = Guid.NewGuid();

            var existingReader = new BookVault.Domain.Entities.Reader
            {
                Id = readerId,
                UserId = userId,
                Borrowings = new List<Borrowing>
                    {
                        new Borrowing
                        {
                            Id = 1,
                            Readerid = readerId
                        }
                    }
            };

            var existingUser = new User
			{
				Id = userId,
				Isactive = true,
				Isdeleted = false
			};

			_readerRepositoryMock
				.Setup(repo => repo.GetByIdAsync(readerId))
				.ReturnsAsync(existingReader);

			_userRepositoryMock
				.Setup(repo => repo.GetByIdAsync(userId))
				.ReturnsAsync(existingUser);
			
            // Act
			   await _readerService.DeleteAsync(readerId);
			// Assert

			Assert.False(existingUser.Isactive);
			Assert.True(existingUser.Isdeleted);

			// Verify

			_readerRepositoryMock.Verify(
		        repo => repo.Delete(It.IsAny<Reader>()),
		        Times.Never);

			_userRepositoryMock.Verify(
				repo => repo.Delete(It.IsAny<User>()),
				Times.Never);

			_readerRepositoryMock.Verify(
				repo => repo.SaveAsync(),
				Times.Once);


		}
		
        [Fact]
        public async Task DeleteAsync_WhenReaderHasNoBorrowings_ShouldDeleteReaderAndUser()
		{
			// Arrange
			var readerId = 1;
			var userId = Guid.NewGuid();
			
            var existingReader = new BookVault.Domain.Entities.Reader
			{
				Id = readerId,
				UserId = userId,
				Borrowings = new List<Borrowing>() // No borrowings
			};

			var existingUser = new User
			{
				Id = userId,
				Isactive = true,
				Isdeleted = false
			};
			
            _readerRepositoryMock
				.Setup(repo => repo.GetByIdAsync(readerId))
				.ReturnsAsync(existingReader);
			
            _userRepositoryMock
				.Setup(repo => repo.GetByIdAsync(userId))
				.ReturnsAsync(existingUser);
			
            // Act
			await _readerService.DeleteAsync(readerId);
			
            // Assert

            Assert.True(existingUser.Isactive);
            Assert.False(existingUser.Isdeleted);

			_readerRepositoryMock.Verify(
				repo => repo.Delete(existingReader),
				Times.Once);
			
            _userRepositoryMock.Verify(
				repo => repo.Delete(existingUser),
				Times.Once);
			
            _readerRepositoryMock.Verify(
				repo => repo.SaveAsync(),
				Times.Once);
		}

	}

}
