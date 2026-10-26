using BookVault.Domain.Entities;

namespace BookVault.Application.Interfaces.Repositories;

public interface IBorrowingRepository
{
    Task<IEnumerable<Borrowing>> GetAllAsync();

    Task<Borrowing?> GetByIdAsync(int id);

    Task<IEnumerable<Borrowing>> GetByReaderIdAsync(int readerId);

    Task<IEnumerable<Borrowing>> GetByBookIdAsync(Guid bookId);

    Task<bool> ExistsActiveBorrowingAsync(int readerId, Guid bookId);

	Task<int> GetActiveBorrowingCountByBookIdAsync(Guid bookId);

	Task AddAsync(Borrowing borrowing);

    void Update(Borrowing borrowing);

    Task SaveAsync();
}