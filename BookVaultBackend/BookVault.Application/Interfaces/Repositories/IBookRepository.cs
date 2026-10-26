using BookVault.Domain.Entities;

namespace BookVault.Application.Interfaces.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();

    Task<Book?> GetByIdAsync(Guid id);

    Task<Book?> GetByIsbnAsync(string isbn);

    Task<IEnumerable<Book>> GetByAuthorIdAsync(Guid authorId);

    Task<IEnumerable<Book>> GetByPublisherIdAsync(Guid publisherId);

    Task AddAsync(Book book);

    void Update(Book book);

    void Delete(Book book);

    Task SaveAsync();
}