using BookVault.Application.DTOs.Room.Requests;
using BookVault.Application.DTOs.Room.Responses;

namespace BookVault.Application.Interfaces.Services;

public interface IRoomService
{
    Task<IEnumerable<RoomDto>> GetAllAsync();

    Task<RoomDetailsDto?> GetByIdAsync(int id);

    Task<RoomDto> CreateAsync(CreateRoomDto dto);

    Task UpdateAsync(int id, UpdateRoomDto dto);

    Task DeleteAsync(int id);
}