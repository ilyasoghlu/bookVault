using BookVault.Domain.Entities;

namespace BookVault.Application.Interfaces.Repositories;

public interface IAuthorRepository
{
    Task<IEnumerable<Author>> GetAllAsync();

    Task<Author?> GetByIdAsync(Guid id);

    Task<Author?> GetByNameAsync(
        string firstname,
        string lastname);

    Task<IEnumerable<Author>> GetByPublisherIdAsync(Guid publisherId);

    Task AddAsync(Author author);

    void Update(Author author);

    void Delete(Author author);

    Task SaveAsync();
}