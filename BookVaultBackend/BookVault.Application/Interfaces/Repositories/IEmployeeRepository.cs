using BookVault.Domain.Entities;

namespace BookVault.Application.Interfaces.Repositories;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync();

    Task<Employee?> GetByIdAsync(int id);

    Task<Employee?> GetByFirstnameAsync(string firstname);

    Task<Employee?> GetByLastnameAsync(string lastname);

    Task<Employee?> GetByEmailAsync(string email);

    Task<Employee?> GetByRoomNumberAsync(int roomnumber);

    Task AddAsync(Employee employee);

    void Update(Employee employee);

    void Delete(Employee employee);

    Task SaveAsync();
}