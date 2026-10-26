using BookVault.Application.Interfaces.Repositories;
using BookVault.Domain.Entities;
using BookVault.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookVault.Infrastructure.Repositories;

public class ShelfRepository : IShelfRepository
{
    private readonly BookVaultContext _context;

    public ShelfRepository(BookVaultContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Shelf>> GetAllAsync()
    {
        return await _context.Shelves
            .Include(x => x.Case)
            .ToListAsync();
    }

    public async Task<Shelf?> GetByIdAsync(int id)
    {
        return await _context.Shelves
            .Include(x => x.Case)
			.Include(x => x.Books)
			.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Shelf?> GetByCodeAsync(int code)
    {
        return await _context.Shelves
            .FirstOrDefaultAsync(x => x.Shelvecode == code);
    }

    public async Task AddAsync(Shelf shelf)
    {
        await _context.Shelves.AddAsync(shelf);
    }

    public void Update(Shelf shelf)
    {
        _context.Shelves.Update(shelf);
    }

    public void Delete(Shelf shelf)
    {
        _context.Shelves.Remove(shelf);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}