using System.ComponentModel.DataAnnotations;
using BookVault.Application.DTOs.Borrowings.Requests;
using BookVault.Application.DTOs.Borrowings.Responses;
using BookVault.Application.DTOs.Books.Responses;
using BookVault.Application.DTOs.Reader.Responses;
using BookVault.Application.Exceptions;
using BookVault.Application.Interfaces.Repositories;
using BookVault.Application.Interfaces.Services;
using BookVault.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookVault.Application.Services;

public class BorrowingService : IBorrowingService
{
    private readonly IBorrowingRepository _borrowingRepository;
    private readonly IReaderRepository _readerRepository;
    private readonly IBookRepository _bookRepository;

    public BorrowingService(
        IBorrowingRepository borrowingRepository,
        IReaderRepository readerRepository,
        IBookRepository bookRepository)
    {
        _borrowingRepository = borrowingRepository;
        _readerRepository = readerRepository;
        _bookRepository = bookRepository;
    }

    public async Task<IEnumerable<BorrowingDetailsDto>> GetAllAsync()
    {
        var borrowings = await _borrowingRepository.GetAllAsync();

        return borrowings.Select(MapToDetailsDto);
    }

    public async Task<BorrowingDetailsDto?> GetByIdAsync(int id)
    {
        var borrowing = await _borrowingRepository.GetByIdAsync(id);

        if (borrowing == null)
            return null;

        return MapToDetailsDto(borrowing);
    }

    public async Task<BorrowingDetailsDto> CreateAsync(CreateBorrowingDto dto)
    {
        var reader =
            await _readerRepository.GetByIdAsync(dto.Readerid);

        if (reader == null)
            throw new NotFoundException("Reader not found.");

        var book =
            await _bookRepository.GetByIdAsync(dto.Bookid);

        if (book == null)
            throw new NotFoundException("Book not found.");

        var existingBorrowing =
            await _borrowingRepository.ExistsActiveBorrowingAsync(
                dto.Readerid,
                dto.Bookid);

        if (existingBorrowing)
        {
            throw new ConflictException(
                "This reader already has an active borrowing for this book.");
        }

		if (dto.Duedate < DateOnly.FromDateTime(DateTime.UtcNow))
		{
			throw new BadRequestException(
				"Due date cannot be earlier than today.");
		}
		
        var activeBorrowingCount =
	        await _borrowingRepository.GetActiveBorrowingCountByBookIdAsync(
		        dto.Bookid);

		if (activeBorrowingCount >= book.Totalcopies)
		{
			throw new ConflictException(
				"No available copies of this book.");
		}


       

		var now = DateTime.SpecifyKind(
			DateTime.UtcNow,
			DateTimeKind.Unspecified);

		var borrowing = new Borrowing
        {
            Readerid = dto.Readerid,
            Bookid = dto.Bookid,

            Duedate = dto.Duedate,
            Returndate = null,
            Createdat = now,
            Updatedat = now
        };

        await _borrowingRepository.AddAsync(borrowing);
        await _borrowingRepository.SaveAsync();

        return new BorrowingDetailsDto
        {
            Id = borrowing.Id,
            Duedate = borrowing.Duedate,
            Returndate = borrowing.Returndate,
            Createdat = borrowing.Createdat,
            Updatedat = borrowing.Updatedat,

            Readerid = borrowing.Readerid,
            Bookid = borrowing.Bookid,
           
        };
    }

    public async Task UpdateAsync(
    int id,
    UpdateBorrowingDto dto)
    {
        var borrowing =
            await _borrowingRepository.GetByIdAsync(id);

        if (borrowing == null)
        {
            throw new NotFoundException(
                "Borrowing not found.");
        }

        if (borrowing.Createdat.HasValue &&
            dto.Duedate < DateOnly.FromDateTime(borrowing.Createdat.Value))
        {
            throw new BadRequestException(
                "Due date cannot be earlier than the borrowing creation date.");
        }

        borrowing.Duedate = dto.Duedate;

        if (dto.Returndate.HasValue &&
            borrowing.Createdat.HasValue &&
            dto.Returndate.Value < DateOnly.FromDateTime(borrowing.Createdat.Value))
        {
            throw new BadRequestException(
                "Return date cannot be earlier than the borrowing creation date.");
        }

        if (borrowing.Returndate == null &&
            dto.Returndate.HasValue)
        {
            borrowing.Returndate = dto.Returndate.Value;
        }
        else if (borrowing.Returndate != null &&
                 dto.Returndate.HasValue)
        {
            throw new ConflictException(
                "This borrowing has already been returned.");
        }

        borrowing.Createdat = borrowing.Createdat.HasValue
            ? DateTime.SpecifyKind(
                borrowing.Createdat.Value,
                DateTimeKind.Unspecified)
            : null;

        borrowing.Updatedat = DateTime.SpecifyKind(
            DateTime.UtcNow,
            DateTimeKind.Unspecified);



        _borrowingRepository.Update(borrowing);

        await _borrowingRepository.SaveAsync();
    }


  

    private static BorrowingDetailsDto MapToDetailsDto(
        Borrowing borrowing)
    {
        return new BorrowingDetailsDto
        {
            Id = borrowing.Id,

            Duedate = borrowing.Duedate,
            Returndate = borrowing.Returndate,

            Createdat = borrowing.Createdat,
            Updatedat = borrowing.Updatedat,

            Readerid = borrowing.Readerid,

            ReaderDetails = borrowing.Reader == null
                ? null
                : new ReaderDetailsDto
                {
                    Id = borrowing.Reader.Id,
                    Firstname = borrowing.Reader.Firstname,
                    Lastname = borrowing.Reader.Lastname,
                    Email = borrowing.Reader.Email,
                    Phone = borrowing.Reader.Phone,
                    Dateofbirth = borrowing.Reader.Dateofbirth,
                    Expirationdate = borrowing.Reader.Expirationdate,
                    Createdat = borrowing.Reader.Createdat,
                    Updatedat = borrowing.Reader.Updatedat,
                    Createdby = borrowing.Reader.Createdby,
                    Updatedby = borrowing.Reader.Updatedby,
                },

            Bookid = borrowing.Bookid,

            BookDetails = borrowing.Book == null
             ? null
             : new BookDetailsDto
             { 
                Id = borrowing.Book.Id,
                Isbn = borrowing.Book.Isbn,
				 Title = borrowing.Book.Title,
				 Category = borrowing.Book.Category,
				 Genre = borrowing.Book.Genre,
				 Publicationdate = borrowing.Book.Publicationdate,
				 Pagecount = borrowing.Book.Pagecount,
				 Edition = borrowing.Book.Edition,
				 Totalcopies = borrowing.Book.Totalcopies,
				 Language = borrowing.Book.Language,
				 Createdat = borrowing.Book.Createdat,
				 Updatedat = borrowing.Book.Updatedat
			 }

        };
    }
}