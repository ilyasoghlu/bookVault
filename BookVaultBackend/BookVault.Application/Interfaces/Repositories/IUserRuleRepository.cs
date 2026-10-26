using BookVault.Domain.Entities;

namespace BookVault.Application.Interfaces.Repositories;

public interface IUserRuleRepository
{
	Task<bool> HasPermissionAsync(
		Guid userId,
		string ruleCode);

	Task<List<string>> GetUserPermissionsAsync(
	Guid userId);

	Task<UserRule?> GetByUserAndRuleAsync(
		Guid userId, 
		Guid ruleId);

	Task AddAsync(UserRule userRule);

	Task SaveChangesAsync();
}
