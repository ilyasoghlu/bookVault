using BookVault.Domain.Entities;

namespace BookVault.Application.Interfaces.Repositories;

public interface IBookCaseRepository
{
    Task<IEnumerable<BookCase>> GetAllAsync();

    Task<BookCase?> GetByIdAsync(int id);

    Task<BookCase?> GetByCaseNumberAsync(int caseNumber);

    Task AddAsync(BookCase bookCase);

    void Update(BookCase bookCase);

    void Delete(BookCase bookCase);

    Task SaveAsync();
}