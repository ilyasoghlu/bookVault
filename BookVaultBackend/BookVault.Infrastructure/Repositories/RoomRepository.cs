using BookVault.Application.Interfaces.Repositories;
using BookVault.Domain.Entities;
using BookVault.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookVault.Infrastructure.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly BookVaultContext _context;

    public RoomRepository(BookVaultContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Room>> GetAllAsync()
    {
        return await _context.Rooms
            .Include(x => x.Employee)
            //.Include(x => x.Section)
            .ToListAsync();
    }

    public async Task<Room?> GetByIdAsync(int id)
    {
        return await _context.Rooms
            .Include(x => x.Employee)
            .Include(x => x.Sections)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Room?> GetByTitleAsync(string title)
    {
        return await _context.Rooms
            .FirstOrDefaultAsync(x => x.Title == title);
    }

    public async Task<Room?> GetByRoomNumberAsync(int roomnumber)
    {
        return await _context.Rooms
            .FirstOrDefaultAsync(x => x.Roomnumber == roomnumber);
    }

    public async Task AddAsync(Room room)
    {
        await _context.Rooms.AddAsync(room);
    }

    public void Update(Room room)
    {
        _context.Rooms.Update(room);
    }

    public void Delete(Room room)
    {
        _context.Rooms.Remove(room);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}