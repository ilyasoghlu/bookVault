using BookVault.Application.DTOs.Room.Requests;
using BookVault.Application.DTOs.Room.Responses;
using BookVault.Application.Exceptions;
using BookVault.Application.Interfaces.Repositories;
using BookVault.Application.Interfaces.Services;
using BookVault.Domain.Entities;

namespace BookVault.Application.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;

    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<IEnumerable<RoomDto>> GetAllAsync()
    {
        var rooms = await _roomRepository.GetAllAsync();

        return rooms.Select(MapToDto).ToList();
    }

    public async Task<RoomDetailsDto?> GetByIdAsync(int id)
    {
        var room = await _roomRepository.GetByIdAsync(id);

        if (room == null)
            return null;

        return new RoomDetailsDto
        {
            Id = room.Id,
            Title = room.Title,
            Roomnumber = room.Roomnumber,
            Capacity = room.Capacity,
            Roomfloor = room.Roomfloor,
            Description = room.Description,
            Employeeid = room.Employeeid,
          

            EmployeeName = room.Employee == null
                ? null
                : $"{room.Employee.Firstname} {room.Employee.Lastname}",

            Createdat = room.Createdat,
            Updatedat = room.Updatedat
        };
    }

    public async Task<RoomDto> CreateAsync(
        CreateRoomDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            throw new BadRequestException(
                "Room title is required."
            );
        }

        var existingRoom =
            await _roomRepository.GetByRoomNumberAsync(
                dto.Roomnumber);

        if (existingRoom != null)
        {
            throw new ConflictException(
                "Room with this number already exists."
            );
        }

        var now = DateTime.UtcNow;

        var room = new Room
        {
            Title = dto.Title,
            Roomnumber = dto.Roomnumber,
            Capacity = dto.Capacity,
            Roomfloor = dto.Roomfloor,
            Description = dto.Description,      

            Createdat = now,
            Updatedat = now
        };

        await _roomRepository.AddAsync(room);
        await _roomRepository.SaveAsync();

        return MapToDto(room);
    }

    public async Task UpdateAsync(
        int id,
        UpdateRoomDto dto)
    {
        var room = await _roomRepository.GetByIdAsync(id);

        if (room == null)
        {
            throw new NotFoundException(
                "Room not found."
            );
        }

        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            throw new BadRequestException(
                "Room title is required."
            );
        }

        var existingRoom =
            await _roomRepository.GetByRoomNumberAsync(
                dto.Roomnumber);

        if (existingRoom != null &&
            existingRoom.Id != id)
        {
            throw new ConflictException(
                "Room with this number already exists."
            );
        }

        room.Title = dto.Title;
        room.Roomnumber = dto.Roomnumber;
        room.Capacity = dto.Capacity;
        room.Description = dto.Description;
        room.Employeeid = dto.Employeeid;
    
        room.Createdat = room.Createdat.HasValue
            ? DateTime.SpecifyKind(
                room.Createdat.Value,
                DateTimeKind.Utc)
            : null;

        room.Updatedat = DateTime.UtcNow;

        _roomRepository.Update(room);

        await _roomRepository.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var room = await _roomRepository.GetByIdAsync(id);

        if (room == null)
        {
            throw new NotFoundException(
                "Room not found."
            );
        }

        if (room.Sections.Any())
        {
            throw new ConflictException(
				"This room contains sections. Please move the sections to another room before deleting this room.");
            
        }


        _roomRepository.Delete(room);

        await _roomRepository.SaveAsync();
    }

    private static RoomDto MapToDto(Room room)
    {
        return new RoomDto
        {
            Id = room.Id,
            Title = room.Title,
            Roomnumber = room.Roomnumber,
            Capacity = room.Capacity,
            Roomfloor = room.Roomfloor,
            Description = room.Description,
            Employeeid = room.Employeeid,
       
        };
    }
}