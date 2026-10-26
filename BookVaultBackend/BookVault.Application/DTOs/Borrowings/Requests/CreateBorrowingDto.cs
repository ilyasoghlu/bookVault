using System.ComponentModel.DataAnnotations;

namespace BookVault.Application.DTOs.Borrowings.Requests
{
	public class CreateBorrowingDto
	{
		public DateOnly Duedate { get; set; }

		[Range(1, int.MaxValue)]
		public int Readerid { get; set; }

		[Required]
		public Guid Bookid { get; set; }
	}
}