using BookVault.Application.Interfaces.Repositories;
using BookVault.Domain.Entities;
using BookVault.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookVault.Infrastructure.Repositories;

public class AuthorRepository : IAuthorRepository
{
    private readonly BookVaultContext _context;

    public AuthorRepository(BookVaultContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Author>> GetAllAsync()
    {
        return await _context.Authors
            .AsNoTracking()
            .Include(x => x.Publisher)
            .Include(x => x.Books)
            .ToListAsync();
    }

    public async Task<Author?> GetByIdAsync(Guid id)
    {
        return await _context.Authors
            .Include(x => x.Publisher)
            .Include(x => x.Books)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Author?> GetByNameAsync(
        string firstname,
        string lastname)
    {
        return await _context.Authors
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.Firstname == firstname &&
                x.Lastname == lastname);
    }

    public async Task<IEnumerable<Author>> GetByPublisherIdAsync(
        Guid publisherId)
    {
        return await _context.Authors
            .AsNoTracking()
            .Include(x => x.Books)
            .Where(x => x.Publisherid == publisherId)
            .ToListAsync();
    }

    public async Task AddAsync(Author author)
    {
        await _context.Authors.AddAsync(author);
    }

    public void Update(Author author)
    {
        _context.Authors.Update(author);
    }

    public void Delete(Author author)
    {
        _context.Authors.Remove(author);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}