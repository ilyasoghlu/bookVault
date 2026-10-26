using BookVault.Application.DTOs.Authors.Responses;
using BookVault.Application.DTOs.Books.Requests;
using BookVault.Application.DTOs.Books.Responses;
using BookVault.Application.DTOs.Publisher.Responses;
using BookVault.Application.DTOs.Shelf.Responses;
using BookVault.Application.Exceptions;
using BookVault.Application.Interfaces.Repositories;
using BookVault.Application.Interfaces.Services;
using BookVault.Domain.Entities;

namespace BookVault.Application.Services;

public class BookService : IBookService
{
	private readonly IBookRepository _bookRepository;
	private readonly IAuthorRepository _authorRepository;
	private readonly IPublisherRepository _publisherRepository;
	private readonly IShelfRepository _shelfRepository;

	public BookService(
		IBookRepository bookRepository,
		IAuthorRepository authorRepository,
		IPublisherRepository publisherRepository,
		IShelfRepository shelfRepository)
	{
		_bookRepository = bookRepository;
		_authorRepository = authorRepository;
		_publisherRepository = publisherRepository;
		_shelfRepository = shelfRepository;
	}

	public async Task<IEnumerable<BookDto>> GetAllAsync()
	{
		var books = await _bookRepository.GetAllAsync();

		return books.Select(MapToDto).ToList();
	}

	public async Task<BookDetailsDto> GetByIdAsync(Guid id)
	{
		var book = await _bookRepository.GetByIdAsync(id);

		if (book is null)
			throw new NotFoundException("Book not found.");

		return MapToDetailsDto(book);
	}

	public async Task<Guid> CreateAsync(CreateBookDto dto)
	{
		var existingBook =
			await _bookRepository.GetByIsbnAsync(dto.Isbn);

		if (existingBook is not null)
		{
			throw new ConflictException(
				"Book with this ISBN already exists.");
		}

		await ValidateRelationshipsAsync(
			dto.Authorid,
			dto.Publisherid,
			dto.Shelfid);

		var now = DateTime.SpecifyKind(
			DateTime.UtcNow,
			DateTimeKind.Unspecified);

		var book = new Book
		{
			Id = Guid.NewGuid(),
			Isbn = dto.Isbn,
			Title = dto.Title,
			Category = dto.Category,
			Genre = dto.Genre,
			Publicationdate = dto.Publicationdate,
			Pagecount = dto.Pagecount,
			Edition = dto.Edition,
			Totalcopies = dto.Totalcopies,
			Language = dto.Language,

			Authorid = dto.Authorid,
			Publisherid = dto.Publisherid,
			Shelfid = dto.Shelfid,

			Createdat = now,
			Updatedat = now
		};

		await _bookRepository.AddAsync(book);
		await _bookRepository.SaveAsync();

		return book.Id;
	}

	public async Task UpdateAsync(
		Guid id,
		UpdateBookDto dto)
	{
		var book =
			await _bookRepository.GetByIdAsync(id);

		if (book is null)
			throw new NotFoundException("Book not found.");

		var existingBook =
			await _bookRepository.GetByIsbnAsync(dto.Isbn);

		if (existingBook is not null &&
			existingBook.Id != id)
		{
			throw new ConflictException(
				"Book with this ISBN already exists.");
		}

		await ValidateRelationshipsAsync(
			dto.Authorid,
			dto.Publisherid,
			dto.Shelfid);

		book.Isbn = dto.Isbn;
		book.Title = dto.Title;
		book.Category = dto.Category;
		book.Genre = dto.Genre;
		book.Publicationdate = dto.Publicationdate;
		book.Pagecount = dto.Pagecount;
		book.Edition = dto.Edition;
		book.Totalcopies = dto.Totalcopies;
		book.Language = dto.Language;

		book.Authorid = dto.Authorid;
		book.Publisherid = dto.Publisherid;
		book.Shelfid = dto.Shelfid;

		book.Createdat = book.Createdat.HasValue
			? DateTime.SpecifyKind(
				book.Createdat.Value,
				DateTimeKind.Unspecified)
			: null;

		book.Updatedat = DateTime.SpecifyKind(
			DateTime.UtcNow,
			DateTimeKind.Unspecified);

		_bookRepository.Update(book);

		await _bookRepository.SaveAsync();
	}

	public async Task DeleteAsync(Guid id)
	{
		var book =
			await _bookRepository.GetByIdAsync(id);

		if (book is null)
			throw new NotFoundException("Book not found.");

		_bookRepository.Delete(book);

		await _bookRepository.SaveAsync();
	}

	private async Task ValidateRelationshipsAsync(
		Guid? authorId,
		Guid? publisherId,
		int? shelfId)
	{
		if (!authorId.HasValue)
		{
			throw new BadRequestException(
				"Author is required.");
		}

		if (!publisherId.HasValue)
		{
			throw new BadRequestException(
				"Publisher is required.");
		}

		if (!shelfId.HasValue)
		{
			throw new BadRequestException(
				"Shelf is required.");
		}

		var author =
			await _authorRepository.GetByIdAsync(
				authorId.Value);

		if (author is null)
		{
			throw new NotFoundException(
				"Author not found.");
		}

		var publisher =
			await _publisherRepository.GetByIdAsync(
				publisherId.Value);

		if (publisher is null)
		{
			throw new NotFoundException(
				"Publisher not found.");
		}

		var shelf =
			await _shelfRepository.GetByIdAsync(
				shelfId.Value);

		if (shelf is null)
		{
			throw new NotFoundException(
				"Shelf not found.");
		}
	}

