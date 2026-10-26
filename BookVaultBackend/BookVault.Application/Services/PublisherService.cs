using BookVault.Application.DTOs.Authors.Responses;
using BookVault.Application.DTOs.Books.Responses;
using BookVault.Application.DTOs.Publisher.Responses;
using BookVault.Application.DTOs.Publishers.Requests;
using BookVault.Application.DTOs.Publishers.Responses;
using BookVault.Application.Exceptions;
using BookVault.Application.Interfaces.Repositories;
using BookVault.Application.Interfaces.Services;
using BookVault.Domain.Entities;

namespace BookVault.Application.Services;

public class PublisherService : IPublisherService
{
    private readonly IPublisherRepository _repository;

    public PublisherService(IPublisherRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PublisherDto>> GetAllAsync()
    {
        var publishers = await _repository.GetAllAsync();

        return publishers.Select(x => new PublisherDto
        {
            Id = x.Id,
            Publishername = x.Publishername,
            Address = x.Address,
            City = x.City,
            Website = x.Website,
            Email = x.Email,
            Phone = x.Phone,
            Createdat = x.Createdat
        }).ToList();
    }

    public async Task<PublisherDetailsDto> GetByIdAsync(Guid id)
    {
        var publisher = await _repository.GetByIdAsync(id);

        if (publisher == null)
        {
            throw new NotFoundException(
                "Publisher not found."
            );
        }

        return new PublisherDetailsDto
        {
            Id = publisher.Id,
            Publishername = publisher.Publishername,
            Address = publisher.Address,
            City = publisher.City,
            Website = publisher.Website,
            Email = publisher.Email,
            Phone = publisher.Phone,

            Authors = publisher.Authors
                .Select(author => new AuthorDto
                {
                    Id = author.Id,
                    Firstname = author.Firstname,
                    Lastname = author.Lastname,
                    Birthdate = author.Birthdate,
                    Deathdate = author.Deathdate,
                    Nationality = author.Nationality,
                    Biography = author.Biography,
                    Createdat = author.Createdat,
                    Publisherid = author.Publisherid
                })
                .ToList(),

            Books = publisher.Books
                .Select(book => new BookDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    Isbn = book.Isbn,
                    Authorid = book.Authorid,
                    Publisherid = book.Publisherid,
                })
                .ToList()
        };
    }

    public async Task<Guid> CreateAsync(CreatePublisherDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            throw new BadRequestException(
                "Email is required."
            );
        }

        if (string.IsNullOrWhiteSpace(dto.Publishername))
        {
            throw new BadRequestException(
                "Publisher name is required."
            );
        }

        var existingPublisher =
            await _repository.GetByEmailAsync(dto.Email);

        if (existingPublisher != null)
        {
            throw new ConflictException(
                "Publisher already exists."
            );
        }

		var existingWebsite =
	        await _repository.GetByWebsiteAsync(dto.Website);

		if (existingWebsite != null)
		{
			throw new ConflictException(
				"Website is already used by another publisher."
			);
		}

		var now = DateTime.SpecifyKind(
	        DateTime.UtcNow,
	        DateTimeKind.Unspecified);

		var publisher = new Publisher
        {
            Publishername = dto.Publishername,
            Address = dto.Address,
            City = dto.City,
            Website = dto.Website,
            Email = dto.Email,
            Phone = dto.Phone,

            Createdat = now,
            Updatedat = now
        };

        await _repository.AddAsync(publisher);

        await _repository.SaveAsync();

        return publisher.Id;
    }

    public async Task UpdateAsync(
        Guid id,
        UpdatePublisherDto dto)
    {
        var publisher =
            await _repository.GetByIdAsync(id);

        if (publisher == null)
        {
            throw new NotFoundException(
                "Publisher not found."
            );
        }

        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            throw new BadRequestException(
                "Email is required."
            );
        }

        if (string.IsNullOrWhiteSpace(dto.Publishername))
        {
            throw new BadRequestException(
                "Publisher name is required."
            );
        }

        var existingPublisher =
            await _repository.GetByEmailAsync(dto.Email);

        if (existingPublisher != null &&
            existingPublisher.Id != id)
        {
            throw new ConflictException(
                "Email is already used by another publisher."
            );
        }

		var existingWebsite =
	        await _repository.GetByWebsiteAsync(dto.Website);

		if (existingWebsite != null &&
			existingWebsite.Id != id)
		{
			throw new ConflictException(
				"Website is already used by another publisher."
			);
		}

		publisher.Publishername = dto.Publishername;
		publisher.Address = dto.Address;
		publisher.City = dto.City;
		publisher.Website = dto.Website;
		publisher.Email = dto.Email;
		publisher.Phone = dto.Phone;

		publisher.Createdat = publisher.Createdat.HasValue
			? DateTime.SpecifyKind(
				publisher.Createdat.Value,
				DateTimeKind.Unspecified)
			: null;

		publisher.Updatedat = DateTime.SpecifyKind(
			DateTime.UtcNow,
			DateTimeKind.Unspecified);

		_repository.Update(publisher);

		await _repository.SaveAsync();
	}

    public async Task DeleteAsync(Guid id)
    {
        var publisher =
            await _repository.GetByIdAsync(id);

        if (publisher == null)
        {
            throw new NotFoundException(
                "Publisher not found."
            );
        }

		if (publisher.Authors.Any() || publisher.Books.Any())
		{
			throw new ConflictException(
				"This publisher cannot be deleted because it is referenced by existing authors or books. This historical relationship must be preserved."

			);
		}

		_repository.Delete(publisher);

        await _repository.SaveAsync();
    }
}