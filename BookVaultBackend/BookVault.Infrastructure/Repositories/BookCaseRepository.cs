using BookVault.Application.Interfaces.Repositories;
using BookVault.Domain.Entities;
using BookVault.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookVault.Infrastructure.Repositories;

public class BookCaseRepository : IBookCaseRepository
{
    private readonly BookVaultContext _context;

    public BookCaseRepository(BookVaultContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BookCase>> GetAllAsync()
    {
        return await _context.BookCases
            .Include(x => x.Section)
            .ToListAsync();
    }

    public async Task<BookCase?> GetByIdAsync(int id)
    {
        return await _context.BookCases
            .Include(x => x.Section)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<BookCase?> GetByCaseNumberAsync(int caseNumber)
    {
        return await _context.BookCases
            .FirstOrDefaultAsync(x => x.Casenumber == caseNumber);
    }

    public async Task AddAsync(BookCase bookCase)
    {
        await _context.BookCases.AddAsync(bookCase);
    }

    public void Update(BookCase bookCase)
    {
        _context.BookCases.Update(bookCase);
    }

    public void Delete(BookCase bookCase)
    {
        _context.BookCases.Remove(bookCase);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}