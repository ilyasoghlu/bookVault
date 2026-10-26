using BookVault.Domain.Entities;

namespace BookVault.Application.Interfaces.Repositories;

public interface ISectionRepository
{
    Task<IEnumerable<Section>> GetAllAsync();

    Task<Section?> GetByIdAsync(int id);

    Task<Section?> GetByTitleAsync(string title);

    Task<Section?> GetBySectionNumberAsync(int sectionNumber);

    Task AddAsync(Section section);

    void Update(Section section);

    void Delete(Section section);

    Task SaveAsync();
}