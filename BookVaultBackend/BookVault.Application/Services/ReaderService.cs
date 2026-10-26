
using BookVault.Application.DTOs.Auth;
using BookVault.Application.DTOs.Reader.Requests;
using BookVault.Application.DTOs.Reader.Responses;
using BookVault.Application.Exceptions;
using BookVault.Application.Interfaces.Repositories;
using BookVault.Application.Interfaces.Services;
using BookVault.Domain.Entities;

namespace BookVault.Application.Services;

public class ReaderService : IReaderService
{
	private readonly IReaderRepository _readerRepository;
	private readonly IUserRepository _userRepository;
	private readonly IAuthService _authService;
	private readonly IRuleService _ruleService;
	private readonly ICurrentUserService _currentUserService;

	public ReaderService(
		IReaderRepository readerRepository,
		IUserRepository userRepository,
		IAuthService authService,
		IRuleService ruleService,
		ICurrentUserService currentUserService)
	{
		_readerRepository = readerRepository;
		_userRepository = userRepository;
		_authService = authService;
		_ruleService = ruleService;
		_currentUserService = currentUserService;
	}

	public async Task<IEnumerable<ReaderDto>> GetAllAsync()
	{
		var readers = await _readerRepository.GetAllAsync();

		return readers.Select(MapToDto).ToList();
	}

	public async Task<ReaderDetailsDto?> GetByIdAsync(int id)
	{
		var reader = await _readerRepository.GetByIdAsync(id);

		if (reader == null)
			return null;

		return new ReaderDetailsDto
		{
			Id = reader.Id,
			Firstname = reader.Firstname,
			Lastname = reader.Lastname,
			Email = reader.Email,
			Phone = reader.Phone,
			Dateofbirth = reader.Dateofbirth,
			Expirationdate = reader.Expirationdate,
			Createdat = reader.Createdat,
			Updatedat = reader.Updatedat,
			Createdby = reader.Createdby,
			Updatedby = reader.Updatedby,

			Borrowings = reader.Borrowings
				.Select(MapBorrowingToDto)
				.ToList()
		};
	}

