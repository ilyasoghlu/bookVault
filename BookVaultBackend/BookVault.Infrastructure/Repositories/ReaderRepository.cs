using BookVault.Application.Interfaces.Repositories;
using BookVault.Domain.Entities;
using BookVault.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookVault.Infrastructure.Repositories;

public class ReaderRepository : IReaderRepository
{
    private readonly BookVaultContext _context;

    public ReaderRepository(BookVaultContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Reader>> GetAllAsync()
    {
        return await _context.Readers
            .Include(x => x.Borrowings)
            .ToListAsync();
    }

    public async Task<Reader?> GetByIdAsync(int id)
    {
        return await _context.Readers
            .Include(x => x.Borrowings)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Reader?> GetByReaderEmailAsync(string readerEmail)
    {
        return await _context.Readers
            .FirstOrDefaultAsync(x => x.Email == readerEmail);
    }

    public async Task AddAsync(Reader reader)
    {
        await _context.Readers.AddAsync(reader);
    }

    public void Update(Reader reader)
    {
        _context.Readers.Update(reader);
    }

    public void Delete(Reader reader)
    {
        _context.Readers.Remove(reader);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}