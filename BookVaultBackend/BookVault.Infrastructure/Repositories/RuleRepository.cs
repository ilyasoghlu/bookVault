using BookVault.Application.Interfaces.Repositories;
using BookVault.Domain.Entities;
using BookVault.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookVault.Infrastructure.Repositories;

public class RuleRepository : IRuleRepository
{
	private readonly BookVaultContext _context;

	public RuleRepository(BookVaultContext context)
	{
		_context = context;
	}

	public async Task<Rule?> GetByCodeAsync(string code)
	{
		return await _context.Rules
			.FirstOrDefaultAsync(x => x.Code == code);
	}

	public async Task AddAsync(Rule rule)
	{
		await _context.Rules.AddAsync(rule);
	}

	public async Task SaveChangesAsync()
	{
		await _context.SaveChangesAsync();
	}
}
