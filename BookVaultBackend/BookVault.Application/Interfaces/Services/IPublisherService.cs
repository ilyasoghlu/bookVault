using BookVault.Application.DTOs.Publisher.Responses;
using BookVault.Application.DTOs.Publishers.Requests;
using BookVault.Application.DTOs.Publishers.Responses;

namespace BookVault.Application.Interfaces.Services;

public interface IPublisherService
{
    Task<IEnumerable<PublisherDto>> GetAllAsync();

    Task<PublisherDetailsDto> GetByIdAsync(Guid id);

    Task<Guid> CreateAsync(CreatePublisherDto dto);

    Task UpdateAsync(Guid id, UpdatePublisherDto dto);

    Task DeleteAsync(Guid id);
}