using BookVault.Application.Interfaces.Repositories;
using BookVault.Domain.Entities;
using BookVault.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookVault.Infrastructure.Repositories;

public class SectionRepository : ISectionRepository
{
    private readonly BookVaultContext _context;

    public SectionRepository(BookVaultContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Section>> GetAllAsync()
    {
        return await _context.Sections
            .Include(x => x.Employee)
            .Include(x => x.BookCases)
            .ToListAsync();
    }

    public async Task<Section?> GetByIdAsync(int id)
    {
        return await _context.Sections
            .Include(x => x.Employee)
            .Include(x => x.BookCases)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Section?> GetByTitleAsync(string title)
    {
        return await _context.Sections
            .FirstOrDefaultAsync(x => x.Title == title);
    }

	public async Task<Section?> GetBySectionNumberAsync(int sectionnumber)
	{
		return await _context.Sections
			.FirstOrDefaultAsync(x => x.Sectionnumber == sectionnumber);
	}

	public async Task AddAsync(Section section)
    {
        await _context.Sections.AddAsync(section);
    }

    public void Update(Section section)
    {
        _context.Sections.Update(section);
    }

    public void Delete(Section section)
    {
        _context.Sections.Remove(section);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}