using BookVault.Application.DTOs.Books.Requests;
using BookVault.Application.DTOs.Books.Responses;

namespace BookVault.Application.Interfaces.Services;

public interface IBookService
{
    Task<IEnumerable<BookDto>> GetAllAsync();

    Task<BookDetailsDto> GetByIdAsync(Guid id);

    Task<Guid> CreateAsync(CreateBookDto dto);

    Task UpdateAsync(Guid id, UpdateBookDto dto);

    Task DeleteAsync(Guid id);
}