namespace BookVault.Application.DTOs.BookCase.Responses;

public class BookCaseDetailsDto
{
    public int Id { get; set; }

    public int Casenumber { get; set; }

    public int Capacity { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public int? Sectionid { get; set; }

    public string? SectionTitle { get; set; }

    public int? Roomid { get; set; }

    public string? RoomTitle { get; set; }
}