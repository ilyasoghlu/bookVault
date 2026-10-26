using BookVault.Application.DTOs.Section.Requests;
using BookVault.Application.DTOs.Section.Responses;

namespace BookVault.Application.Interfaces.Services;

public interface ISectionService
{
    Task<IEnumerable<SectionDto>> GetAllAsync();

    Task<SectionDetailsDto> GetByIdAsync(int id);

    Task<int> CreateAsync(CreateSectionDto dto);

    Task UpdateAsync(int id, UpdateSectionDto dto);

    Task DeleteAsync(int id);
}