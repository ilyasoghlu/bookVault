namespace BookVault.Application.Interfaces.Services;

public interface IRuleService
{
	Task<bool> HasPermissionAsync(
		Guid userId,
		string ruleCode);

	Task CreateAsync(
		string code,
		string ruleName,
		string? description);

	Task AssignPermissionAsync(
		Guid userId,
		string ruleCode,
		Guid? assignedBy = null,
		DateTimeOffset? expiresAt = null);

}