	public async Task<ReaderDto> CreateAsync(CreateReaderDto dto)
	{
		var currentUsername = _currentUserService.Username;

		if (string.IsNullOrWhiteSpace(currentUsername))
		{
			throw new UnauthorizedAccessException(
				"Authenticated user information is missing.");
		}

		if (string.IsNullOrWhiteSpace(dto.Firstname))
		{
			throw new BadRequestException(
				"Reader first name is required.");
		}

		if (string.IsNullOrWhiteSpace(dto.Lastname))
		{
			throw new BadRequestException(
				"Reader last name is required.");
		}

		if (string.IsNullOrWhiteSpace(dto.Email))
		{
			throw new BadRequestException(
				"Reader email is required.");
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

		if (dto.Dateofbirth >
			DateOnly.FromDateTime(DateTime.UtcNow))
		{
			throw new BadRequestException(
				"Date of birth cannot be in the future.");
		}

		if (dto.Expirationdate <=
			DateOnly.FromDateTime(DateTime.UtcNow))
		{
			throw new BadRequestException(
				"Expiration date must be in the future.");
		}

		var email = dto.Email.Trim().ToLowerInvariant();
		var username = dto.Username.Trim();

		var existingReader =
			await _readerRepository.GetByReaderEmailAsync(email);

		if (existingReader != null)
		{
			throw new ConflictException(
				"Reader with this email already exists.");
		}

		var registerRequest = new RegisterRequest
		{
			FirstName = dto.Firstname.Trim(),
			LastName = dto.Lastname.Trim(),
			Email = email,
			UserName = username,
			Password = dto.Password
		};

		var userId =
			await _authService.RegisterAsync(registerRequest);

		var now = DateTime.SpecifyKind(
			DateTime.UtcNow,
			DateTimeKind.Unspecified);

		var reader = new Reader
		{
			Firstname = dto.Firstname.Trim(),
			Lastname = dto.Lastname.Trim(),
			Email = email,
			Phone = dto.Phone,
			Dateofbirth = dto.Dateofbirth,
			Expirationdate = dto.Expirationdate,
			UserId = userId,
			Createdby = currentUsername,
			Updatedby = currentUsername,
			Createdat = now,
			Updatedat = now
		};

		await _readerRepository.AddAsync(reader);
		await _readerRepository.SaveAsync();

		await _ruleService.AssignPermissionAsync(
			userId,
			"BOOK_READ");

		await _ruleService.AssignPermissionAsync(
			userId,
			"AUTHOR_READ");

		await _ruleService.AssignPermissionAsync(
			userId,
			"PUBLISHER_READ");

		return MapToDto(reader);
	}

	public async Task UpdateAsync(
		int id,
		UpdateReaderDto dto)
	{
		var reader =
			await _readerRepository.GetByIdAsync(id);

		if (reader == null)
		{
			throw new NotFoundException(
				"Reader not found.");
		}

		if (string.IsNullOrWhiteSpace(dto.Firstname))
		{
			throw new BadRequestException(
				"Reader first name is required.");
		}

		if (string.IsNullOrWhiteSpace(dto.Lastname))
		{
			throw new BadRequestException(
				"Reader last name is required.");
		}

		if (string.IsNullOrWhiteSpace(dto.Email))
		{
			throw new BadRequestException(
				"Reader email is required.");
		}

		if (dto.Dateofbirth >
			DateOnly.FromDateTime(DateTime.UtcNow))
		{
			throw new BadRequestException(
				"Date of birth cannot be in the future.");
		}

		if (dto.Expirationdate <=
			DateOnly.FromDateTime(DateTime.UtcNow))
		{
			throw new BadRequestException(
				"Expiration date must be in the future.");
		}

		var currentUsername = _currentUserService.Username;

		if (string.IsNullOrWhiteSpace(currentUsername))
		{
			throw new UnauthorizedAccessException(
				"Authenticated user information is missing.");
		}

		var email = dto.Email.Trim().ToLowerInvariant();

		var existingReader =
			await _readerRepository.GetByReaderEmailAsync(email);

		if (existingReader != null &&
			existingReader.Id != id)
		{
			throw new ConflictException(
				"Another reader already uses this email.");
		}

		var user =
			await _userRepository.GetByIdAsync(reader.UserId);

		if (user == null)
		{
			throw new NotFoundException(
				"Reader user account not found.");
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

		var now = DateTime.SpecifyKind(
			DateTime.UtcNow,
			DateTimeKind.Unspecified);

		reader.Firstname = dto.Firstname.Trim();
		reader.Lastname = dto.Lastname.Trim();
		reader.Email = email;
		reader.Phone = dto.Phone;
		reader.Dateofbirth = dto.Dateofbirth;
		reader.Expirationdate = dto.Expirationdate;
		reader.Updatedby = currentUsername;
		reader.Updatedat = now;

		user.Firstname = dto.Firstname.Trim();
		user.Lastname = dto.Lastname.Trim();
		user.Email = email;
		user.Updatedat = now;

		_readerRepository.Update(reader);

		await _readerRepository.SaveAsync();
	}

	public async Task DeleteAsync(int id)
	{
		var reader =
			await _readerRepository.GetByIdAsync(id);

		if (reader == null)
		{
			throw new NotFoundException(
				"Reader not found.");
		}

		var user =
			await _userRepository.GetByIdAsync(reader.UserId);

		if (user == null)
		{
			throw new NotFoundException(
				"Reader user account not found.");
		}

		var hasBorrowings =
			reader.Borrowings.Any();

		var now = DateTime.UtcNow;

		if (hasBorrowings)
		{
			// Borrowing history must remain intact.
			// Therefore the Reader and User records cannot
			// be physically deleted.

			user.Isactive = false;
			user.Isdeleted = true;
			user.Updatedat = now;

			await _readerRepository.SaveAsync();
			return;
		}

		_readerRepository.Delete(reader);
		_userRepository.Delete(user);

		await _readerRepository.SaveAsync();
	}

	private static ReaderDto MapToDto(Reader reader)
	{
		return new ReaderDto
		{
			Id = reader.Id,
			Firstname = reader.Firstname,
			Lastname = reader.Lastname,
			Email = reader.Email,
			Phone = reader.Phone,
			Dateofbirth = reader.Dateofbirth,
			Expirationdate = reader.Expirationdate,
			Createdat = reader.Createdat,
			Updatedat = reader.Updatedat,
			Createdby = reader.Createdby,
			Updatedby = reader.Updatedby,

			Borrowings = reader.Borrowings
				.Select(MapBorrowingToDto)
				.ToList()
		};
	}

	private static BookVault.Application.DTOs.Borrowings.Responses.BorrowingDto
		MapBorrowingToDto(Borrowing borrowing)
	{
		return new BookVault.Application.DTOs.Borrowings.Responses.BorrowingDto
		{
			Id = borrowing.Id,
			Duedate = borrowing.Duedate,
			Returndate = borrowing.Returndate,
			Createdat = borrowing.Createdat,
			Updatedat = borrowing.Updatedat,
			Readerid = borrowing.Readerid,
			Bookid = borrowing.Bookid
		};
	}
}

