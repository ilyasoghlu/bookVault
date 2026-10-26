namespace BookVault.Application.DTOs.Room.Responses;

public class RoomDetailsDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public int Roomfloor { get; set; }

    public int Roomnumber { get; set; }

    public int Capacity { get; set; }

    public string Description { get; set; } = string.Empty;

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public int? Employeeid { get; set; }

    public string? EmployeeName { get; set; }
}