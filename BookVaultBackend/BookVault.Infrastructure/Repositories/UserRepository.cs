using BookVault.Application.Interfaces.Repositories;
using BookVault.Domain.Entities;
using BookVault.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookVault.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
	private readonly BookVaultContext _context;

	public UserRepository(BookVaultContext context)
	{
		_context = context;
	}

	public async Task<User?> GetByEmailAsync(string email)
	{
		return await _context.Users
			.FirstOrDefaultAsync(x => x.Email == email);
	}

	public async Task<User?> GetByUsernameAsync(string username)
	{
		return await _context.Users
			.FirstOrDefaultAsync(x => x.Username == username);
	}

	public async Task AddAsync(User user)
	{
		await _context.Users.AddAsync(user);
	}
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
			.FirstOrDefaultAsync(x => x.Id == id);

	}
	public void Delete(User user)
	{
		_context.Users.Remove(user);
	}

	public async Task SaveChangesAsync()
	{
		await _context.SaveChangesAsync();
	}

}
