using BookVault.Application.DTOs.Shelf.Requests;
using BookVault.Application.DTOs.Shelf.Responses;
using BookVault.Application.Exceptions;
using BookVault.Application.Interfaces.Repositories;
using BookVault.Application.Interfaces.Services;
using BookVault.Domain.Entities;

namespace BookVault.Application.Services;

public class ShelfService : IShelfService
{
    private readonly IShelfRepository _shelfRepository;

    public ShelfService(IShelfRepository shelfRepository)
    {
        _shelfRepository = shelfRepository;
    }

    public async Task<IEnumerable<ShelfDto>> GetAllAsync()
    {
        var shelves = await _shelfRepository.GetAllAsync();

        return shelves.Select(x => new ShelfDto
        {
            Id = x.Id,
            Shelvecode = x.Shelvecode,
            Capacity = x.Capacity,
       
          
        }).ToList();
    }

    public async Task<ShelfDetailsDto?> GetByIdAsync(int id)
    {
        var shelf = await _shelfRepository.GetByIdAsync(id);

        if (shelf == null)
            return null;

        return new ShelfDetailsDto
        {
            Id = shelf.Id,
            Shelvecode = shelf.Shelvecode,
            Capacity = shelf.Capacity,

            CaseNumber = shelf.Case?.Casenumber,

            Createdat = shelf.Createdat,
            Updatedat = shelf.Updatedat
        };
    }

    public async Task<ShelfDto> CreateAsync(CreateShelfDto dto)
    {
        var existingShelf =
            await _shelfRepository.GetByCodeAsync(dto.Shelvecode);

        if (existingShelf != null)
        {
            throw new ConflictException(
                "Shelf with this code already exists."
            );
        }

        var now = DateTime.UtcNow;

        var shelf = new Shelf
        {
            Shelvecode = dto.Shelvecode,
            Capacity = dto.Capacity,
			Caseid = dto.Caseid,

			Createdat = now,
            Updatedat = now
        };

        await _shelfRepository.AddAsync(shelf);
        await _shelfRepository.SaveAsync();




        return new ShelfDto
        {
            Id = shelf.Id,
            Shelvecode = shelf.Shelvecode,
            Capacity = shelf.Capacity,
    
        };
    }

    public async Task UpdateAsync(
        int id,
        UpdateShelfDto dto)
    {
        var shelf =
            await _shelfRepository.GetByIdAsync(id);

        if (shelf == null)
        {
            throw new NotFoundException(
                "Shelf not found."
            );
        }

        var existingShelf =
            await _shelfRepository.GetByCodeAsync(
                dto.Shelvecode);

        if (existingShelf != null &&
            existingShelf.Id != id)
        {
            throw new ConflictException(
                "Shelf with this code already exists."
            );
        }

        shelf.Shelvecode = dto.Shelvecode;
        shelf.Capacity = dto.Capacity;
	

		shelf.Createdat = shelf.Createdat.HasValue
            ? DateTime.SpecifyKind(
                shelf.Createdat.Value,
                DateTimeKind.Utc)
            : null;

        
        shelf.Updatedat = DateTime.UtcNow;

        _shelfRepository.Update(shelf);

        await _shelfRepository.SaveAsync();
    }

	public async Task DeleteAsync(int id)
	{
		var shelf =
			await _shelfRepository.GetByIdAsync(id);

		if (shelf == null)
		{
			throw new NotFoundException(
				"Shelf not found."
			);
		}

		if (shelf.Books.Any())
		{
			throw new ConflictException(
				"This shelf contains books. Please move the books to another shelf before deleting it."
			);
		}

		_shelfRepository.Delete(shelf);

		await _shelfRepository.SaveAsync();
	}
}