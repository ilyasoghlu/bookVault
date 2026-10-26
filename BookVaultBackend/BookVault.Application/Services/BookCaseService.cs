using BookVault.Application.DTOs.BookCase.Requests;
using BookVault.Application.DTOs.BookCase.Responses;
using BookVault.Application.Exceptions;
using BookVault.Application.Interfaces.Repositories;
using BookVault.Application.Interfaces.Services;
using BookVault.Domain.Entities;

namespace BookVault.Application.Services;

public class BookCaseService : IBookCaseService
{
    private readonly IBookCaseRepository _bookCaseRepository;

    public BookCaseService(IBookCaseRepository bookCaseRepository)
    {
        _bookCaseRepository = bookCaseRepository;
    }

    public async Task<IEnumerable<BookCaseDto>> GetAllAsync()
    {
        var bookCases = await _bookCaseRepository.GetAllAsync();

        return bookCases.Select(MapToDto).ToList();
    }

    public async Task<BookCaseDetailsDto?> GetByIdAsync(int id)
    {
        var bookCase = await _bookCaseRepository.GetByIdAsync(id);

        if (bookCase == null)
            return null;

        return new BookCaseDetailsDto
        {
            Id = bookCase.Id,
            Casenumber = bookCase.Casenumber,
            Capacity = bookCase.Capacity,
            Sectionid = bookCase.Sectionid,
            SectionTitle = bookCase.Section?.Title,
            Createdat = bookCase.Createdat,
            Updatedat = bookCase.Updatedat
        };
    }

    public async Task<BookCaseDto> CreateAsync(
        CreateBookCaseDto dto)
    {
        if (dto.Casenumber <= 0)
        {
            throw new BadRequestException(
                "Case number must be greater than zero."
            );
        }

        if (dto.Capacity < 0)
        {
            throw new BadRequestException(
                "Capacity cannot be negative."
            );
        }

        var existingBookCase =
            await _bookCaseRepository.GetByCaseNumberAsync(
                dto.Casenumber);

        if (existingBookCase != null)
        {
            throw new ConflictException(
                "BookCase with this number already exists."
            );
        }

        var now = DateTime.UtcNow;

        var bookCase = new BookCase
        {
            Casenumber = dto.Casenumber,
            Capacity = dto.Capacity,
            Sectionid = dto.Sectionid,
            Createdat = now,
            Updatedat = now
        };

        await _bookCaseRepository.AddAsync(bookCase);
        await _bookCaseRepository.SaveAsync();

        return MapToDto(bookCase);
    }

    public async Task UpdateAsync(
        int id,
        UpdateBookCaseDto dto)
    {
        var bookCase =
            await _bookCaseRepository.GetByIdAsync(id);

        if (bookCase == null)
        {
            throw new NotFoundException(
                "BookCase not found."
            );
        }

        if (dto.Casenumber <= 0)
        {
            throw new BadRequestException(
                "Case number must be greater than zero."
            );
        }

        if (dto.Capacity < 0)
        {
            throw new BadRequestException(
                "Capacity cannot be negative."
            );
        }

        var existingBookCase =
            await _bookCaseRepository.GetByCaseNumberAsync(
                dto.Casenumber);

        if (existingBookCase != null &&
            existingBookCase.Id != id)
        {
            throw new ConflictException(
                "BookCase with this number already exists."
            );
        }

        bookCase.Casenumber = dto.Casenumber;
        bookCase.Capacity = dto.Capacity;
        bookCase.Sectionid = dto.Sectionid;
     

        
        bookCase.Createdat = bookCase.Createdat.HasValue
            ? DateTime.SpecifyKind(
                bookCase.Createdat.Value,
                DateTimeKind.Utc)
            : null;

        
        bookCase.Updatedat = DateTime.UtcNow;

        _bookCaseRepository.Update(bookCase);

        await _bookCaseRepository.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var bookCase =
            await _bookCaseRepository.GetByIdAsync(id);

        if (bookCase == null)
        {
            throw new NotFoundException(
                "BookCase not found."
            );
        }

        _bookCaseRepository.Delete(bookCase);

        await _bookCaseRepository.SaveAsync();
    }

    private static BookCaseDto MapToDto(BookCase bookCase)
    {
        return new BookCaseDto
        {
            Id = bookCase.Id,
            Casenumber = bookCase.Casenumber,
            Capacity = bookCase.Capacity,
            Sectionid = bookCase.Sectionid

        };
    }
}