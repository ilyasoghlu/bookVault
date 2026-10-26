namespace BookVault.Application.DTOs.Reader.Requests;

public class UpdateReaderDto
{
	public string Firstname { get; set; } = null!;

	public string Lastname { get; set; } = null!;

	public string Email { get; set; } = null!;

	public string Phone { get; set; } = null!;

	public DateOnly Dateofbirth { get; set; }

	public DateOnly Expirationdate { get; set; }
}