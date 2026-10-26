namespace BookVault.Application.DTOs.Section.Requests;

public class UpdateSectionDto
{
    public string Title { get; set; } = null!;

    public int Sectionnumber { get; set; }

    public int Capacity { get; set; }

    public int Sectionfloor { get; set; }

    public string Description { get; set; } = null!;

    public int? Employeeid { get; set; }

	public int Roomid { get; set; }
}