using BookVault.Application.DTOs.Room.Responses;
using BookVault.Application.DTOs.Section.Responses;

namespace BookVault.Application.DTOs.Employee.Responses;

public class EmployeeDetailsDto
{
    public int Id { get; set; }

    public string Firstname { get; set; } = string.Empty;

    public string Lastname { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Employeeposition { get; set; } = string.Empty;

    public DateOnly Hiredate { get; set; }

    public DateOnly? Enddate { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public List<RoomDto> Rooms { get; set; } = new();

    public List<SectionDto> Sections { get; set; } = new();
}