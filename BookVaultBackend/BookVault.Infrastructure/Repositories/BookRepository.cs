using BookVault.Application.Interfaces.Repositories;
using BookVault.Domain.Entities;
using BookVault.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookVault.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly BookVaultContext _context;

    public BookRepository(BookVaultContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _context.Books
            .AsNoTracking()
            .Include(book => book.Author)
            .Include(book => book.Publisher)
            .Include(book => book.Shelf)
				.ThenInclude(shelf => shelf.Case)
					.ThenInclude(bookCase => bookCase.Section)
						.ThenInclude(section => section.Room)
							.ThenInclude(room => room.Employee)


			.Include(book => book.Borrowings)
            .ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(Guid id)
    {
        return await _context.Books
            .Include(book => book.Author)
            .Include(book => book.Publisher)
           	.Include(book => book.Shelf)
				.ThenInclude(shelf => shelf.Case)
					.ThenInclude(bookCase => bookCase.Section)
						.ThenInclude(section => section.Room)
                            .ThenInclude(room => room.Employee)

			.Include(book => book.Borrowings)
               .ThenInclude(borrwing => borrwing.Reader)
            .FirstOrDefaultAsync(book => book.Id == id);
    }

    public async Task<Book?> GetByIsbnAsync(string isbn)
    {
        return await _context.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(book => book.Isbn == isbn);
    }

    public async Task<IEnumerable<Book>> GetByAuthorIdAsync(Guid authorId)
    {
        return await _context.Books
            .AsNoTracking()
            .Where(book => book.Authorid == authorId)
            .Include(book => book.Author)
            .Include(book => book.Publisher)
            .Include(book => book.Shelf)
				.ThenInclude(shelf => shelf.Case)
			.Include(book => book.Borrowings)
            .ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetByPublisherIdAsync(Guid publisherId)
    {
        return await _context.Books
            .AsNoTracking()
            .Where(book => book.Publisherid == publisherId)
            .Include(book => book.Author)
            .Include(book => book.Publisher)
            .Include(book => book.Shelf)
				.ThenInclude(shelf => shelf.Case)
			.Include(book => book.Borrowings)
            .ToListAsync();
    }


    public async Task AddAsync(Book book)
    {
        await _context.Books.AddAsync(book);
    }

    public void Update(Book book)
    {
        _context.Books.Update(book);
    }

    public void Delete(Book book)
    {
        _context.Books.Remove(book);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}