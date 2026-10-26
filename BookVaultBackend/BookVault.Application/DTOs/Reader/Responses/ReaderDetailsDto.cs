using BookVault.Application.DTOs.Borrowings.Responses;

namespace BookVault.Application.DTOs.Reader.Responses;

public class ReaderDetailsDto
{
    public int Id { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public DateOnly Dateofbirth { get; set; }

    public DateOnly Expirationdate { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public string Createdby { get; set; } = null!;

    public string Updatedby { get; set; } = null!;

    public ICollection<BorrowingDto> Borrowings { get; set; }
        = new List<BorrowingDto>();
}