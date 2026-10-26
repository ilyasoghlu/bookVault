namespace BookVault.Application.DTOs.Shelf.Requests;

public class CreateShelfDto
{
    public int Shelvecode { get; set; }

    public int Capacity { get; set; }

    public int Caseid { get; set; }

}