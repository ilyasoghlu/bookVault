using BookVault.Application.DTOs.Employee.Requests;
using BookVault.Application.DTOs.Employee.Responses;
using BookVault.Application.DTOs.Room.Responses;
using BookVault.Application.DTOs.Section.Responses;
using BookVault.Application.Exceptions;
using BookVault.Application.Interfaces.Auth;
using BookVault.Application.Interfaces.Repositories;
using BookVault.Application.Interfaces.Services;
using BookVault.Domain.Entities;

namespace BookVault.Application.Services;

public class EmployeeService : IEmployeeService
{
	private readonly IEmployeeRepository _employeeRepository;
	private readonly IUserRepository _userRepository;
	private readonly IPasswordService _passwordService;
	private readonly IRuleService _ruleService;

	public EmployeeService(
		IEmployeeRepository employeeRepository,
		IUserRepository userRepository,
		IPasswordService passwordService,
		IRuleService ruleService)
	{
		_employeeRepository = employeeRepository;
		_userRepository = userRepository;
		_passwordService = passwordService;
		_ruleService = ruleService;
	}

	public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
	{
		var employees = await _employeeRepository.GetAllAsync();

		return employees.Select(MapToDto).ToList();
	}

	public async Task<EmployeeDetailsDto?> GetByIdAsync(int id)
	{
		var employee = await _employeeRepository.GetByIdAsync(id);

		if (employee == null)
			return null;

		return new EmployeeDetailsDto
		{
			Id = employee.Id,
			Firstname = employee.Firstname,
			Lastname = employee.Lastname,
			Email = employee.Email,
			Phone = employee.Phone,
			Employeeposition = employee.Employeeposition,
			Hiredate = employee.Hiredate,
			Enddate = employee.Enddate,
			Createdat = employee.Createdat,
			Updatedat = employee.Updatedat,

			Rooms = employee.Rooms
				.Select(MapRoomToDto)
				.ToList(),

			Sections = employee.Sections
				.Select(MapSectionToDto)
				.ToList()
		};
	}

	public async Task<EmployeeDto> CreateAsync(
		CreateEmployeeDto dto)
	{
		if (string.IsNullOrWhiteSpace(dto.Firstname))
		{
			throw new BadRequestException(
				"Employee first name is required.");
		}

		if (string.IsNullOrWhiteSpace(dto.Lastname))
		{
			throw new BadRequestException(
				"Employee last name is required.");
		}

		if (string.IsNullOrWhiteSpace(dto.Email))
		{
			throw new BadRequestException(
				"Employee email is required.");
		}

		if (string.IsNullOrWhiteSpace(dto.Username))
		{
			throw new BadRequestException(
				"Username is required.");
		}

		if (string.IsNullOrWhiteSpace(dto.Password))
		{
			throw new BadRequestException(
				"Password is required.");
		}

		if (dto.Hiredate > DateOnly.FromDateTime(DateTime.UtcNow))
		{
			throw new BadRequestException(
				"Hire date cannot be in the future.");
		}

		if (dto.Enddate.HasValue &&
			dto.Enddate.Value < dto.Hiredate)
		{
			throw new BadRequestException(
				"End date cannot be earlier than hire date.");
		}

		var email = dto.Email.Trim().ToLowerInvariant();
		var username = dto.Username.Trim();

		var existingUser =
			await _userRepository.GetByEmailAsync(email);

		if (existingUser != null)
		{
			throw new ConflictException(
				"A user with this email already exists.");
		}

		var existingUsername =
			await _userRepository.GetByUsernameAsync(username);

		if (existingUsername != null)
		{
			throw new ConflictException(
				"This username is already taken.");
		}

		var existingEmployee =
			await _employeeRepository.GetByEmailAsync(email);

		if (existingEmployee != null)
		{
			throw new ConflictException(
				"Employee with this email already exists.");
		}

		var now = GetDatabaseDateTime();

		var user = new User
		{
			Id = Guid.NewGuid(),
			Firstname = dto.Firstname.Trim(),
			Lastname = dto.Lastname.Trim(),
			Email = email,
			Username = username,
			Isactive = true,
			Isdeleted = false,
			Createdat = now
		};

		user.Passwordhash =
			_passwordService.HashPassword(
				user,
				dto.Password);

		await _userRepository.AddAsync(user);

		var employee = new Employee
		{
			UserId = user.Id,
			Firstname = dto.Firstname.Trim(),
			Lastname = dto.Lastname.Trim(),
			Email = email,
			Phone = dto.Phone,
			Employeeposition = dto.Employeeposition,
			Hiredate = dto.Hiredate,
			Enddate = dto.Enddate,
			Createdat = now,
			Updatedat = now
		};

		await _employeeRepository.AddAsync(employee);

		await _employeeRepository.SaveAsync();

		var employeePermissions = new[]
		{
			"BOOK_READ",
			"BOOK_CREATE",
			"BOOK_UPDATE",
			"BOOK_DELETE",

			"AUTHOR_READ",
			"AUTHOR_CREATE",
			"AUTHOR_UPDATE",
			"AUTHOR_DELETE",

			"PUBLISHER_READ",
			"PUBLISHER_CREATE",
			"PUBLISHER_UPDATE",
			"PUBLISHER_DELETE",

			"BOOKCASE_READ",
			"BOOKCASE_CREATE",
			"BOOKCASE_UPDATE",
			"BOOKCASE_DELETE",

			"ROOM_READ",
			"ROOM_CREATE",
			"ROOM_UPDATE",
			"ROOM_DELETE",

			"SECTION_READ",
			"SECTION_CREATE",
			"SECTION_UPDATE",
			"SECTION_DELETE",

			"SHELF_READ",
			"SHELF_CREATE",
			"SHELF_UPDATE",
			"SHELF_DELETE",

			"BORROWING_READ",
			"BORROWING_CREATE",
			"BORROWING_UPDATE",

			"READER_READ",
			"READER_CREATE",
			"READER_UPDATE",
			"READER_DELETE"
		};

		foreach (var permission in employeePermissions)
		{
			await _ruleService.AssignPermissionAsync(
				user.Id,
				permission);
		}

		return MapToDto(employee);
	}

