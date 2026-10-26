using BookVault.Application.Interfaces.Repositories;
using BookVault.Domain.Entities;
using BookVault.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookVault.Infrastructure.Repositories;

public class BorrowingRepository : IBorrowingRepository
{
    private readonly BookVaultContext _context;

    public BorrowingRepository(BookVaultContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsActiveBorrowingAsync(int readerId, Guid bookId)
    {
        return await _context.Borrowings
            .AnyAsync(x =>
                x.Readerid == readerId &&
                x.Bookid == bookId &&
                x.Returndate == null);
    }

    public async Task<IEnumerable<Borrowing>> GetAllAsync()
    {
        return await _context.Borrowings
            .Include(x => x.Reader)
            .Include(x => x.Book)
            .ToListAsync();
    }

    public async Task<IEnumerable<Borrowing>> GetByBookIdAsync(Guid bookId)
    {
        return await _context.Borrowings
            .Include(x => x.Reader)
            .Include(x => x.Book)
            .Where(x => x.Bookid == bookId)
            .ToListAsync();
    }

    public async Task<Borrowing?> GetByIdAsync(int id)
    {
        return await _context.Borrowings
            .Include(x => x.Reader)
            .Include(x => x.Book)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Borrowing>> GetByReaderIdAsync(int readerId)
    {
        return await _context.Borrowings
            .Include(x => x.Reader)
            .Include(x => x.Book)
            .Where(x => x.Readerid == readerId)
            .ToListAsync();
    }

	public async Task<int> GetActiveBorrowingCountByBookIdAsync(Guid bookId)
	{
		return await _context.Borrowings
			.CountAsync(x =>
				x.Bookid == bookId &&
				x.Returndate == null);
	}

	public async Task AddAsync(Borrowing borrowing)
    {
        await _context.Borrowings.AddAsync(borrowing);
    }

    public void Update(Borrowing borrowing)
    {
        _context.Borrowings.Update(borrowing);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}