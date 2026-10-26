namespace BookVault.Application.DTOs.Publishers.Responses;

public class PublisherDto
{
    public Guid Id { get; set; }

    public string Publishername { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Website { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public DateTime? Createdat { get; set; }
}