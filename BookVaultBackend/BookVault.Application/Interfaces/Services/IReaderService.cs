using BookVault.Application.DTOs.Reader.Requests;
using BookVault.Application.DTOs.Reader.Responses;

namespace BookVault.Application.Interfaces.Services;

public interface IReaderService
{
    Task<IEnumerable<ReaderDto>> GetAllAsync();

    Task<ReaderDetailsDto?> GetByIdAsync(int id);

    Task<ReaderDto> CreateAsync(CreateReaderDto dto);

    Task UpdateAsync(int id, UpdateReaderDto dto);

    Task DeleteAsync(int id);
}