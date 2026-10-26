using System.ComponentModel.DataAnnotations;

namespace BookVault.Application.DTOs.Books.Requests;

public class CreateBookDto
{
    [Required]
    [MaxLength(36)]
    public string Isbn { get; set; } = null!;

    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string Genre { get; set; } = null!;

    public DateOnly Publicationdate { get; set; }

    [Range(1, int.MaxValue)]
    public int Pagecount { get; set; }

    [Required]
    [MaxLength(50)]
    public string Edition { get; set; } = null!;

    [Range(1, int.MaxValue)]
    public int Totalcopies { get; set; }

    [Required]
    [MaxLength(50)]
    public string Language { get; set; } = null!;

    [Required]
    public Guid? Authorid { get; set; }

    [Required]
    public Guid? Publisherid { get; set; }

    [Required]
    public int? Shelfid { get; set; }
}