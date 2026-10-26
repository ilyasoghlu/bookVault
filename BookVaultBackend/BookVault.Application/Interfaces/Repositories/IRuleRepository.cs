using BookVault.Domain.Entities;

namespace BookVault.Application.Interfaces.Repositories;

public interface IRuleRepository
{
	Task<Rule?> GetByCodeAsync(string code);

	Task AddAsync(Rule rule);

	Task SaveChangesAsync();
}
