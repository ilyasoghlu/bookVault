using BookVault.Application.DTOs.Authors.Responses;
using BookVault.Application.DTOs.Publisher.Responses;
using BookVault.Application.DTOs.Shelf.Responses;

namespace BookVault.Application.DTOs.Books.Responses;

public class BookDto
{
    public Guid Id { get; set; }

    public string Isbn { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string Genre { get; set; } = null!;

    public DateOnly Publicationdate { get; set; }

    public int Pagecount { get; set; }

    public string Edition { get; set; } = null!;

    public int Totalcopies { get; set; }

    public string Language { get; set; } = null!;

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public Guid? Authorid { get; set; }

    public Guid? Publisherid { get; set; }

    public int? Shelfid { get; set; }

    public AuthorDetailsDto? Author { get; set; }

    public PublisherDetailsDto? Publisher { get; set; }

    public ShelfDetailsDto? Shelf { get; set; }

    public BookBorrowingDetailsDto? Borrowing { get; set; }
}