	public async Task UpdateAsync(
		int id,
		UpdateEmployeeDto dto)
	{
		var employee =
			await _employeeRepository.GetByIdAsync(id);

		if (employee == null)
		{
			throw new NotFoundException(
				"Employee not found.");
		}

		if (string.IsNullOrWhiteSpace(dto.Firstname))
		{
			throw new BadRequestException(
				"Employee first name is required.");
		}

		if (string.IsNullOrWhiteSpace(dto.Lastname))
		{
			throw new BadRequestException(
				"Employee last name is required.");
		}

		if (string.IsNullOrWhiteSpace(dto.Email))
		{
			throw new BadRequestException(
				"Employee email is required.");
		}

		if (dto.Hiredate > DateOnly.FromDateTime(DateTime.UtcNow))
		{
			throw new BadRequestException(
				"Hire date cannot be in the future.");
		}

		if (dto.Enddate.HasValue &&
			dto.Enddate.Value < dto.Hiredate)
		{
			throw new BadRequestException(
				"End date cannot be earlier than hire date.");
		}

		var email = dto.Email.Trim().ToLowerInvariant();

		var existingEmployee =
			await _employeeRepository.GetByEmailAsync(email);

		if (existingEmployee != null &&
			existingEmployee.Id != id)
		{
			throw new ConflictException(
				"Another employee already uses this email.");
		}

		var user =
			await _userRepository.GetByIdAsync(employee.UserId);

		if (user == null)
		{
			throw new NotFoundException(
				"Employee user account not found.");
		}

		if (user.Email != email)
		{
			var existingUser =
				await _userRepository.GetByEmailAsync(email);

			if (existingUser != null &&
				existingUser.Id != user.Id)
			{
				throw new ConflictException(
					"A user with this email already exists.");
			}
		}



		var now = GetDatabaseDateTime();

		// Update Employee
		employee.Firstname = dto.Firstname.Trim();
		employee.Lastname = dto.Lastname.Trim();
		employee.Email = email;
		employee.Phone = dto.Phone;
		employee.Employeeposition = dto.Employeeposition;
		employee.Hiredate = dto.Hiredate;
		employee.Enddate = dto.Enddate;
		employee.Updatedat = now;

		// Keep linked User synchronized
		user.Firstname = dto.Firstname.Trim();
		user.Lastname = dto.Lastname.Trim();
		user.Email = email;
		user.Updatedat = now;

		_employeeRepository.Update(employee);

		await _employeeRepository.SaveAsync();
	}

	public async Task DeleteAsync(int id)
	{
		var employee =
			await _employeeRepository.GetByIdAsync(id);

		if (employee == null)
		{
			throw new NotFoundException(
				"Employee not found.");
		}

		var user =
			await _userRepository.GetByIdAsync(employee.UserId);

		_employeeRepository.Delete(employee);

		if (user != null)
		{
			_userRepository.Delete(user);
		}

		await _employeeRepository.SaveAsync();
	}

	private static EmployeeDto MapToDto(
		Employee employee)
	{
		return new EmployeeDto
		{
			Id = employee.Id,
			Firstname = employee.Firstname,
			Lastname = employee.Lastname,
			Email = employee.Email,
			Phone = employee.Phone,
			Employeeposition = employee.Employeeposition,
			Hiredate = employee.Hiredate,
			Enddate = employee.Enddate
		};
	}

	private static RoomDto MapRoomToDto(Room room)
	{
		return new RoomDto
		{
			Id = room.Id,
			Title = room.Title,
			Roomnumber = room.Roomnumber,
			Capacity = room.Capacity,
			Roomfloor = room.Roomfloor,
			Description = room.Description,
			Employeeid = room.Employeeid
		};
	}

	private static SectionDto MapSectionToDto(
		Section section)
	{
		return new SectionDto
		{
			Id = section.Id,
			Title = section.Title,
			Sectionnumber = section.Sectionnumber,
			Capacity = section.Capacity,
			Sectionfloor = section.Sectionfloor,
			Description = section.Description,
			Employeeid = section.Employeeid,
			Roomid = section.Roomid
		};
	}

	private static DateTime GetDatabaseDateTime()
	{
		return DateTime.SpecifyKind(
			DateTime.UtcNow,
			DateTimeKind.Unspecified);
	}
}

