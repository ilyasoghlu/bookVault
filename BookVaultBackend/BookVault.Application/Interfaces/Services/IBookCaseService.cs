using BookVault.Application.DTOs.BookCase.Requests;
using BookVault.Application.DTOs.BookCase.Responses;

namespace BookVault.Application.Interfaces.Services;

public interface IBookCaseService
{
    Task<IEnumerable<BookCaseDto>> GetAllAsync();

    Task<BookCaseDetailsDto?> GetByIdAsync(int id);

    Task<BookCaseDto> CreateAsync(CreateBookCaseDto dto);

    Task UpdateAsync(int id, UpdateBookCaseDto dto);

    Task DeleteAsync(int id);
}