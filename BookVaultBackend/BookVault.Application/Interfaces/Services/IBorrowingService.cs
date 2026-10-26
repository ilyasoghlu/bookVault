using BookVault.Application.DTOs.Borrowings.Requests;
using BookVault.Application.DTOs.Borrowings.Responses;

namespace BookVault.Application.Interfaces.Services;

public interface IBorrowingService
{
    Task<IEnumerable<BorrowingDetailsDto>> GetAllAsync();

    Task<BorrowingDetailsDto?> GetByIdAsync(int id);

    Task<BorrowingDetailsDto> CreateAsync(CreateBorrowingDto dto);

    Task UpdateAsync(int id, UpdateBorrowingDto dto);

}