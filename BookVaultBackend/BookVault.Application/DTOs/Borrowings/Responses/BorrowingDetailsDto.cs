using BookVault.Application.DTOs.Books.Responses;
using BookVault.Application.DTOs.Reader.Responses;

namespace BookVault.Application.DTOs.Borrowings.Responses;

public class BorrowingDetailsDto
{
   public int Id { get; set; }

    public DateOnly Duedate { get; set; }

    public DateOnly? Returndate { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public ReaderDetailsDto? ReaderDetails { get; set; }
    public int Readerid { get; set; }

    public BookDetailsDto? BookDetails { get; set; }

    public Guid Bookid { get; set; }

}