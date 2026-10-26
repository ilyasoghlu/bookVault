using BookVault.Domain.Entities;

namespace BookVault.Application.Interfaces.Repositories;

public interface IShelfRepository
{
    Task<IEnumerable<Shelf>> GetAllAsync();

    Task<Shelf?> GetByIdAsync(int id);

    Task<Shelf?> GetByCodeAsync(int code);

    Task AddAsync(Shelf shelf);

    void Update(Shelf shelf);

    void Delete(Shelf shelf);

    Task SaveAsync();
}