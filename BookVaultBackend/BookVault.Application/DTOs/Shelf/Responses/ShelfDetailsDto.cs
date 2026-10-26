using BookVault.Application.DTOs.BookCase.Responses;

namespace BookVault.Application.DTOs.Shelf.Responses;

public class ShelfDetailsDto
{
    public int Id { get; set; }

    public int Shelvecode { get; set; }

    public int Capacity { get; set; }

    public int? CaseNumber { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public BookCaseDetailsDto? Case{ get; set; }
}