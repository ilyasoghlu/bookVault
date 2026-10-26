namespace BookVault.Application.DTOs.Reader.Requests;

public class CreateReaderDto
{
	public string Firstname { get; set; } = null!;

	public string Lastname { get; set; } = null!;

	public string Email { get; set; } = null!;

	public string Username { get; set; } = null!;

	public string Password { get; set; } = null!;

	public string Phone { get; set; } = null!;

	public DateOnly Dateofbirth { get; set; }

	public DateOnly Expirationdate { get; set; }
}