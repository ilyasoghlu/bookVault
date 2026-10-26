namespace BookVault.Application.DTOs.Room.Requests;

public class CreateRoomDto
{
    public string Title { get; set; } = string.Empty;

    public int Roomnumber { get; set; }

    public int Capacity { get; set; }

    public int Roomfloor { get; set; }

    public string Description { get; set; } = string.Empty;


}