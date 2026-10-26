using BookVault.Domain.Entities;

namespace BookVault.Application.Interfaces.Repositories;

public interface IPublisherRepository
{
    Task<IEnumerable<Publisher>> GetAllAsync();

    Task<Publisher?> GetByIdAsync(Guid id);

    Task<Publisher?> GetByEmailAsync(string email);

	Task<Publisher?> GetByWebsiteAsync(string website);

	Task AddAsync(Publisher publisher);

    void Update(Publisher publisher);

    void Delete(Publisher publisher);

    Task SaveAsync();
}