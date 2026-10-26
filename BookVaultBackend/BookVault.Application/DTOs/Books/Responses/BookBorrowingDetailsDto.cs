

namespace BookVault.Application.DTOs.Books.Responses
{
    public class BookBorrowingDetailsDto
    {

        public int TotalCopies { get; set; }
        public int BorrowedCopies { get; set; }
        public int AvailableCopies { get; set; }

        public List<ActiveBorrowingDto> ActiveBorrowings { get; set; }
            = new();
       
        
    }
}
