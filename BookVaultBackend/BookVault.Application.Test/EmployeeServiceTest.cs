using BookVault.Application.Services;
using BookVault.Application.Interfaces.Repositories;
using BookVault.Application.Exceptions;
using Moq;
using Xunit;
using BookVault.Domain.Entities;
using BookVault.Application.Interfaces.Services;
using Microsoft.AspNetCore.Rewrite;
using BookVault.Application.Interfaces.Auth;
using BookVault.Application.DTOs.Employee.Requests;
using BookVault.Application.Authorization;

namespace BookVault.Application.Test
{
	public class EmployeeServiceTest
	{
		private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
		private readonly Mock<IUserRepository> _userRepositoryMock;
		private readonly Mock<IPasswordService> _passwordServiceMock;
		private readonly Mock<IRuleService> _ruleServiceMock;
		private readonly EmployeeService _employeeService;

		public EmployeeServiceTest()
		{
			_employeeRepositoryMock = new Mock<IEmployeeRepository>();
			_userRepositoryMock = new Mock<IUserRepository>();
			_passwordServiceMock = new Mock<IPasswordService>();
			_ruleServiceMock = new Mock<IRuleService>();

			_employeeService = new EmployeeService(
				_employeeRepositoryMock.Object,
				_userRepositoryMock.Object,
				_passwordServiceMock.Object,
				_ruleServiceMock.Object
			);
		}

		[Fact]

		public async Task GetAllAsync_ShouldReturnAllEmployees()
		{
			// Arrange
			var employees = new List<Employee>
			{
				new Employee { Id = 1, Firstname = "John", Lastname = "Doe" },
				new Employee { Id = 2, Firstname = "Jane", Lastname = "Smith" }
			};

			_employeeRepositoryMock.Setup(repo => repo.GetAllAsync())
				.ReturnsAsync(employees);

			// Act
			var result = await _employeeService.GetAllAsync();

			// Assert
			Assert.Equal(employees.Count, result.Count());
			Assert.Contains(result, r => r.Firstname == "John" && r.Lastname == "Doe");
			Assert.Contains(result, r => r.Firstname == "Jane" && r.Lastname == "Smith");

			// Verify 
			_employeeRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
		}

		[Fact]
		public async Task GetAllAsync_WhenEmloyeesNotFound_ShouldReturnEmptyList()
		{
			// Arrange


			_employeeRepositoryMock
				.Setup(repo => repo.GetAllAsync())
				.ReturnsAsync(new List<Employee>());

			// Act
			var result = await _employeeService.GetAllAsync();

			// Assert

			Assert.Empty(result);
			// Verify 

			_employeeRepositoryMock.Verify(
				repo => repo.GetAllAsync(),
				Times.Once);
		}




		[Fact]

		public async Task GetByIdAsync_ShouldThrowNotFoundException_ThrowsNotFoundException()
		{
			// Arrange
			var employeeId = 1;
			_employeeRepositoryMock
				.Setup(repo => repo.GetByIdAsync(employeeId))
				.ReturnsAsync((Employee)null);

			// Act & Assert
			var result = await _employeeService.GetByIdAsync(employeeId);


			Assert.Null(result);

			// Verify 
			_employeeRepositoryMock.Verify(
				repo => repo.GetByIdAsync(employeeId),
				Times.Once);

		}

		[Fact]
		public async Task GetByIdAsync_ShouldReturnEmployeeDetails_WhenEmployeeExists()
		{
			// Arrange
			var employeeId = 1;
			var employee = new Employee
			{
				Id = employeeId,
				Firstname = "John",
				Lastname = "Doe",
				Email = "john.doe@example.com",
				Phone = "1234567890",
				Employeeposition = "Manager",
				Hiredate = new DateOnly(2020, 1, 1),
				Enddate = null,
				Createdat = DateTime.UtcNow,
				Updatedat = DateTime.UtcNow,
				Rooms = new List<Room>(),
				Sections = new List<Section>()

			};

			_employeeRepositoryMock
				.Setup(repo => repo.GetByIdAsync(employeeId))
				.ReturnsAsync(employee);

			// Act
			var result = await _employeeService.GetByIdAsync(employeeId);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(employeeId, result.Id);
			Assert.Equal("John", result.Firstname);
			Assert.Equal("Doe", result.Lastname);

			// Verify 
			_employeeRepositoryMock
				.Verify(repo => repo.GetByIdAsync(employeeId),
				Times.Once);
		}

