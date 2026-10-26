namespace BookVault.Application.DTOs.Section.Responses;

public class SectionDto
{
	public int Id { get; set; }

	public string Title { get; set; } = string.Empty;

	public int Sectionnumber { get; set; }

	public int Capacity { get; set; }

	public int Sectionfloor { get; set; }

	public string Description { get; set; } = string.Empty;

	public DateTime? Createdat { get; set; }

	public DateTime? Updatedat { get; set; }

	public int? Employeeid { get; set; }

	public int Roomid { get; set; }
}