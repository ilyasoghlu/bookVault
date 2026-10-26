using BookVault.Application.DTOs.Employee.Requests;
using BookVault.Application.DTOs.Employee.Responses;

namespace BookVault.Application.Interfaces.Services;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAllAsync();

    Task<EmployeeDetailsDto?> GetByIdAsync(int id);

    Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto);

    Task UpdateAsync(int id, UpdateEmployeeDto dto);

    Task DeleteAsync(int id);
}