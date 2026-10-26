namespace BookVault.Application.DTOs.Borrowings.Requests
{
    public class UpdateBorrowingDto
    {
        public DateOnly Duedate { get; set; }

        public DateOnly? Returndate { get; set; }

     
    }
}