		// Create Async Tests

		[Fact]
		public async Task CreateAsync_ShouldReturnEmployeeDto_WhenValidInput()
		{
			// Arrange
			var createEmployeeDto = new CreateEmployeeDto
			{
				Firstname = "John",
				Lastname = "Doe",
				Email = "john.doe@example.com",
				Phone = "1234567890",
				Username = "johndoe",
				Password = "password123",
				Employeeposition = "Manager",
				Hiredate = new DateOnly(2020, 1, 1),
				Enddate = null,
			};

			// Act
			var result = await _employeeService.CreateAsync(createEmployeeDto);

			// Assert
			Assert.NotNull(result);
			Assert.Equal("John", result.Firstname);
			Assert.Equal("Doe", result.Lastname);
			Assert.Equal("john.doe@example.com", result.Email);
			Assert.Equal("1234567890", result.Phone);
			Assert.Equal("Manager", result.Employeeposition);
			Assert.Equal(new DateOnly(2020, 1, 1), result.Hiredate);
			Assert.Null(result.Enddate);

			// Verify
			_employeeRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Employee>()), Times.Once);
		}

		[Fact]
		public async Task CreateAsync_WhenFirstnameDoesNotMeetRequirements_ThrowsBadRequestException()
		{
			// Arrange
			var createEmployeeDto = new CreateEmployeeDto
			{
				Firstname = "   ", // Invalid first name
				Lastname = "Doe",
				Email = "john.doe@example.com"
			};

			// Act & Assert
			await Assert.ThrowsAsync<BadRequestException>(
				() => _employeeService.CreateAsync(createEmployeeDto));

			// Verify
			_employeeRepositoryMock.
				Verify(repo => repo.AddAsync(It.IsAny<Employee>()),
				Times.Never);
		}

		[Fact]
		public async Task CreateAsync_WhenLastnameDoesNotMeetRequirements_ThrowsBadRequestException()
		{
			// Arrange
			var createEmployeeDto = new CreateEmployeeDto
			{
				Firstname = "John",
				Lastname = "   ", // Invalid last name
				Email = "john.doe@example.com"

			};
			// Act & Assert
			await Assert.ThrowsAsync<BadRequestException>(
				() => _employeeService.CreateAsync(createEmployeeDto));

			// Verify
			_employeeRepositoryMock.
				Verify(repo => repo.AddAsync(It.IsAny<Employee>()),
				Times.Never);
		}

		[Fact]
		public async Task CreateAsync_WhenEmailDoesNotMeetRequirements_ThrowsBadRequestException()
		{
			// Arrange
			var createEmployeeDto = new CreateEmployeeDto
			{
				Firstname = "John",
				Lastname = "Doe",
				Email = "   " // Invalid email
			};
			// Act & Assert
			await Assert.ThrowsAsync<BadRequestException>(
				() => _employeeService.CreateAsync(createEmployeeDto));
			// Verify
			_employeeRepositoryMock.
				Verify(repo => repo.AddAsync(It.IsAny<Employee>()),
				Times.Never);
		}

		[Fact]
		public async Task CreateAsync_WhenUsernameDoesNotMeetRequirements_ThrowsBadRequestException()
		{
			// Arrange
			var createEmployeeDto = new CreateEmployeeDto
			{
				Firstname = "John",
				Lastname = "Doe",
				Email = "john.doe@example.com",
				Username = "   ", // Invalid username
				Password = "password123"
			};
			// Act & Assert
			await Assert.ThrowsAsync<BadRequestException>(
				() => _employeeService.CreateAsync(createEmployeeDto));
			// Verify
			_employeeRepositoryMock.
				Verify(repo => repo.AddAsync(It.IsAny<Employee>()),
				Times.Never);
		}

		[Fact]
		public async Task CreateAsync_WhenPasswordDoesNotMeetRequirements_ThrowsBadRequestException()
		{
			// Arrange
			var createEmployeeDto = new CreateEmployeeDto
			{
				Firstname = "John",
				Lastname = "Doe",
				Email = "john.doe@example.com",
				Username = "johndoe",
				Password = "   " // Invalid password
			};

			// Act & Assert
			await Assert.ThrowsAsync<BadRequestException>(
				() => _employeeService.CreateAsync(createEmployeeDto));
			// Verify
			_employeeRepositoryMock.
				Verify(repo => repo.AddAsync(It.IsAny<Employee>()),
				Times.Never);
		}

		[Fact]
		public async Task CreateAsync_WhenAllFieldsAreInvalid_ThrowsBadRequestException()
		{
			// Arrange
			var createEmployeeDto = new CreateEmployeeDto
			{
				Firstname = "   ", // Invalid first name
				Lastname = "   ", // Invalid last name
				Email = "   ", // Invalid email
				Username = "   ", // Invalid username
				Password = "   " // Invalid password
			};
			// Act & Assert
			await Assert.ThrowsAsync<BadRequestException>(
				() => _employeeService.CreateAsync(createEmployeeDto));
			// Verify
			_employeeRepositoryMock.
				Verify(repo => repo.AddAsync(It.IsAny<Employee>()),
				Times.Never);
		}

		[Fact]
		public async Task CreateAsync_WhenAllFieldsAreValid_ShouldCreateEmployee()
		{
			// Arrange
			var createEmployeeDto = new CreateEmployeeDto
			{
				Firstname = "John",
				Lastname = "Doe",
				Email = "john.doe@example.com",
				Username = "johndoe",
				Password = "password123"
			};
			// Act
			var result = await _employeeService.CreateAsync(createEmployeeDto);
			// Assert
			Assert.NotNull(result);
			Assert.Equal("John", result.Firstname);
			Assert.Equal("Doe", result.Lastname);
			Assert.Equal("john.doe@example.com", result.Email);
			// Verify
			_employeeRepositoryMock.
				Verify(repo => repo.AddAsync(It.IsAny<Employee>()),
				Times.Once);
		}

		[Fact]
		public async Task CreateAsync_WhenHiredateIsInTheFuture_ShouldThrowBadRequestException()
		{
			// Arrange
			var createEmployeeDto = new CreateEmployeeDto
			{
				Firstname = "John",
				Lastname = "Doe",
				Email = "john.doe@example.com",
				Username = "johndoe",
				Password = "password123",
				Hiredate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)) // Future hire date
			};

			// Act & Assert
			await Assert.ThrowsAsync<BadRequestException>(
				() => _employeeService.CreateAsync(createEmployeeDto));

			// Verify
			_employeeRepositoryMock.
				Verify(repo => repo.AddAsync(It.IsAny<Employee>()),
				Times.Never);
		}

		[Fact]
		public async Task CreateAsync_WhenEnddateIsEarlierThanHiredate_ShouldThrowBadRequestException()
		{
			// Arrange
			var createEmployeeDto = new CreateEmployeeDto
			{
				Firstname = "John",
				Lastname = "Doe",
				Email = "john.doe@example.com",
				Username = "johndoe",
				Password = "password123",
				Hiredate = DateOnly.FromDateTime(DateTime.UtcNow),
				Enddate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)) // End date earlier than hire date
			};
			// Act & Assert
			await Assert.ThrowsAsync<BadRequestException>(
				() => _employeeService.CreateAsync(createEmployeeDto));

			// Verify
			_employeeRepositoryMock.
				Verify(repo => repo.AddAsync(It.IsAny<Employee>()),
				Times.Never);
		}


		[Fact]
		public async Task CreateAsync_WhenEmailAlreadyExists_ShouldThrowConflictException()
		{
			// Arrange
			var createEmployeeDto = new CreateEmployeeDto
			{
				Firstname = "John",
				Lastname = "Doe",
				Email = "john.doe@example.com",
				Username = "johndoe",
				Password = "password123",
				Hiredate = DateOnly.FromDateTime(DateTime.UtcNow),
				Enddate = DateOnly.FromDateTime(DateTime.UtcNow)
			};


			var existingEmployee = new Employee
			{
				Id = 1,
				Firstname = "Existing",
				Lastname = "Employee",
				Email = "john.doe@example.com"

			};

			_employeeRepositoryMock
				.Setup(repo => repo.GetByEmailAsync(createEmployeeDto.Email))
				.ReturnsAsync(existingEmployee);

			// Act & Assert

			await Assert.ThrowsAsync<ConflictException>(
				() => _employeeService.CreateAsync(createEmployeeDto));

			//Verify 

			_employeeRepositoryMock.
				Verify(repo => repo.AddAsync(It.IsAny<Employee>()),
				Times.Never);

			_employeeRepositoryMock.
				Verify(repo => repo.SaveAsync(),
				Times.Never);
		}

		[Fact]
		public async Task CreateAsync_WhenUsernameAlreadyExists_ShouldThrowConflictException()
		{
			// Arrange
			var createEmployeeDto = new CreateEmployeeDto
			{
				Firstname = "John",
				Lastname = "Doe",
				Email = "john.doe@example.com",
				Username = "johndoe",
				Password = "password123",
				Hiredate = DateOnly.FromDateTime(DateTime.UtcNow),
				Enddate = DateOnly.FromDateTime(DateTime.UtcNow)


			};

			var existingUser = new User
			{
				Id = Guid.NewGuid(),
				Username = "johndoe"
			};



			_userRepositoryMock
				.Setup(repo => repo.GetByUsernameAsync(createEmployeeDto.Username))
				.ReturnsAsync(existingUser);


			// Act & Assert
			await Assert.ThrowsAsync<ConflictException>(
				() => _employeeService.CreateAsync(createEmployeeDto));

			// Verify

			_userRepositoryMock.Verify(
				repo => repo.AddAsync(It.IsAny<User>()),
				Times.Never);

			_userRepositoryMock.Verify(
				repo => repo.SaveChangesAsync(),
				Times.Never);

		}

		[Fact]
		public async Task CreateAsync_WhenEmployeeCreated_ShouldAssignEmployeePermissions()
		{
			// Arrange
			var createEmployeeDto = new CreateEmployeeDto
			{
				Firstname = "John",
				Lastname = "Doe",
				Email = "john.doe@example.com",
				Username = "johndoe",
				Password = "password123",
				Hiredate = DateOnly.FromDateTime(DateTime.UtcNow),
				Enddate = DateOnly.FromDateTime(DateTime.UtcNow)
			};

			var newUser = new User
			{
				Id = Guid.NewGuid(),
				Username = "johndoe"
			};

			_userRepositoryMock
				.Setup(repo => repo.AddAsync(It.IsAny<User>()))
				.Returns(Task.CompletedTask);


			// Act
			var result = await _employeeService.CreateAsync(createEmployeeDto);

			// Assert
			Assert.NotNull(result);
			Assert.Equal("John", result.Firstname);
			Assert.Equal("Doe", result.Lastname);
			Assert.Equal("john.doe@example.com", result.Email);

			// Verify

			_userRepositoryMock.Verify(
				repo => repo.AddAsync(It.IsAny<User>()),
				Times.Once);

			_employeeRepositoryMock.Verify(
				repo => repo.AddAsync(It.IsAny<Employee>()),
				Times.Once);

			_employeeRepositoryMock.Verify(
				repo => repo.SaveAsync(),
				Times.Once);

			_ruleServiceMock.Verify(
				service => service.AssignPermissionAsync(
					It.IsAny<Guid>(),
					It.IsAny<string>(),
					It.IsAny<Guid?>(),
					It.IsAny<DateTimeOffset?>()),
				Times.Exactly(35));
		}

		[Fact]
		public async Task CreateAsync_WhenEmployeeCreated_ShouldAssignPermissionsToCorrectUser()
		{
			// Arrange
			var createEmployeeDto = new CreateEmployeeDto
			{
				Firstname = "John",
				Lastname = "Doe",
				Email = "john.doe@example.com",
				Username = "johndoe",
				Password = "password123",
				Hiredate = DateOnly.FromDateTime(DateTime.UtcNow),
				Enddate = DateOnly.FromDateTime(DateTime.UtcNow)
			};

			User? createdUser = null;

			_userRepositoryMock
				.Setup(repo => repo.AddAsync(It.IsAny<User>()))
				.Callback<User>(user => createdUser = user)
				.Returns(Task.CompletedTask);

			_userRepositoryMock
				.Setup(repo => repo.GetByEmailAsync(It.IsAny<string>()))
				.ReturnsAsync((User?)null);

			_userRepositoryMock
				.Setup(repo => repo.GetByUsernameAsync(It.IsAny<string>()))
				.ReturnsAsync((User?)null);

			_passwordServiceMock
				.Setup(service => service.HashPassword(It.IsAny<User>(), It.IsAny<string>()))
				.Returns("hashedPassword");

			_employeeRepositoryMock
				.Setup(repo => repo.AddAsync(It.IsAny<Employee>()))
				.Returns(Task.CompletedTask);

			_employeeRepositoryMock
				.Setup(repo => repo.SaveAsync())
				.Returns(Task.CompletedTask);

			// Act
			var result = await _employeeService.CreateAsync(createEmployeeDto);


			// Assert
			Assert.NotNull(result);
			Assert.Equal("John", result.Firstname);
			Assert.Equal("Doe", result.Lastname);
			Assert.Equal("john.doe@example.com", result.Email);


			//Verify 
			_ruleServiceMock.Verify(
				service => service.AssignPermissionAsync(
					createdUser!.Id,
					It.IsAny<string>(),
					It.IsAny<Guid?>(),
					It.IsAny<DateTimeOffset?>()),
				Times.Exactly(35));


		}


		// Update Async Tests

		[Fact]
		public async Task UpdateAsync_WhenEmployeeDoesnotExist_ThrowsNotFoundException()
		{
			// Arrange
			var employeeId = 1;

			_employeeRepositoryMock
				.Setup(repo => repo.GetByIdAsync(employeeId))
				.ReturnsAsync((Employee)null);

			//Act & Assert

			await Assert.ThrowsAsync<NotFoundException>(
				() => _employeeService.UpdateAsync(employeeId, new UpdateEmployeeDto()));

			// Verify

			_employeeRepositoryMock.Verify(
				repo => repo.GetByIdAsync(employeeId),
				Times.Once);

		}

		[Fact]

		public async Task UpdateAsync_WhenFirstnameIsNullOrWhitespace_ThrowsBadRequestException()
		{
			// Arrange
			var employeeId = 1;
			var existingEmployee = new Employee
			{
				Id = employeeId,
				Firstname = "John",
				Lastname = "Doe"
			};

			_employeeRepositoryMock
				.Setup(repo => repo.GetByIdAsync(employeeId))
				.ReturnsAsync(existingEmployee);

			// Act & Assert
			await Assert.ThrowsAsync<BadRequestException>(
				() => _employeeService.UpdateAsync(
					employeeId,
					new UpdateEmployeeDto
					{
						Firstname = "   "
					}));

			// Verify
			_employeeRepositoryMock.Verify(
				repo => repo.GetByIdAsync(employeeId),
				Times.Once);
		}

		[Fact]

		public async Task UpdateAsync_WhenLastnameIsNullOrWhitespace_ThrowsBadRequestException()
		{
			// Arrange
			var employeeId = 1;
			var existingEmployee = new Employee
			{
				Id = employeeId,
				Firstname = "John",
				Lastname = "Doe"
			};
			_employeeRepositoryMock
				.Setup(repo => repo.GetByIdAsync(employeeId))
				.ReturnsAsync(existingEmployee);
			// Act & Assert
			await Assert.ThrowsAsync<BadRequestException>(
				() => _employeeService.UpdateAsync(
					employeeId,
					new UpdateEmployeeDto
					{
						Lastname = "   "
					}));
			// Verify
			_employeeRepositoryMock.Verify(
				repo => repo.GetByIdAsync(employeeId),
				Times.Once);
		}

		[Fact]

		public async Task UpdateAsync_WhenEmployeeEmailIsNullOrWhitespace_ThrowsBadRequestException()
		{
			// Arrange
			var employeeId = 1;
			var existingEmployee = new Employee
			{
				Id = employeeId,
				Firstname = "John",
				Lastname = "Doe",
				Email = "john.doe@example.com"
			};

			_employeeRepositoryMock
				.Setup(repo => repo.GetByIdAsync(employeeId))
				.ReturnsAsync(existingEmployee);

			var updateEmployeeDto = new UpdateEmployeeDto
			{
				Firstname = "Jane",
				Lastname = "Smith",
				Email = " "
			};
			// Act & Assert

			await Assert.ThrowsAsync<BadRequestException>(
				() => _employeeService.UpdateAsync(
					employeeId,
					new UpdateEmployeeDto
					{
						Firstname = "Jane",
						Lastname = "Smith",
						Email = " "
					}));
			// Verify

			_employeeRepositoryMock.Verify(
				repo => repo.GetByIdAsync(employeeId),
				Times.Once);
		}

		[Fact]

		public async Task UpdateAsync_WhenEmployeeHiredateIsInTheFuture_ThrowsBadRequestException()
		{
			// Arrange
			var employeeId = 1;
			var existingEmployee = new Employee
			{
				Id = employeeId,
				Firstname = "John",
				Lastname = "Doe",
				Hiredate = DateOnly.FromDateTime(DateTime.UtcNow)
			};
			_employeeRepositoryMock
				.Setup(repo => repo.GetByIdAsync(employeeId))
				.ReturnsAsync(existingEmployee);

			// Act & Assert
			await Assert.ThrowsAsync<BadRequestException>(
				() => _employeeService.UpdateAsync(
					employeeId,
					new UpdateEmployeeDto
					{
						Hiredate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)) // Future hire date
					}));

			// Verify
			_employeeRepositoryMock.Verify(
				repo => repo.GetByIdAsync(employeeId),
				Times.Once);
		}


		[Fact]
		public async Task UpdateAsync_WhenEmployeeEnddateIsEarlierThanHiredate_ThrowsBadRequestException()
		{
			// Arrange
			var employeeId = 1;
			var existingEmployee = new Employee
			{
				Id = employeeId,
				Firstname = "John",
				Lastname = "Doe",
				Hiredate = DateOnly.FromDateTime(DateTime.UtcNow)
			};

			_employeeRepositoryMock
				.Setup(repo => repo.GetByIdAsync(employeeId))
				.ReturnsAsync(existingEmployee);

			// Act & Assert
			await Assert.ThrowsAsync<BadRequestException>(
				() => _employeeService.UpdateAsync(
					employeeId,
					new UpdateEmployeeDto
					{
						Hiredate = DateOnly.FromDateTime(DateTime.UtcNow),
						Enddate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)) // End date earlier than hire date
					}));

			// Verify
			_employeeRepositoryMock.Verify(
				repo => repo.GetByIdAsync(employeeId),
				Times.Once);
		}


		[Fact]
		public async Task UpdateAsync_WhenEmployeeEmailAlreadyExists_ThrowsConflictException()
		{
			// Arrange
			var employeeId = 1;
			var existingEmployee = new Employee
			{
				Id = employeeId,
				Firstname = "John",
				Lastname = "Doe",
				Email = "john.doe@example.com"
			};

			var anotherEmployee = new Employee
			{
				Id = 2,
				Firstname = "Jane",
				Lastname = "Smith",
				Email = "jane.smith@example.com"
			};

			_employeeRepositoryMock
				.Setup(repo => repo.GetByIdAsync(employeeId))
				.ReturnsAsync(existingEmployee);

			_employeeRepositoryMock
				.Setup(repo => repo.GetByEmailAsync("jane.smith@example.com"))
				.ReturnsAsync(anotherEmployee);

			// Act & Assert

			await Assert.ThrowsAsync<ConflictException>(
				() => _employeeService.UpdateAsync(
					employeeId,
					new UpdateEmployeeDto
					{
						Firstname = "John",
						Lastname = "Doe",
						Email = "jane.smith@example.com"

					}));

			// Verify
			_employeeRepositoryMock.Verify(
				repo => repo.GetByIdAsync(employeeId),
				Times.Once);
		}

		//[Fact]
		//public async Task WhenEmployeeUserAccountNotFound_ShouldThrowNotFoundException()
		//{
		//	// Arrange
		//	var employeeId = 1;
		//	var existingEmployee = new Employee
		//	{
		//		Id = employeeId
		//	};

		//	_employeeRepositoryMock
		//		.Setup(repo => repo.GetUserAccountByEmployeeIdAsync(employeeId))
		//		.ReturnsAsync((UserAccount)null);

		//	// Act & Assert
		//	await Assert.ThrowsAsync<NotFoundException>(
		//		() => _employeeService.UpdateAsync(
		//			employeeId,
		//			new UpdateEmployeeDto
		//			{
		//				Firstname = "John",
		//				Lastname = "Doe",
		//				Email = "john.doe@example.com"
		//			}));

		//	// Verify
		//	_employeeRepositoryMock.Verify(
		//		repo => repo.GetByIdAsync(employeeId),
		//		Times.Once);
		//}


		// Delete Async Tests	


		[Fact]

		public async Task DeleteAsync_WhenEmployeeDoesNotExist_ThrowsNotFoundException()
		{
			// Arrange
			var employeeId = 1;
			_employeeRepositoryMock
				.Setup(repo => repo.GetByIdAsync(employeeId))
				.ReturnsAsync((Employee)null);
			// Act & Assert
			await Assert.ThrowsAsync<NotFoundException>(
				() => _employeeService.DeleteAsync(employeeId));
			// Verify
			_employeeRepositoryMock.Verify(
				repo => repo.GetByIdAsync(employeeId),
				Times.Once);
		}

		[Fact]

		public async Task DeleteAsync_WhenEmployeeExists_ShouldDeleteEmployeeAndUser()
		{
			// Arrange
			var employeeId = 1;
			var existingEmployee = new Employee
			{
				Id = employeeId,
				UserId = Guid.NewGuid()
			};
			var existingUser = new User
			{
				Id = existingEmployee.UserId
			};
			_employeeRepositoryMock
				.Setup(repo => repo.GetByIdAsync(employeeId))
				.ReturnsAsync(existingEmployee);
			_userRepositoryMock
				.Setup(repo => repo.GetByIdAsync(existingEmployee.UserId))
				.ReturnsAsync(existingUser);
			// Act
			await _employeeService.DeleteAsync(employeeId);
			// Assert
			_employeeRepositoryMock.Verify(
				repo => repo.Delete(existingEmployee),
				Times.Once);
			_userRepositoryMock.Verify(
				repo => repo.Delete(existingUser),
				Times.Once);
			_employeeRepositoryMock.Verify(
				repo => repo.SaveAsync(),
				Times.Once);
		}


		[Fact]

		public async Task DeleteAsync_WhenEmployeeExistsButUserDoesNotExist_ShouldDeleteEmployeeOnly()
		{
			// Arrange
			var employeeId = 1;
			var existingEmployee = new Employee
			{
				Id = employeeId,
				UserId = Guid.NewGuid()
			};


			_employeeRepositoryMock
				.Setup(repo => repo.GetByIdAsync(employeeId))
				.ReturnsAsync(existingEmployee);

			_userRepositoryMock
				.Setup(repo => repo.GetByIdAsync(existingEmployee.UserId))
				.ReturnsAsync((User)null);

			// Act
			await _employeeService.DeleteAsync(employeeId);

			// Assert

			_employeeRepositoryMock.Verify(
				repo => repo.Delete(existingEmployee),
				Times.Once);

			_userRepositoryMock.Verify(
				repo => repo.Delete(It.IsAny<User>()),
				Times.Never);

			_employeeRepositoryMock.Verify(
				repo => repo.SaveAsync(),
				Times.Once);
		}

		[Fact]

		public async Task DeleteAsync_WhenEmployeeExistsAndUserExists_ShouldDeleteBoth()
		{
			// Arrange
			var employeeId = 1;
			var existingEmployee = new Employee
			{
				Id = employeeId,
				UserId = Guid.NewGuid()
			};

			var existingUser = new User
			{
				Id = existingEmployee.UserId
			};

			_employeeRepositoryMock
				.Setup(repo => repo.GetByIdAsync(employeeId))
				.ReturnsAsync(existingEmployee);

			_userRepositoryMock
				.Setup(repo => repo.GetByIdAsync(existingEmployee.UserId))
				.ReturnsAsync(existingUser);

			// Act
			await _employeeService.DeleteAsync(employeeId);

			// Assert

			_employeeRepositoryMock.Verify(
				repo => repo.Delete(existingEmployee),
				Times.Once);

			_userRepositoryMock.Verify(
				repo => repo.Delete(existingUser),
				Times.Once);

			_employeeRepositoryMock.Verify(
				repo => repo.SaveAsync(),
				Times.Once);
		}


		

	}
}
