using BookVault.Domain.Entities;

namespace BookVault.Application.Interfaces.Repositories;

public interface IReaderRepository
{
    Task<IEnumerable<Reader>> GetAllAsync();

    Task<Reader?> GetByIdAsync(int id);

    Task<Reader?> GetByReaderEmailAsync(string readerEmail);

    Task AddAsync(Reader reader);

    void Update(Reader reader);

    void Delete(Reader reader);

    Task SaveAsync();
}