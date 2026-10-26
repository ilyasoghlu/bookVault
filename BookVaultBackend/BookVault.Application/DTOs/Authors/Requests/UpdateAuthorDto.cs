namespace BookVault.Application.DTOs.Authors.Requests;

public class UpdateAuthorDto
{
    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public DateOnly Birthdate { get; set; }

    public DateOnly? Deathdate { get; set; }

    public string Nationality { get; set; } = null!;

    public string Biography { get; set; } = null!;

    public Guid? Publisherid { get; set; }
}