	private static BookDto MapToDto(Book book)
	{
		var activeBorrowings = book.Borrowings
			.Where(borrowing => borrowing.Returndate == null)
			.ToList();

		return new BookDto
		{
			Id = book.Id,
			Isbn = book.Isbn,
			Title = book.Title,
			Category = book.Category,
			Genre = book.Genre,
			Publicationdate = book.Publicationdate,
			Pagecount = book.Pagecount,
			Edition = book.Edition,
			Totalcopies = book.Totalcopies,
			Language = book.Language,

			Createdat = book.Createdat,
			Updatedat = book.Updatedat,

			Authorid = book.Authorid,
			Publisherid = book.Publisherid,
			Shelfid = book.Shelfid,

			Author = book.Author is null
				? null
				: new AuthorDetailsDto
				{
					Id = book.Author.Id,
					Firstname = book.Author.Firstname,
					Lastname = book.Author.Lastname,
					Birthdate = book.Author.Birthdate,
					Deathdate = book.Author.Deathdate,
					Nationality = book.Author.Nationality,
					Biography = book.Author.Biography,
					Createdat = book.Author.Createdat,
					Updatedat = book.Author.Updatedat,
					Publisherid = book.Author.Publisherid
				},

			Publisher = book.Publisher is null
				? null
				: new PublisherDetailsDto
				{
					Id = book.Publisher.Id,
					Publishername = book.Publisher.Publishername,
					Address = book.Publisher.Address,
					City = book.Publisher.City,
					Website = book.Publisher.Website,
					Email = book.Publisher.Email,
					Phone = book.Publisher.Phone
				},

			Shelf = book.Shelf is null
				? null
				: new ShelfDetailsDto
				{
					Id = book.Shelf.Id,
					Shelvecode = book.Shelf.Shelvecode,
					Capacity = book.Shelf.Capacity,
					CaseNumber = book.Shelf.Case?.Casenumber,
					Createdat = book.Shelf.Createdat,
					Updatedat = book.Shelf.Updatedat
				},

			Borrowing = new BookBorrowingDetailsDto
			{
				TotalCopies = book.Totalcopies,
				BorrowedCopies = activeBorrowings.Count,
				AvailableCopies =
					Math.Max(
						book.Totalcopies - activeBorrowings.Count,
						0),

				ActiveBorrowings = activeBorrowings
					.Select(borrowing => new ActiveBorrowingDto
					{
						BorrowedBy = borrowing.Reader is null
							? "Not specified"
							: $"{borrowing.Reader.Firstname} {borrowing.Reader.Lastname}",

						DueDate = borrowing.Duedate
					})
					.ToList()
			}
		};
	}

	private static BookDetailsDto MapToDetailsDto(Book book)
	{
		var activeBorrowings = book.Borrowings
			.Where(borrowing => borrowing.Returndate == null)
			.ToList();

		return new BookDetailsDto
		{
			Id = book.Id,
			Isbn = book.Isbn,
			Title = book.Title,
			Category = book.Category,
			Genre = book.Genre,
			Publicationdate = book.Publicationdate,
			Pagecount = book.Pagecount,
			Edition = book.Edition,
			Totalcopies = book.Totalcopies,
			Language = book.Language,

			Createdat = book.Createdat,
			Updatedat = book.Updatedat,

			Authorid = book.Authorid,
			Publisherid = book.Publisherid,

			Author = book.Author is null
				? null
				: new AuthorDetailsDto
				{
					Id = book.Author.Id,
					Firstname = book.Author.Firstname,
					Lastname = book.Author.Lastname,
					Birthdate = book.Author.Birthdate,
					Deathdate = book.Author.Deathdate,
					Nationality = book.Author.Nationality,
					Biography = book.Author.Biography,
					Createdat = book.Author.Createdat,
					Updatedat = book.Author.Updatedat,
					Publisherid = book.Author.Publisherid
				},

			Publisher = book.Publisher is null
				? null
				: new PublisherDetailsDto
				{
					Id = book.Publisher.Id,
					Publishername = book.Publisher.Publishername,
					Address = book.Publisher.Address,
					City = book.Publisher.City,
					Website = book.Publisher.Website,
					Email = book.Publisher.Email,
					Phone = book.Publisher.Phone
				},

			Location = book.Shelf is null
				? null
				: new BookLocationDetailsDto
				{
					SectionTitle =
						book.Shelf.Case?.Section?.Title,

					RoomNumber =
						book.Shelf.Case?.Section?.Room?.Roomnumber,

					RoomFloor =
						book.Shelf.Case?.Section?.Room?.Roomfloor,

					CaseNumber =
						book.Shelf.Case?.Casenumber,

					ShelfCode =
						book.Shelf.Shelvecode,

					EmployeeName =
						book.Shelf.Case?.Section?.Room?.Employee is null
							? null
							: $"{book.Shelf.Case.Section.Room.Employee.Firstname} {book.Shelf.Case.Section.Room.Employee.Lastname}",

					EmployeePosition =
						book.Shelf.Case?.Section?.Room?.Employee?.Employeeposition
				},

			Borrowing = new BookBorrowingDetailsDto
			{
				TotalCopies = book.Totalcopies,
				BorrowedCopies = activeBorrowings.Count,
				AvailableCopies =
					Math.Max(
						book.Totalcopies - activeBorrowings.Count,
						0),

				ActiveBorrowings = activeBorrowings
					.Select(borrowing => new ActiveBorrowingDto
					{
						BorrowedBy = borrowing.Reader is null
							? "Not specified"
							: $"{borrowing.Reader.Firstname} {borrowing.Reader.Lastname}",

						DueDate = borrowing.Duedate
					})
					.ToList()
			}
		};
	}
}