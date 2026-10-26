using BookVault.Application.Interfaces.Repositories;
using BookVault.Domain.Entities;
using BookVault.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookVault.Infrastructure.Repositories;

public class UserRuleRepository : IUserRuleRepository
{
	private readonly BookVaultContext _context;

	public UserRuleRepository(BookVaultContext context)
	{
		_context = context;
	}

	public async Task<bool> HasPermissionAsync(
		Guid userId,
		string ruleCode)
	{
		return await _context.UserRules
			.AnyAsync(x =>
				x.Userid == userId &&
				x.Isallowed &&
				x.Rule.Isactive &&
				x.Rule.Code == ruleCode &&
				(x.Expiresat == null ||
				 x.Expiresat > DateTimeOffset.UtcNow));
	}

	public async Task<List<string>> GetUserPermissionsAsync(
	Guid userId)
	{
		return await _context.UserRules
			.Where(x =>
				x.Userid == userId &&
				x.Isallowed &&
				x.Rule.Isactive &&
				(x.Expiresat == null ||
				 x.Expiresat > DateTimeOffset.UtcNow))
			.Select(x => x.Rule.Code)
			.ToListAsync();
	}

	public async Task<UserRule?> GetByUserAndRuleAsync(
		Guid userId,
		Guid ruleId)
		{
			return await _context.UserRules
				.FirstOrDefaultAsync(x =>
					x.Userid == userId &&
					x.Ruleid == ruleId);
	}

	public async Task AddAsync(UserRule userRule)
	{
		await _context.UserRules.AddAsync(userRule);
	}

	public async Task SaveChangesAsync()
	{
		await _context.SaveChangesAsync();
	}
}
