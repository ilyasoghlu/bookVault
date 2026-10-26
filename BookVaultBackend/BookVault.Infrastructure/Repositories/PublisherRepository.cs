using BookVault.Application.Interfaces.Repositories;
using BookVault.Domain.Entities;
using BookVault.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookVault.Infrastructure.Repositories;

public class PublisherRepository : IPublisherRepository
{
    private readonly BookVaultContext _context;

    public PublisherRepository(BookVaultContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Publisher>> GetAllAsync()
    {
        return await _context.Publishers
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Publisher?> GetByIdAsync(Guid id)
    {
        return await _context.Publishers
            .Include(x => x.Authors)
            .Include(x => x.Books)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Publisher?> GetByEmailAsync(string email)
    {
        return await _context.Publishers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email);
    }

	public async Task<Publisher?> GetByWebsiteAsync(string website)
	{
		return await _context.Publishers
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Website == website);
	}

	public async Task AddAsync(Publisher publisher)
    {
        await _context.Publishers.AddAsync(publisher);
    }

    public void Update(Publisher publisher)
    {
        _context.Publishers.Update(publisher);
    }

    public void Delete(Publisher publisher)
    {
        _context.Publishers.Remove(publisher);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}