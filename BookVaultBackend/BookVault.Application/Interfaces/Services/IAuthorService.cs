using BookVault.Application.DTOs.Authors.Requests;
using BookVault.Application.DTOs.Authors.Responses;

namespace BookVault.Application.Interfaces.Services;

public interface IAuthorService
{
    Task<IEnumerable<AuthorDto>> GetAllAsync();

    Task<AuthorDetailsDto> GetByIdAsync(Guid id);

    Task<Guid> CreateAsync(CreateAuthorDto dto);

    Task UpdateAsync(Guid id, UpdateAuthorDto dto);

    Task DeleteAsync(Guid id);
}