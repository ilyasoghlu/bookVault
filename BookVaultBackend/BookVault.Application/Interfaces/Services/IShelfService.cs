using BookVault.Application.DTOs.Shelf.Requests;
using BookVault.Application.DTOs.Shelf.Responses;

namespace BookVault.Application.Interfaces.Services;

public interface IShelfService
{
    Task<IEnumerable<ShelfDto>> GetAllAsync();

    Task<ShelfDetailsDto?> GetByIdAsync(int id);

    Task<ShelfDto> CreateAsync(CreateShelfDto dto);

    Task UpdateAsync(int id, UpdateShelfDto dto);

    Task DeleteAsync(int id);
}