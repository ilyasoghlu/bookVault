namespace BookVault.Application.DTOs.Rules;

public class AssignPermissionRequest
{
	public Guid UserId { get; set; }

	public string RuleCode { get; set; } = null!;

	public DateTimeOffset? ExpiresAt { get; set; }
}
