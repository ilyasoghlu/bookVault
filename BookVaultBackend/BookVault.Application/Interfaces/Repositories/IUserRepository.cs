using BookVault.Domain.Entities;

namespace BookVault.Application.Interfaces.Repositories;

public interface IUserRepository
{
	Task<User?> GetByEmailAsync(string email);

	Task<User?> GetByUsernameAsync(string username);

	Task<User?> GetByIdAsync(Guid id);

	Task AddAsync(User user);

	void Delete(User user);

	Task SaveChangesAsync();
}
