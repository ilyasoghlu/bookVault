namespace BookVault.Application.DTOs.Publishers.Requests;

public class CreatePublisherDto
{
    public string Publishername { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Website { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;
}