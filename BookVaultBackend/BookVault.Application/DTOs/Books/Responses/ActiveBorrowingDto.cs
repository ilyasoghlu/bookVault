

namespace BookVault.Application.DTOs.Books.Responses
{
    public class ActiveBorrowingDto
    {
		public string? BorrowedBy { get; set; }
		public DateOnly? DueDate { get; set; }
	}
}
