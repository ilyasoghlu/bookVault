using BookVault.Application.Exceptions;
using BookVault.Application.Interfaces.Repositories;
using BookVault.Application.Interfaces.Services;
using BookVault.Domain.Entities;

namespace BookVault.Application.Services;

public class RuleService : IRuleService
{
	private readonly IUserRuleRepository _userRuleRepository;
	private readonly IRuleRepository _ruleRepository;
	private readonly IUserRepository _userRepository;

	public RuleService(
		IUserRuleRepository userRuleRepository,
		IRuleRepository ruleRepository,
		IUserRepository userRepository)
	{
		_userRuleRepository = userRuleRepository;
		_ruleRepository = ruleRepository;
		_userRepository = userRepository;
	}

	public async Task<bool> HasPermissionAsync(
		Guid userId,
		string ruleCode)
	{
		return await _userRuleRepository.HasPermissionAsync(
			userId,
			ruleCode);
	}

	public async Task CreateAsync(
		string code,
		string ruleName,
		string? description)
	{
		var existingRule =
			await _ruleRepository.GetByCodeAsync(code);

		if (existingRule is not null)
		{
			throw new ConflictException(
				"A rule with this code already exists.");
		}

		var rule = new Rule
		{
			Code = code,
			Rulename = ruleName,
			Description = description,
			Isactive = true,
			Createdat = DateTime.UtcNow
		};

		await _ruleRepository.AddAsync(rule);
		await _ruleRepository.SaveChangesAsync();
	}

	public async Task AssignPermissionAsync(
		Guid userId,
		string ruleCode,
		Guid? assignedBy = null,
		DateTimeOffset? expiresAt = null)
	{
		var user = await _userRepository.GetByIdAsync(userId);

		if (user is null)
		{
			throw new NotFoundException(
				"User not found.");
		}

		var rule = await _ruleRepository.GetByCodeAsync(ruleCode);

		if (rule is null)
		{
			throw new NotFoundException(
				"Rule not found.");
		}

		if (!rule.Isactive)
		{
			throw new ConflictException(
				"Rule is inactive.");
		}

		var existingUserRule =
			await _userRuleRepository.GetByUserAndRuleAsync(
				userId,
				rule.Id);

		if (existingUserRule is not null)
		{
			existingUserRule.Isallowed = true;
			existingUserRule.Assignedat = DateTimeOffset.UtcNow;
			existingUserRule.Assignedby = assignedBy;
			existingUserRule.Expiresat = expiresAt;
		}
		else
		{
			var userRule = new UserRule
			{
				Userid = userId,
				Ruleid = rule.Id,
				Isallowed = true,
				Assignedat = DateTimeOffset.UtcNow,
				Assignedby = assignedBy,
				Expiresat = expiresAt
			};

			await _userRuleRepository.AddAsync(userRule);
		}

		await _userRuleRepository.SaveChangesAsync();
	}
}
