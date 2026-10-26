using BookVault.Application.DTOs.Authors.Requests;
using BookVault.Application.DTOs.Authors.Responses;
using BookVault.Application.DTOs.Publisher.Responses;
using BookVault.Application.Exceptions;
using BookVault.Application.Interfaces.Repositories;
using BookVault.Application.Interfaces.Services;
using BookVault.Domain.Entities;

namespace BookVault.Application.Services;

public class AuthorService : IAuthorService
{
	private readonly IAuthorRepository _authorRepository;
	private readonly IPublisherRepository _publisherRepository;

	public AuthorService(
		IAuthorRepository authorRepository,
		IPublisherRepository publisherRepository)
	{
		_authorRepository = authorRepository;
		_publisherRepository = publisherRepository;
	}

	public async Task<IEnumerable<AuthorDto>> GetAllAsync()
	{
		var authors = await _authorRepository.GetAllAsync();

		return authors.Select(x => new AuthorDto
		{
			Id = x.Id,
			Firstname = x.Firstname,
			Lastname = x.Lastname,
			Birthdate = x.Birthdate,
			Deathdate = x.Deathdate,
			Nationality = x.Nationality,
			Biography = x.Biography,
			Createdat = x.Createdat,
			Updatedat = x.Updatedat,
			Publisherid = x.Publisherid
		}).ToList();
	}

	public async Task<AuthorDetailsDto> GetByIdAsync(Guid id)
	{
		var author = await _authorRepository.GetByIdAsync(id);

		if (author is null)
		{
			throw new NotFoundException("Author not found.");
		}

		return new AuthorDetailsDto
		{
			Id = author.Id,
			Firstname = author.Firstname,
			Lastname = author.Lastname,
			Birthdate = author.Birthdate,
			Deathdate = author.Deathdate,
			Nationality = author.Nationality,
			Biography = author.Biography,
			Createdat = author.Createdat,
			Updatedat = author.Updatedat,
			Publisherid = author.Publisherid,

			Publisher = author.Publisher is null
				? null
				: new PublisherDetailsDto
				{
					Id = author.Publisher.Id,
					Publishername = author.Publisher.Publishername,
					Address = author.Publisher.Address,
					City = author.Publisher.City,
					Website = author.Publisher.Website,
					Email = author.Publisher.Email,
					Phone = author.Publisher.Phone
				}
		};
	}

	public async Task<Guid> CreateAsync(CreateAuthorDto dto)
	{
		ValidateAuthorFields(
			dto.Firstname,
			dto.Lastname,
			dto.Nationality,
			dto.Biography);

		ValidateDates(
			dto.Birthdate,
			dto.Deathdate);

		var firstname = dto.Firstname.Trim();
		var lastname = dto.Lastname.Trim();
		var nationality = dto.Nationality.Trim();
		var biography = dto.Biography.Trim();

		var existingAuthor =
			await _authorRepository.GetByNameAsync(
				firstname,
				lastname);

		if (existingAuthor is not null)
		{
			throw new ConflictException(
				"Author already exists.");
		}

		if (dto.Publisherid.HasValue)
		{
			var publisher =
				await _publisherRepository.GetByIdAsync(
					dto.Publisherid.Value);

			if (publisher is null)
			{
				throw new NotFoundException(
					"Publisher not found.");
			}
		}

		var now = GetDatabaseDateTime();

		var author = new Author
		{
			Id = Guid.NewGuid(),
			Firstname = firstname,
			Lastname = lastname,
			Birthdate = dto.Birthdate,
			Deathdate = dto.Deathdate,
			Nationality = nationality,
			Biography = biography,
			Publisherid = dto.Publisherid,
			Createdat = now,
			Updatedat = now
		};

		await _authorRepository.AddAsync(author);
		await _authorRepository.SaveAsync();

		return author.Id;
	}

	public async Task UpdateAsync(
		Guid id,
		UpdateAuthorDto dto)
	{
		var author = await _authorRepository.GetByIdAsync(id);

		if (author is null)
		{
			throw new NotFoundException(
				"Author not found.");
		}

		ValidateAuthorFields(
			dto.Firstname,
			dto.Lastname,
			dto.Nationality,
			dto.Biography);

		ValidateDates(
			dto.Birthdate,
			dto.Deathdate);

		var firstname = dto.Firstname.Trim();
		var lastname = dto.Lastname.Trim();
		var nationality = dto.Nationality.Trim();
		var biography = dto.Biography.Trim();

		var existingAuthor =
			await _authorRepository.GetByNameAsync(
				firstname,
				lastname);

		if (existingAuthor is not null &&
			existingAuthor.Id != id)
		{
			throw new ConflictException(
				"Author already exists.");
		}

		if (dto.Publisherid.HasValue)
		{
			var publisher =
				await _publisherRepository.GetByIdAsync(
					dto.Publisherid.Value);

			if (publisher is null)
			{
				throw new NotFoundException(
					"Publisher not found.");
			}
		}

		author.Firstname = firstname;
		author.Lastname = lastname;
		author.Birthdate = dto.Birthdate;
		author.Deathdate = dto.Deathdate;
		author.Nationality = nationality;
		author.Biography = biography;
		author.Publisherid = dto.Publisherid;

		author.Updatedat = GetDatabaseDateTime();

		await _authorRepository.SaveAsync();
	}

	public async Task DeleteAsync(Guid id)
	{
		var author = await _authorRepository.GetByIdAsync(id);

		if (author is null)
		{
			throw new NotFoundException(
				"Author not found.");
		}

		if (author.Books.Any())
		{
			throw new ConflictException(
				"Author cannot be deleted because books are associated with this author.");
		}

		_authorRepository.Delete(author);

		await _authorRepository.SaveAsync();
	}

	private static void ValidateAuthorFields(
		string firstname,
		string lastname,
		string nationality,
		string biography)
	{
		if (string.IsNullOrWhiteSpace(firstname))
		{
			throw new BadRequestException(
				"First name is required.");
		}

		if (string.IsNullOrWhiteSpace(lastname))
		{
			throw new BadRequestException(
				"Last name is required.");
		}

		if (string.IsNullOrWhiteSpace(nationality))
		{
			throw new BadRequestException(
				"Nationality is required.");
		}

		if (string.IsNullOrWhiteSpace(biography))
		{
			throw new BadRequestException(
				"Biography is required.");
		}
	}

	private static void ValidateDates(
		DateOnly birthdate,
		DateOnly? deathdate)
	{
		var today = DateOnly.FromDateTime(
			DateTime.UtcNow);

		if (birthdate > today)
		{
			throw new BadRequestException(
				"Birthdate cannot be in the future.");
		}

		if (!deathdate.HasValue)
		{
			return;
		}

		if (deathdate.Value > today)
		{
			throw new BadRequestException(
				"Deathdate cannot be in the future.");
		}

		if (deathdate.Value <= birthdate)
		{
			throw new BadRequestException(
				"Deathdate must be later than birthdate.");
		}
	}

	private static DateTime GetDatabaseDateTime()
	{
		return DateTime.SpecifyKind(
			DateTime.UtcNow,
			DateTimeKind.Unspecified);
	}
}