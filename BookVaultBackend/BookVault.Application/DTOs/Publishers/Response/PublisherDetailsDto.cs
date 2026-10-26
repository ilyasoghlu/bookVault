using BookVault.Application.DTOs.Authors.Responses;
using BookVault.Application.DTOs.Books.Responses;

namespace BookVault.Application.DTOs.Publisher.Responses;

public class PublisherDetailsDto
{
    public Guid Id { get; set; }

    public string Publishername { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string Website { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public List<AuthorDto> Authors { get; set; } = new();

    public List<BookDto> Books { get; set; } = new();
}