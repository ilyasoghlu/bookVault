using BookVault.Application.Interfaces.Repositories;
using BookVault.Domain.Entities;
using BookVault.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookVault.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
	private readonly BookVaultContext _context;

	public EmployeeRepository(BookVaultContext context)
	{
		_context = context;
	}

	public async Task<IEnumerable<Employee>> GetAllAsync()
	{
		return await _context.Employees
			.Include(x => x.Rooms)
			.Include(x => x.Sections)
			.ToListAsync();
	}

	public async Task<Employee?> GetByIdAsync(int id)
	{
		return await _context.Employees
			.Include(x => x.Rooms)
			.Include(x => x.Sections)
			.FirstOrDefaultAsync(x => x.Id == id);
	}

	public async Task<Employee?> GetByFirstnameAsync(string firstname)
	{
		return await _context.Employees
			.FirstOrDefaultAsync(x => x.Firstname == firstname);
	}

	public async Task<Employee?> GetByLastnameAsync(string lastname)
	{
		return await _context.Employees
			.FirstOrDefaultAsync(x => x.Lastname == lastname);
	}

	public async Task<Employee?> GetByEmailAsync(string email)
	{
		var normalizedEmail = email.Trim().ToLower();

		return await _context.Employees
			.FirstOrDefaultAsync(x =>
				x.Email.ToLower() == normalizedEmail);
	}

	public async Task<Employee?> GetByRoomNumberAsync(int roomnumber)
	{
		return await _context.Employees
			.Include(x => x.Rooms)
			.FirstOrDefaultAsync(x =>
				x.Rooms.Any(r => r.Roomnumber == roomnumber));
	}

	public async Task AddAsync(Employee employee)
	{
		await _context.Employees.AddAsync(employee);
	}

	public void Update(Employee employee)
	{
		_context.Employees.Update(employee);
	}

	public void Delete(Employee employee)
	{
		_context.Employees.Remove(employee);
	}

	public async Task SaveAsync()
	{
		await _context.SaveChangesAsync();
	}
}