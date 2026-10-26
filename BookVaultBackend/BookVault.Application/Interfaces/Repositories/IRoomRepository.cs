using BookVault.Domain.Entities;

namespace BookVault.Application.Interfaces.Repositories;

public interface IRoomRepository
{
    Task<IEnumerable<Room>> GetAllAsync();

    Task<Room?> GetByIdAsync(int id);

    Task<Room?> GetByTitleAsync(string title);

    Task<Room?> GetByRoomNumberAsync(int roomnumber);

    Task AddAsync(Room room);

    void Update(Room room);

    void Delete(Room room);

    Task SaveAsync();
}